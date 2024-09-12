using System;
using CsvHelper;
using GloboTicket.TicketManagement.Application.Contracts.Infrastructure;
using GloboTicket.TicketManagement.Application.Features.Events.Queries.GetEventDetail;

namespace GloboTicket.TicketManagement.Infrastructure.FileExport
{

    public class CsvExporter : ICsvExporter
    {
        public byte[] ExportEventsToCsv(List<EventExportsDto> eventExportsDtos)
        {
            using var memoryStream = new MemoryStream();
            using (var streamWriter = new StreamWriter(memoryStream)){
                using var csvWriter = new CsvWriter(streamWriter);
            }

            return memoryStream.ToArray();
        }
    }
}
