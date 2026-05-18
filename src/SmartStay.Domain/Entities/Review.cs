using System;

namespace SmartStay.Domain.Entities;

public class Review : BaseEntity
{
    public Guid ReviewerId { get; set; }
    public Guid RevieweeId { get; set; }
    public Guid ContractId { get; set; }
    public int Rating { get; set; } // 1-5
    public string? Comment { get; set; }

    public User? Reviewer { get; set; }
    public User? Reviewee { get; set; }
    public Contract? Contract { get; set; }
}
