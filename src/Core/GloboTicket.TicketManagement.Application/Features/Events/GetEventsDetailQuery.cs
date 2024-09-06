using MediatR;

namespace GloboTicket.TicketManagement.Application.Features.Events
{
    public class GetEventsDetailQuery : IRequest<EventDetailVM>
    {
        public Guid Id { get; set; }
    }
}

