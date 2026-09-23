using Microsoft.EntityFrameworkCore;
using TicketExpress.Models;

namespace TicketExpress.Data;

public class TicketExpressContext : DbContext
{
    public TicketExpressContext(DbContextOptions<TicketExpressContext> options)
        : base(options)
    {
    }

    public DbSet<Evento> Eventos { get; set; }
    public DbSet<Boleto> Boletos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Evento>()
            .Property(e => e.PrecioBoleto)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Evento>()
            .HasMany(e => e.Boletos)
            .WithOne(b => b.Evento)
            .HasForeignKey(b => b.EventoId);
    }
}