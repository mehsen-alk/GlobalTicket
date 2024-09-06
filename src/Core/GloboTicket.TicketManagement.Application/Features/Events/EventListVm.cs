using System.Runtime.CompilerServices;
using GloboTicket.TicketManagement.Domain.Entities;

namespace GloboTicket.TicketManagement.Application.Features.Events
{
    public class EventListVM
    {
        public Guid EventId { get; set; }
        public required string Name { get; set; }
        public DateTime Date { get; set; }
        public required string ImageUrl { get; set; }
    }
}

