namespace SmartStay.Application.DTOs.Room;

public class UpdateRoomRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Type { get; set; }
    public decimal BasePrice { get; set; }
    public double? AreaM2 { get; set; }
    public int? MaxOccupants { get; set; }
}
