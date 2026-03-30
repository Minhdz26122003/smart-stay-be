using System;

namespace SmartStay.Domain.Entities;

public class Review : BaseEntity
{
    public Guid ReviewerId { get; set; }
    public Guid RevieweeId { get; set; }
    public Guid ContractId { get; set; }
    public int Rating { get; set; } // 1-5
    public string? Comment { get; set; }
}
