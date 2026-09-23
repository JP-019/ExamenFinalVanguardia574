using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using TicketExpress.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TicketExpressContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<TicketExpressContext>();
    context.Database.EnsureCreated();
}

app.Run();
