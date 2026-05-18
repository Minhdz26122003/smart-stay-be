using AutoMapper;
using Moq;
using SmartStay.Application.DTOs.Invoice;
using SmartStay.Domain.Entities;
using SmartStay.Domain.Enums;
using SmartStay.Domain.Exceptions;
using SmartStay.Domain.Interfaces;
using SmartStay.Infrastructure.Services;

namespace SmartStay.Tests.Invoices;

public class InvoiceServiceConstraintTests
{
    [Fact]
    public async Task CreateInvoiceAsync_ThrowsBadRequest_WhenInvoiceAlreadyExistsForContractMonthAndYear()
    {
        var landlordId = Guid.NewGuid();
        var propertyId = Guid.NewGuid();
        var roomId = Guid.NewGuid();
        var contractId = Guid.NewGuid();

        var service = CreateService(
            out var invoiceRepository,
            out var contractRepository,
            out var roomRepository,
            out var propertyRepository,
            out _,
            out var mapper);

        roomRepository
            .Setup(x => x.GetByIdAsync(roomId))
            .ReturnsAsync(new Room { Id = roomId, PropertyId = propertyId });

        propertyRepository
            .Setup(x => x.GetByIdAsync(propertyId))
            .ReturnsAsync(new Property { Id = propertyId, LandlordId = landlordId });

        contractRepository
            .Setup(x => x.GetByIdAsync(contractId))
            .ReturnsAsync(new Contract { Id = contractId, RoomId = roomId, TenantId = Guid.NewGuid() });

        invoiceRepository
            .Setup(x => x.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Invoice, bool>>>()))
            .ReturnsAsync(new[]
            {
                new Invoice
                {
                    Id = Guid.NewGuid(),
                    RoomId = roomId,
                    ContractId = contractId,
                    Month = 5,
                    Year = 2026,
                    TotalAmount = 100,
                    PaidAmount = 0,
                    Status = InvoiceStatus.Pending
                }
            });

        mapper
            .Setup(x => x.Map<Invoice>(It.IsAny<CreateInvoiceRequest>()))
            .Returns((CreateInvoiceRequest source) => new Invoice
            {
                RoomId = source.RoomId,
                ContractId = source.ContractId,
                Month = source.Month,
                Year = source.Year,
                BreakdownJson = source.BreakdownJson,
                TotalAmount = source.TotalAmount,
                PaidAmount = 0,
                Status = InvoiceStatus.Pending
            });

        var request = new CreateInvoiceRequest
        {
            RoomId = roomId,
            ContractId = contractId,
            Month = 5,
            Year = 2026,
            BreakdownJson = "{}",
            TotalAmount = 3000000
        };

        await Assert.ThrowsAsync<BadRequestException>(() => service.CreateInvoiceAsync(landlordId, request));
    }

    [Fact]
    public async Task UpdateInvoiceAsync_ThrowsBadRequest_WhenPaidAmountExceedsTotalAmount()
    {
        var landlordId = Guid.NewGuid();
        var propertyId = Guid.NewGuid();
        var roomId = Guid.NewGuid();
        var invoiceId = Guid.NewGuid();

        var service = CreateService(
            out _,
            out _,
            out var roomRepository,
            out var propertyRepository,
            out _,
            out _);

        var invoiceRepository = CreateInvoiceRepository(roomId, invoiceId);

        service = new InvoiceService(
            invoiceRepository.Object,
            new Mock<IRepository<Contract>>().Object,
            roomRepository.Object,
            propertyRepository.Object,
            new Mock<IUnitOfWork>().Object,
            new Mock<IMapper>().Object);

        roomRepository
            .Setup(x => x.GetByIdAsync(roomId))
            .ReturnsAsync(new Room { Id = roomId, PropertyId = propertyId });

        propertyRepository
            .Setup(x => x.GetByIdAsync(propertyId))
            .ReturnsAsync(new Property { Id = propertyId, LandlordId = landlordId });

        var request = new UpdateInvoiceRequest
        {
            PaidAmount = 2500000,
            Status = InvoiceStatus.Paid
        };

        await Assert.ThrowsAsync<BadRequestException>(() => service.UpdateInvoiceAsync(landlordId, invoiceId, request));
    }

    private static Mock<IRepository<Invoice>> CreateInvoiceRepository(Guid roomId, Guid invoiceId)
    {
        var invoiceRepository = new Mock<IRepository<Invoice>>();
        invoiceRepository
            .Setup(x => x.GetByIdAsync(invoiceId))
            .ReturnsAsync(new Invoice
            {
                Id = invoiceId,
                RoomId = roomId,
                ContractId = Guid.NewGuid(),
                Month = 5,
                Year = 2026,
                BreakdownJson = "{}",
                TotalAmount = 2000000,
                PaidAmount = 0,
                Status = InvoiceStatus.Pending
            });
        return invoiceRepository;
    }

    private static InvoiceService CreateService(
        out Mock<IRepository<Invoice>> invoiceRepository,
        out Mock<IRepository<Contract>> contractRepository,
        out Mock<IRepository<Room>> roomRepository,
        out Mock<IRepository<Property>> propertyRepository,
        out Mock<IUnitOfWork> unitOfWork,
        out Mock<IMapper> mapper)
    {
        invoiceRepository = new Mock<IRepository<Invoice>>();
        contractRepository = new Mock<IRepository<Contract>>();
        roomRepository = new Mock<IRepository<Room>>();
        propertyRepository = new Mock<IRepository<Property>>();
        unitOfWork = new Mock<IUnitOfWork>();
        mapper = new Mock<IMapper>();

        return new InvoiceService(
            invoiceRepository.Object,
            contractRepository.Object,
            roomRepository.Object,
            propertyRepository.Object,
            unitOfWork.Object,
            mapper.Object);
    }
}
