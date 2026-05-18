using AutoMapper;
using Moq;
using SmartStay.Application.DTOs.MeterReading;
using SmartStay.Domain.Entities;
using SmartStay.Domain.Exceptions;
using SmartStay.Domain.Interfaces;
using SmartStay.Infrastructure.Services;

namespace SmartStay.Tests.MeterReadings;

public class MeterReadingServiceConstraintTests
{
    [Fact]
    public async Task CreateMeterReadingAsync_ThrowsBadRequest_WhenNewUnitIsLessThanOldUnit()
    {
        var landlordId = Guid.NewGuid();
        var roomId = Guid.NewGuid();

        var service = CreateService(
            out _,
            out _,
            out _,
            out _,
            out _);

        var request = new CreateMeterReadingRequest
        {
            RoomId = roomId,
            Type = "Electric",
            OldUnit = 50,
            NewUnit = 40,
            Month = 5,
            Year = 2026
        };

        await Assert.ThrowsAsync<BadRequestException>(() => service.CreateMeterReadingAsync(landlordId, request));
    }

    [Fact]
    public async Task CreateMeterReadingAsync_ThrowsBadRequest_WhenReadingAlreadyExistsForRoomTypeMonthAndYear()
    {
        var landlordId = Guid.NewGuid();
        var propertyId = Guid.NewGuid();
        var roomId = Guid.NewGuid();

        var service = CreateService(
            out var meterReadingRepository,
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

        meterReadingRepository
            .Setup(x => x.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<MeterReading, bool>>>()))
            .ReturnsAsync(new[]
            {
                new MeterReading
                {
                    Id = Guid.NewGuid(),
                    RoomId = roomId,
                    Type = "Electric",
                    Month = 5,
                    Year = 2026,
                    OldUnit = 20,
                    NewUnit = 40
                }
            });

        mapper
            .Setup(x => x.Map<MeterReading>(It.IsAny<CreateMeterReadingRequest>()))
            .Returns((CreateMeterReadingRequest source) => new MeterReading
            {
                RoomId = source.RoomId,
                Type = source.Type,
                OldUnit = source.OldUnit,
                NewUnit = source.NewUnit,
                PhotoUrl = source.PhotoUrl,
                Month = source.Month,
                Year = source.Year
            });

        var request = new CreateMeterReadingRequest
        {
            RoomId = roomId,
            Type = "Electric",
            OldUnit = 40,
            NewUnit = 50,
            Month = 5,
            Year = 2026
        };

        await Assert.ThrowsAsync<BadRequestException>(() => service.CreateMeterReadingAsync(landlordId, request));
    }

    private static MeterReadingService CreateService(
        out Mock<IRepository<MeterReading>> meterReadingRepository,
        out Mock<IRepository<Room>> roomRepository,
        out Mock<IRepository<Property>> propertyRepository,
        out Mock<IUnitOfWork> unitOfWork,
        out Mock<IMapper> mapper)
    {
        meterReadingRepository = new Mock<IRepository<MeterReading>>();
        roomRepository = new Mock<IRepository<Room>>();
        propertyRepository = new Mock<IRepository<Property>>();
        unitOfWork = new Mock<IUnitOfWork>();
        mapper = new Mock<IMapper>();

        return new MeterReadingService(
            meterReadingRepository.Object,
            roomRepository.Object,
            propertyRepository.Object,
            unitOfWork.Object,
            mapper.Object);
    }
}
