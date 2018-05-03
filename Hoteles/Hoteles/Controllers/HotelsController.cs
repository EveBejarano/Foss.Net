using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hoteles.Data;
using Hoteles.Models;

namespace Hoteles.Controllers
{
    public class HotelsController : Controller 
    {
        private readonly HotelContext _context;

        public HotelsController(HotelContext context)
        {
            _context = context;
        }

        // GET: Hotels
            public async Task<IActionResult> Index(string sortOrder, string searchString)
            {
                ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewData["CiudadSortParm"] = sortOrder == "ciudad";
                ViewData["CurrentFilter"] = searchString;

                var hotels = from s in _context.Hotel
                               select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                hotels = hotels.Where(s => s.ciudad_hotel.Contains(searchString));
            }

                switch (sortOrder)
                {
                    case "name_desc":
                        hotels = hotels.OrderByDescending(s => s.nombre_hotel);
                        break;
                    case "ciudad":
                        hotels = hotels.OrderBy(s => s.ciudad_hotel);
                        break;
                    default:
                        hotels = hotels.OrderBy(s => s.nombre_hotel);
                        break;
                }
                return View(await hotels.AsNoTracking().ToListAsync());
            }

        // GET: Hotels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hotel = await _context.Hotel
                .Include(h => h.Cadena_Hotel)
                .Include(h => h.Estrellas)
                .Include(h => h.Pais)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.IDHotel == id);
            if (hotel == null)
            {
                return NotFound();
            }

            return View(hotel);
        }

        // GET: Hotels/Create
        public IActionResult Create()
        {
            ViewData["IDCadena"] = new SelectList(_context.Cadena_Hotel, "IDCadena", "IDCadena");
            ViewData["IDEstrellas"] = new SelectList(_context.Estrellas, "IDEstrellas", "IDEstrellas");
            ViewData["IDPais"] = new SelectList(_context.Pais, "IDPais", "IDPais");
            return View();
        }

        // POST: Hotels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("IDHotel,IDCadena,IDPais,IDEstrellas,nombre_hotel,direccion_hotel,email_hotel,sitioweb_hotel,detalles_hotel,ciudad_hotel")] Hotel hotel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(hotel);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException /* ex */)
            {
                // Log the error (uncomment ex variable name and write a log)
                ModelState.AddModelError("", "Unable to save changes. " + 
                    "Try again, and if the problem persists " + 
                    "see your system administrator.");
            }
            ViewData["IDCadena"] = new SelectList(_context.Cadena_Hotel, "IDCadena", "IDCadena", hotel.IDCadena);
            ViewData["IDEstrellas"] = new SelectList(_context.Estrellas, "IDEstrellas", "IDEstrellas", hotel.IDEstrellas);
            ViewData["IDPais"] = new SelectList(_context.Pais, "IDPais", "IDPais", hotel.IDPais);
            return View(hotel);
        }

        // GET: Hotels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hotel = await _context.Hotel.SingleOrDefaultAsync(m => m.IDHotel == id);
            if (hotel == null)
            {
                return NotFound();
            }
            ViewData["IDCadena"] = new SelectList(_context.Cadena_Hotel, "IDCadena", "IDCadena", hotel.IDCadena);
            ViewData["IDEstrellas"] = new SelectList(_context.Estrellas, "IDEstrellas", "IDEstrellas", hotel.IDEstrellas);
            ViewData["IDPais"] = new SelectList(_context.Pais, "IDPais", "IDPais", hotel.IDPais);
            return View(hotel);
        }

        // POST: Hotels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IDHotel,IDCadena,IDPais,IDEstrellas,nombre_hotel,direccion_hotel,email_hotel,sitioweb_hotel,detalles_hotel,ciudad_hotel")] Hotel hotel)
        {
            if (id != hotel.IDHotel)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hotel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HotelExists(hotel.IDHotel))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IDCadena"] = new SelectList(_context.Cadena_Hotel, "IDCadena", "IDCadena", hotel.IDCadena);
            ViewData["IDEstrellas"] = new SelectList(_context.Estrellas, "IDEstrellas", "IDEstrellas", hotel.IDEstrellas);
            ViewData["IDPais"] = new SelectList(_context.Pais, "IDPais", "IDPais", hotel.IDPais);
            return View(hotel);
        }

        // GET: Hotels/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hotel = await _context.Hotel
                .AsNoTracking()
                .Include(h => h.Cadena_Hotel)
                .Include(h => h.Estrellas)
                .Include(h => h.Pais)
                .SingleOrDefaultAsync(m => m.IDHotel == id);
            if (hotel == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }

            return View(hotel);
        }

        // POST: Hotels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hotel = await _context.Hotel
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.IDHotel == id);
            if (hotel == null)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Hotel.Remove(hotel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        private bool HotelExists(int id)
        {
            return _context.Hotel.Any(e => e.IDHotel == id);
        }
    }
}
