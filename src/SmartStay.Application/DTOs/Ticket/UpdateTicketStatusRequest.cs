using SmartStay.Domain.Enums;

namespace SmartStay.Application.DTOs.Ticket;

public class UpdateTicketStatusRequest
{
    public TicketStatus Status { get; set; }
}
