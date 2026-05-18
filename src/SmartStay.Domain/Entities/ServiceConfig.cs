using System;
using SmartStay.Domain.Enums;

namespace SmartStay.Domain.Entities;

public class ServiceConfig : BaseEntity
{
    public Guid PropertyId { get; set; }
    public Guid? RoomId { get; set; } // Null if it applies to the whole property
    public string Type { get; set; } = string.Empty; // e.g. "Electric", "Water", "Trash"
    public decimal UnitPrice { get; set; }
    public ServiceCalcMethod CalcMethod { get; set; }

    public Property? Property { get; set; }
    public Room? Room { get; set; }
}
