using System;

namespace SmartStay.Domain.Entities;

public class Roommate : BaseEntity
{
    public Guid ContractId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? CccdPhotoUrl { get; set; } // Encrypted URL
    public bool IsApproved { get; set; }

    public Contract? Contract { get; set; }
}
