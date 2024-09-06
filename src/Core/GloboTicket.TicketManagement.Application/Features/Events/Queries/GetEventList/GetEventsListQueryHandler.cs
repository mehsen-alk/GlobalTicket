using AutoMapper;
using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using MediatR;
using GloboTicket.TicketManagement.Domain.Entities;

namespace GloboTicket.TicketManagement.Application.Features.Events
{
    public class GetEventsListQueryHandler(IMapper mapper, IAsyncRepository<Event> eventRepository) : IRequestHandler<GetEventsListQuery, List<EventListVM>>
    {

        private readonly IAsyncRepository<Event> _eventRepository = eventRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<List<EventListVM>> Handle(GetEventsListQuery request, CancellationToken cancellationToken)
        {
            var allEvents = (await _eventRepository.ListAllAsync()).OrderBy(x => x.Date).ToList();

            return _mapper.Map<List<EventListVM>>(allEvents);
        }
    }
}

