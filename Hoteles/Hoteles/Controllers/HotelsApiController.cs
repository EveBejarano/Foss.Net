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
        private readonly HotelContext db;

        public HotelsApiController(HotelContext context)
        {
            db = context;
        }

        // GET: api/HotelsApi
        [HttpGet]
        public IQueryable<HotelDTO> GetHotel()
        {
            var hotels = from h in db.Hotel

                         select new HotelDTO()
                         {
                             IDHotel = h.IDHotel,
                             nombre_hotel = h.nombre_hotel,
                             direccion_hotel = h.direccion_hotel,
                             nombre_pais = h.Pais.nombre_pais,
                             Imagen_estrella = h.Estrellas.Imagen_estrella,
                             ciudad_hotel = h.ciudad_hotel
                         };

            return hotels;

        }

        // GET: api/HotelsApi/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetHotel(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var hotel = await db.Hotel.Select(h => new HotelDetailsDTO()
            {
                IDHotel = h.IDHotel,
                nombre_hotel = h.nombre_hotel,
                direccion_hotel = h.direccion_hotel,
                email_hotel = h.email_hotel,
                sitioweb_hotel = h.sitioweb_hotel,
                ciudad_hotel = h.ciudad_hotel,
                detalles_hotel = h.detalles_hotel,
                nombre_cadena = h.Cadena_Hotel.nombre_cadena,
                nombre_pais = h.Pais.nombre_pais,
                Imagen_estrella = h.Estrellas.Imagen_estrella

            }).SingleOrDefaultAsync(h => h.IDHotel == id);

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

            db.Entry(hotel).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
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

            db.Hotel.Add(hotel);
            await db.SaveChangesAsync();

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

            var hotel = await db.Hotel.SingleOrDefaultAsync(m => m.IDHotel == id);
            if (hotel == null)
            {
                return NotFound();
            }

            db.Hotel.Remove(hotel);
            await db.SaveChangesAsync();

            return Ok(hotel);
        }

        private bool HotelExists(int id)
        {
            return db.Hotel.Any(e => e.IDHotel == id);
        }
    }
}
