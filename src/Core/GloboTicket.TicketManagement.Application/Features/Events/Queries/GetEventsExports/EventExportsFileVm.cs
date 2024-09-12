using System;

namespace GloboTicket.TicketManagement.Application.Features.Events.Queries.GetEventsExports;

public class EventExportsFileVm
{
    public string EventExportFileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public byte[]? Data { get; set; }

}
