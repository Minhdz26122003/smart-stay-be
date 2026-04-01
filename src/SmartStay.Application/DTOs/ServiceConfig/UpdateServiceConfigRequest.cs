using SmartStay.Domain.Enums;

namespace SmartStay.Application.DTOs.ServiceConfig;

public class UpdateServiceConfigRequest
{
    public decimal UnitPrice { get; set; }
    public ServiceCalcMethod CalcMethod { get; set; }
}
