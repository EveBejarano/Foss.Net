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
    [Route("api/HotelsApi")]
    public class HotelsApiController : Controller
    {
        private readonly HotelContext _context;

        public HotelsApiController(HotelContext context)
        {
            _context = context;
        }

        // GET: api/HotelsApi
        [HttpGet]
        public IEnumerable<Hotel> GetHotel()
        {
            return _context.Hotel;
        }

        // GET: api/HotelsApi/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetHotel([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var hotel = await _context.Hotel.SingleOrDefaultAsync(m => m.IDHotel == id);

            if (hotel == null)
            {
                return NotFound();
            }

            return Ok(hotel);
        }


        // PUT: api/HotelsApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHotel([FromRoute] int id, [FromBody] Hotel hotel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != hotel.IDHotel)
            {
                return BadRequest();
            }

            _context.Entry(hotel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HotelExists(id))
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

        // POST: api/HotelsApi
        [HttpPost]
        public async Task<IActionResult> PostHotel([FromBody] Hotel hotel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Hotel.Add(hotel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHotel", new { id = hotel.IDHotel }, hotel);
        }

        // DELETE: api/HotelsApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHotel([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var hotel = await _context.Hotel.SingleOrDefaultAsync(m => m.IDHotel == id);
            if (hotel == null)
            {
                return NotFound();
            }

            _context.Hotel.Remove(hotel);
            await _context.SaveChangesAsync();

            return Ok(hotel);
        }

        private bool HotelExists(int id)
        {
            return _context.Hotel.Any(e => e.IDHotel == id);
        }
    }
}