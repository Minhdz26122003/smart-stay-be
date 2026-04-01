using System;

namespace SmartStay.Application.DTOs.Roommate;

public class RoommateDto
{
    public Guid Id { get; set; }
    public Guid ContractId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? CccdPhotoUrl { get; set; }
    public bool IsApproved { get; set; }
    public DateTime CreatedAt { get; set; }
}
