using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketExpress.Common;
using TicketExpress.Data;
using TicketExpress.Models;

namespace TicketExpress.Controllers;

[ApiController]
[Route("[controller]")]
public class BoletosController : ControllerBase
{
    private readonly TicketExpressContext _context;

    public BoletosController(TicketExpressContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Boleto>>> GetBoletos()
    {
        return await _context.Boletos.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Boleto>> GetBoleto(int id)
    {
        var boleto = await _context.Boletos.FindAsync(id);
        if (boleto == null)
            return NotFound();

        return Ok(boleto);
    }

    [HttpPost]
    public async Task<ActionResult<Boleto>> PostBoleto(Boleto boleto)
    {
        boleto.NombreComprador = TextNormalizer.Normalizar(boleto.NombreComprador);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var evento = await _context.Eventos.FindAsync(boleto.EventoId);
        if (evento == null)
            return BadRequest("El evento no existe.");

        if (evento.Fecha.Date < DateTime.Today)
            return BadRequest("No se puede comprar boletos para un evento cuya fecha ya pasó.");

        int disponibles = BoletosDisponibles.Restantes(_context, evento.Id);
        if (boleto.Cantidad > disponibles)
            return BadRequest($"No hay suficientes boletos disponibles. Solo quedan {disponibles}.");

        boleto.FechaCompra = DateTime.Now;
        _context.Boletos.Add(boleto);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetBoleto), new { id = boleto.Id }, boleto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutBoleto(int id, Boleto boleto)
    {
        if (id != boleto.Id)
            return BadRequest();

        var existente = await _context.Boletos.FindAsync(id);
        if (existente == null)
            return NotFound();

        boleto.NombreComprador = TextNormalizer.Normalizar(boleto.NombreComprador);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _context.Entry(existente).CurrentValues.SetValues(boleto);
        await _context.SaveChangesAsync();

        return NoContent();
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBoleto(int id)
    {
        var boleto = await _context.Boletos.FindAsync(id);
        if (boleto == null)
            return NotFound();

        _context.Boletos.Remove(boleto);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}