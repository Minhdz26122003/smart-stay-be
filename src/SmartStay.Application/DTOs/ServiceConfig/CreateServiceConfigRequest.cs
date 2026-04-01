using System;
using SmartStay.Domain.Enums;

namespace SmartStay.Application.DTOs.ServiceConfig;

public class CreateServiceConfigRequest
{
    public Guid PropertyId { get; set; }
    public Guid? RoomId { get; set; }
    public string Type { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public ServiceCalcMethod CalcMethod { get; set; }
}
