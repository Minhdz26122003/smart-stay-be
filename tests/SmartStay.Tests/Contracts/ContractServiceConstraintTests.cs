using AutoMapper;
using Moq;
using SmartStay.Application.DTOs.Contract;
using SmartStay.Domain.Entities;
using SmartStay.Domain.Enums;
using SmartStay.Domain.Exceptions;
using SmartStay.Domain.Interfaces;
using SmartStay.Infrastructure.Services;

namespace SmartStay.Tests.Contracts;

public class ContractServiceConstraintTests
{
    [Fact]
    public async Task CreateContractAsync_ThrowsBadRequest_WhenEndDateIsNotAfterStartDate()
    {
        var service = CreateService(
            out _,
            out _,
            out _,
            out _,
            out _);

        var landlordId = Guid.NewGuid();
        var roomId = Guid.NewGuid();
        var tenantId = Guid.NewGuid();

        var request = new CreateContractRequest
        {
            RoomId = roomId,
            TenantId = tenantId,
            DepositAmount = 1000000,
            StartDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            EndDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        };

        await Assert.ThrowsAsync<BadRequestException>(() => service.CreateContractAsync(landlordId, request));
    }

    [Fact]
    public async Task CreateContractAsync_ThrowsBadRequest_WhenRoomAlreadyHasActiveContract()
    {
        var roomId = Guid.NewGuid();
        var propertyId = Guid.NewGuid();
        var landlordId = Guid.NewGuid();

        var service = CreateService(
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
            .Setup(x => x.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Contract, bool>>>()))
            .ReturnsAsync(new[]
            {
                new Contract
                {
                    Id = Guid.NewGuid(),
                    RoomId = roomId,
                    TenantId = Guid.NewGuid(),
                    StartDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    EndDate = new DateTime(2026, 12, 31, 0, 0, 0, DateTimeKind.Utc),
                    Status = ContractStatus.Active
                }
            });

        mapper
            .Setup(x => x.Map<Contract>(It.IsAny<CreateContractRequest>()))
            .Returns((CreateContractRequest source) => new Contract
            {
                RoomId = source.RoomId,
                TenantId = source.TenantId,
                DepositAmount = source.DepositAmount,
                StartDate = source.StartDate,
                EndDate = source.EndDate,
                Status = ContractStatus.Active
            });

        var request = new CreateContractRequest
        {
            RoomId = roomId,
            TenantId = Guid.NewGuid(),
            DepositAmount = 2000000,
            StartDate = new DateTime(2026, 2, 1, 0, 0, 0, DateTimeKind.Utc),
            EndDate = new DateTime(2026, 12, 31, 0, 0, 0, DateTimeKind.Utc)
        };

        await Assert.ThrowsAsync<BadRequestException>(() => service.CreateContractAsync(landlordId, request));
    }

    private static ContractService CreateService(
        out Mock<IRepository<Contract>> contractRepository,
        out Mock<IRepository<Room>> roomRepository,
        out Mock<IRepository<Property>> propertyRepository,
        out Mock<IUnitOfWork> unitOfWork,
        out Mock<IMapper> mapper)
    {
        contractRepository = new Mock<IRepository<Contract>>();
        roomRepository = new Mock<IRepository<Room>>();
        propertyRepository = new Mock<IRepository<Property>>();
        unitOfWork = new Mock<IUnitOfWork>();
        mapper = new Mock<IMapper>();

        return new ContractService(
            contractRepository.Object,
            roomRepository.Object,
            propertyRepository.Object,
            unitOfWork.Object,
            mapper.Object);
    }
}
