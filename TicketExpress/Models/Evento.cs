namespace TicketExpress.Models;

public class Evento
{
    public int Id { get; set; }
    public string Nombre { get; set; } = "";
    public string Ciudad { get; set; } = "";
    public DateTime Fecha { get; set; }
    public int CapacidadTotal { get; set; }
    public decimal PrecioBoleto { get; set; }

    public List<Boleto> Boletos { get; set; } = new();
}