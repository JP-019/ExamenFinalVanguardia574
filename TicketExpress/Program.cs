using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using TicketExpress.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TicketExpressContext>(options =>
    options.UseInMemoryDatabase("TicketExpressDB"));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
