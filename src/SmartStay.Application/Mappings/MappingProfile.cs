using AutoMapper;
using SmartStay.Domain.Entities;
using SmartStay.Application.DTOs.Auth;

using SmartStay.Application.DTOs.Property;
using SmartStay.Application.DTOs.Room;
using SmartStay.Application.DTOs.Contract;
using SmartStay.Application.DTOs.Invoice;

namespace SmartStay.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Auth
        CreateMap<User, AuthResponse>();
        CreateMap<RegisterRequest, User>();

        // Property
        CreateMap<Property, PropertyDto>()
            .ForMember(d => d.Street, opt => opt.MapFrom(s => s.Address != null ? s.Address.Street : string.Empty))
            .ForMember(d => d.Ward, opt => opt.MapFrom(s => s.Address != null ? s.Address.Ward : string.Empty))
            .ForMember(d => d.District, opt => opt.MapFrom(s => s.Address != null ? s.Address.District : string.Empty))
            .ForMember(d => d.City, opt => opt.MapFrom(s => s.Address != null ? s.Address.City : string.Empty));

        CreateMap<CreatePropertyRequest, Property>()
            .ForMember(d => d.Address, opt => opt.MapFrom(s => new Domain.ValueObjects.Address(s.Street, s.Ward, s.District, s.City)));

        CreateMap<UpdatePropertyRequest, Property>()
            .ForMember(d => d.Address, opt => opt.MapFrom(s => new Domain.ValueObjects.Address(s.Street, s.Ward, s.District, s.City)));

        // Room
        CreateMap<Room, RoomDto>();
        CreateMap<CreateRoomRequest, Room>();
        CreateMap<UpdateRoomRequest, Room>();

        // Contract
        CreateMap<Contract, ContractDto>();
        CreateMap<CreateContractRequest, Contract>();
        CreateMap<UpdateContractRequest, Contract>();

        // Invoice
        CreateMap<Invoice, InvoiceDto>();
        CreateMap<CreateInvoiceRequest, Invoice>();
        CreateMap<UpdateInvoiceRequest, Invoice>();

        // ServiceConfig
        CreateMap<ServiceConfig, SmartStay.Application.DTOs.ServiceConfig.ServiceConfigDto>();
        CreateMap<SmartStay.Application.DTOs.ServiceConfig.CreateServiceConfigRequest, ServiceConfig>();
        CreateMap<SmartStay.Application.DTOs.ServiceConfig.UpdateServiceConfigRequest, ServiceConfig>();

        // MeterReading
        CreateMap<MeterReading, SmartStay.Application.DTOs.MeterReading.MeterReadingDto>();
        CreateMap<SmartStay.Application.DTOs.MeterReading.CreateMeterReadingRequest, MeterReading>();
    }
}
