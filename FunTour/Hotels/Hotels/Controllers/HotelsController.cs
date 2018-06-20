using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Hotels.Data;
using Hotels.Models;

namespace Hotels.Controllers
{
    public class HotelsController : Controller
    {
        private IHotelRepository hotelRepository;

        public HotelsController()
        {
            this.hotelRepository = new HotelRepository(new HotelsContext());
        }

        public HotelsController(IHotelRepository hotelRepository)
        {
            this.hotelRepository = hotelRepository;
        }

        // GET: Hotels
        public ActionResult Index()
        {
            return View(hotelRepository.GetHotels().ToList());
        }

        // GET: Hotels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Hotel hotel = hotelRepository.GetHotelByID(id);

            if (hotel == null)
            {
                return HttpNotFound();
            }

            return View(hotel);
        }

        // GET: Hotels/Create
        public ActionResult Create()
        {
            ViewBag.CountryID = new SelectList(hotelRepository.getCities(), "ZipCode", "CityName");
            ViewBag.ChainId = new SelectList(hotelRepository.getHotelChains(), "ChainID", "ChainName");
            ViewBag.RatingID = new SelectList(hotelRepository.getStarRatings(), "RatingID", "RatingID");
            return View();
        }

        // POST: Hotels/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "HotelID,ChainId,CountryID,RatingID,HotelName,HotelAddress,HotelEmail,HotelWebsite,HotelDetails,HotelCity")] Hotel hotel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    hotelRepository.InsertHotel(hotel);
                    hotelRepository.Save();
                    return RedirectToAction("Index");
                }
            }

            catch (DataException)
            {
                ModelState.AddModelError(string.Empty, "Unable to save changes. Try again, and if the problem persists contact your system administrator.");
            }
            ViewBag.CountryID = new SelectList(hotelRepository.getCities(), "ZipCode", "CityName", hotel.ZipCode);
            ViewBag.ChainId = new SelectList(hotelRepository.getHotelChains(), "ChainID", "ChainName", hotel.ChainId);
            ViewBag.RatingID = new SelectList(hotelRepository.getStarRatings(), "RatingID", "RatingID", hotel.RatingID);
            return View(hotel);
        }

        // GET: Hotels/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hotel hotel = hotelRepository.GetHotelByID(id);
            if (hotel == null)
            {
                return HttpNotFound();
            }
            ViewBag.CountryID = new SelectList(hotelRepository.getCities(), "ZipCode", "CityName", hotel.ZipCode);
            ViewBag.ChainId = new SelectList(hotelRepository.getHotelChains(), "ChainID", "ChainName", hotel.ChainId);
            ViewBag.RatingID = new SelectList(hotelRepository.getStarRatings(), "RatingID", "RatingID", hotel.RatingID);
            return View(hotel);
        }

        // POST: Hotels/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "HotelID,ChainId,CountryID,RatingID,HotelName,HotelAddress,HotelEmail,HotelWebsite,HotelDetails,HotelCity")] Hotel hotel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    hotelRepository.UpdateHotel(hotel);
                    hotelRepository.Save();
                    return RedirectToAction("Index");
                }
            }

            catch (DataException)
            {
                ModelState.AddModelError(string.Empty, "Unable to save changes. Try again, and if the problem persists contact your system administrator.");
            }

            ViewBag.CountryID = new SelectList(hotelRepository.getCities(), "ZipCode", "CityName", hotel.ZipCode);
            ViewBag.ChainId = new SelectList(hotelRepository.getHotelChains(), "ChainID", "ChainName", hotel.ChainId);
            ViewBag.RatingID = new SelectList(hotelRepository.getStarRatings(), "RatingID", "RatingID", hotel.RatingID);
            return View(hotel);
        }

        // GET: Hotels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Hotel hotel = hotelRepository.GetHotelByID(id);

            if (hotel == null)
            {
                return HttpNotFound();
            }
            return View(hotel);
        }

        // POST: Hotels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Hotel hotel = hotelRepository.GetHotelByID(id);
                hotelRepository.DeleteHotel(id);
                hotelRepository.Save();
            }

            catch (DataException)
            {
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            hotelRepository.Dispose();
            base.Dispose(disposing);
        }

    }
}
