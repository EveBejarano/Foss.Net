using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hoteles.Data;
using Hoteles.Models;

namespace Hoteles.Controllers
{
    [Produces("application/json")]
    [Route("api/Habitacions")]
    public class HabitacionsController : Controller
    {
        private readonly HotelContext _context;

        public HabitacionsController(HotelContext context)
        {
            _context = context;
        }

        // GET: api/Habitacions
        [HttpGet]
        public IEnumerable<Habitacion> GetHabitacion()
        {
            return _context.Habitacion;
        }

        // GET: api/Habitacions/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetHabitacion([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var habitacion = await _context.Habitacion.SingleOrDefaultAsync(m => m.IDHabitacion == id);

            if (habitacion == null)
            {
                return NotFound();
            }

            return Ok(habitacion);
        }

        // PUT: api/Habitacions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHabitacion([FromRoute] int id, [FromBody] Habitacion habitacion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != habitacion.IDHabitacion)
            {
                return BadRequest();
            }

            _context.Entry(habitacion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HabitacionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Habitacions
        [HttpPost]
        public async Task<IActionResult> PostHabitacion([FromBody] Habitacion habitacion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Habitacion.Add(habitacion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHabitacion", new { id = habitacion.IDHabitacion }, habitacion);
        }

        // DELETE: api/Habitacions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHabitacion([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var habitacion = await _context.Habitacion.SingleOrDefaultAsync(m => m.IDHabitacion == id);
            if (habitacion == null)
            {
                return NotFound();
            }

            _context.Habitacion.Remove(habitacion);
            await _context.SaveChangesAsync();

            return Ok(habitacion);
        }

        private bool HabitacionExists(int id)
        {
            return _context.Habitacion.Any(e => e.IDHabitacion == id);
        }
    }
}