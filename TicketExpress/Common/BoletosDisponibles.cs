using TicketExpress.Data;

namespace TicketExpress.Common;

public static class BoletosDisponibles
{
    public static int Restantes(TicketExpressContext context, int eventoId)
    {
        var evento = context.Eventos.Find(eventoId);
        if (evento == null)
            return 0;

        int vendidos = context.Boletos
            .Where(b => b.EventoId == eventoId)
            .Sum(b => (int?)b.Cantidad) ?? 0;

        return evento.CapacidadTotal - vendidos;
    }
}