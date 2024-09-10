using GloboTicket.TicketManagement.Api;

var builder = WebApplication.CreateBuilder(args);

var app = builder
    .ConfigureServices()
    .ConfigurePipeline();

// for developing and testing purpose
await app.ResetDatabaseAsync();

app.Run();
