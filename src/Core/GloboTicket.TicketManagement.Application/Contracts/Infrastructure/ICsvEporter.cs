using System;
using GloboTicket.TicketManagement.Application.Features.Events.Queries.GetEventDetail;

namespace GloboTicket.TicketManagement.Application.Contracts.Infrastructure
{
    public interface ICsvExporter
    {
        byte[] ExportEventsToCsv(List<EventExportsDto> eventExportsDtos);
    }
}

