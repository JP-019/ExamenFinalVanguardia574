using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketExpress.Common;
using TicketExpress.Data;
using TicketExpress.Models;

namespace TicketExpress.Controllers;

[ApiController]
[Route("[controller]")]
public class EventosController : ControllerBase
{
    private readonly TicketExpressContext _context;

    public EventosController(TicketExpressContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Evento>>> GetEventos()
    {
        return await _context.Eventos.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Evento>> GetEvento(int id)
    {
        var evento = await _context.Eventos.FindAsync(id);
        if (evento == null)
            return NotFound();

        return Ok(evento);
    }

    [HttpPost]
    public async Task<ActionResult<Evento>> PostEvento(Evento evento)
    {
        evento.Nombre = TextNormalizer.Normalizar(evento.Nombre);
        evento.Ciudad = TextNormalizer.Normalizar(evento.Ciudad);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (evento.Fecha.Date < DateTime.Today)
            return BadRequest(new { Fecha = new[] { "La fecha no puede ser una fecha pasada." } });

        _context.Eventos.Add(evento);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetEvento), new { id = evento.Id }, evento);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutEvento(int id, Evento evento)
    {
        if (id != evento.Id)
            return BadRequest();

        var existente = await _context.Eventos.FindAsync(id);
        if (existente == null)
            return NotFound();

        evento.Nombre = TextNormalizer.Normalizar(evento.Nombre);
        evento.Ciudad = TextNormalizer.Normalizar(evento.Ciudad);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (evento.Fecha.Date < DateTime.Today)
            return BadRequest(new { Fecha = new[] { "La fecha no puede ser una fecha pasada." } });

        _context.Entry(existente).CurrentValues.SetValues(evento);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEvento(int id)
    {
        var evento = await _context.Eventos.FindAsync(id);
        if (evento == null)
            return NotFound();

        bool tieneBoletos = await _context.Boletos.AnyAsync(b => b.EventoId == id);
        if (tieneBoletos)
            return BadRequest("No se puede eliminar un evento que ya tiene boletos vendidos.");

        _context.Eventos.Remove(evento);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}