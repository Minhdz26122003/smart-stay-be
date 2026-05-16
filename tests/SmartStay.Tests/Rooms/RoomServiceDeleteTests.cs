using AutoMapper;
using Moq;
using SmartStay.Domain.Entities;
using SmartStay.Domain.Enums;
using SmartStay.Domain.Exceptions;
using SmartStay.Domain.Interfaces;
using SmartStay.Infrastructure.Services;

namespace SmartStay.Tests.Rooms;

public class RoomServiceDeleteTests
{
    [Fact]
    public async Task DeleteRoomAsync_ThrowsBadRequest_WhenRoomHasActiveContract()
    {
        var landlordId = Guid.NewGuid();
        var roomId = Guid.NewGuid();
        var propertyId = Guid.NewGuid();

        var roomRepository = new Mock<IRepository<Room>>();
        var propertyRepository = new Mock<IRepository<Property>>();
        var contractRepository = new Mock<IRepository<Contract>>();
        var userRepository = new Mock<IRepository<User>>();
        var invoiceRepository = new Mock<IRepository<Invoice>>();
        var inventoryItemRepository = new Mock<IRepository<InventoryItem>>();
        var meterReadingRepository = new Mock<IRepository<MeterReading>>();
        var unitOfWork = new Mock<IUnitOfWork>();
        var mapper = new Mock<IMapper>();

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
                new Contract { RoomId = roomId, Status = ContractStatus.Active }
            });

        var service = new RoomService(
            roomRepository.Object,
            propertyRepository.Object,
            contractRepository.Object,
            userRepository.Object,
            invoiceRepository.Object,
            inventoryItemRepository.Object,
            meterReadingRepository.Object,
            unitOfWork.Object,
            mapper.Object);

        await Assert.ThrowsAsync<BadRequestException>(() => service.DeleteRoomAsync(landlordId, roomId));
    }
}
