using EventApplicationCore.Concrete;
using EventApplicationCore.Filters;
using EventApplicationCore.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventApplicationCore.Controllers
{
    [ValidateUserSession]
    public class BookHallController : Controller
    {
        private readonly DatabaseContext _db;
        public BookHallController(DatabaseContext database)
        {
            _db = database;
        }
        public IActionResult Index() {
            BookingHall bookingHall = new BookingHall();
            return View(bookingHall);

        }
        public IActionResult ShowAll()
        {
            int userid = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
            var bookingHall = _db.BookingHall.Where(x => x.Createdby == userid).ToList();
            var Bookhall = new List<BookHallVM>();
            foreach (var item in bookingHall)
            {
                var hallvm = new BookHallVM();
                hallvm.BookingHallID = item.BookingHallID;
                hallvm.Createdby = _db.Registration.Where(x => x.ID == item.Createdby).Select(x => x.Name).FirstOrDefault();
                hallvm.BookingDate = item.BookingDate;
                hallvm.VenueName = _db.Venue.Where(x => x.VenueID == item.VenueID).Select(c => c.VenueName).FirstOrDefault();
                hallvm.GuestCount = item.GuestCount;
                hallvm.EventType = _db.Event.Where(x => x.EventID == item.EventTypeID).Select(x => x.EventType).FirstOrDefault();
                Bookhall.Add(hallvm);
            }
            return View(Bookhall);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult BookingVenue(BookingHall BookingVenue)
        {
            if (ModelState.IsValid)
            {

                

                //  var result = _IBookingVenue.BookEvent(BD);
                var result = 1;
                BookingHall BV = new BookingHall
                {
                    VenueID = BookingVenue.VenueID,
                    EventTypeID = BookingVenue.EventTypeID,
                    GuestCount = BookingVenue.GuestCount,
                    Createdby = Convert.ToInt32(HttpContext.Session.GetString("UserID")),
                    CreatedDate = DateTime.Now,
                   
                };

                //var VenueID = _IBookingVenue.BookVenue(BV);

                HttpContext.Session.SetInt32("BookingID", result);
                _db.BookingHall.Add(BV);
                _db.SaveChanges();
                if (result > 0)
                {
                     
                    ModelState.Clear();
                    ViewData["BookingMessage"] = "Venue Booked Successfully";
                    return RedirectToAction(nameof(ShowAll));
                }
                else
                {
                   
                    return RedirectToAction(nameof(ShowAll));
                }
            }
            else
            {
             
                return RedirectToAction(nameof(ShowAll));
            }
        }
       
       
        public IActionResult ShowBooking()
        {
            var user = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
            var Booking = _db.BookingHall.Where(x => x.Createdby == user).ToList();
            List<BookingHall> tourBooking = new List<BookingHall>();
            foreach (var item in Booking)
            {
                BookingHall tourBookigngShowVM = new BookingHall();
           
                tourBooking.Add(tourBookigngShowVM);
            }


            return View(tourBooking);
        }
    }
}
