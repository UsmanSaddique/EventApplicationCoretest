using EventApplicationCore.Concrete;
using EventApplicationCore.Filters;
using EventApplicationCore.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventApplicationCore.Controllers
{
    [ValidateUserSession]
    public class BookingTourController :Controller
    {
        private readonly DatabaseContext _dbContext;
        public BookingTourController(DatabaseContext database)
        {
            _dbContext = database;
        }


        public IActionResult Index()
        {
            var tours = _dbContext.Tour.ToList();
            var TourVm = new List<TourVM>();

            foreach (var item in tours)
            {
                var tourvms = new TourVM();
                tourvms.Id = item.Id;
                tourvms.TourName = item.TourName;
                tourvms.PerHead = item.PerHead;
                tourvms.TourDate = item.TourDate;

                tourvms.TourFrom = _dbContext.City.Where(x => x.CityID == item.TourFrom).Select(x => x.CityName).FirstOrDefault();
                tourvms.TourTo = _dbContext.City.Where(x => x.CityID == item.TourTo).Select(x => x.CityName).FirstOrDefault();
                TourVm.Add(tourvms);
            }

            return View(TourVm);
        }

 
        public IActionResult Detail(int id)
        {
            var tours = _dbContext.Tour.Where(x=>x.Id==id).FirstOrDefault();


            ViewBag.From = new SelectList(_dbContext.City, "CityID", "CityName");
            ViewBag.To = new SelectList(_dbContext.City, "CityID", "CityName");
            var tourvms = new TourVM();
                tourvms.Id = tours.Id;
                tourvms.TourName = tours.TourName;
                tourvms.PerHead = tours.PerHead;
                tourvms.TourDate = tours.TourDate;
            tourvms.Discription = tours.Discription;
            tourvms.TransportType = tours.TransportType;
            tourvms.TourFrom = _dbContext.City.Where(x => x.CityID == tours.TourFrom).Select(x => x.CityName).FirstOrDefault();
                tourvms.TourTo = _dbContext.City.Where(x => x.CityID == tours.TourTo).Select(x => x.CityName).FirstOrDefault();
                
             
            return View(tourvms);
        }
      
        public IActionResult Book(TourVM tour)
        {
            var user = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
            TourBooking tourBooking = new TourBooking();
            tourBooking.CustomerId = user;
            tourBooking.NoOfPersons = tour.Seats;
            tourBooking.Total = tour.PerHead * tour.Seats;
            tourBooking.TourId = tour.Id;
            _dbContext.Add(tourBooking);
            _dbContext.SaveChanges();


            return RedirectToAction(nameof(ShowBooking));
        }

        public IActionResult ShowBooking()
        {
            var user = Convert.ToInt32(HttpContext.Session.GetString("UserID"));
            var Booking = _dbContext.TourBooking.Where(x => x.CustomerId==user).ToList();
          List< TourBookigngShowVM> tourBooking = new List<TourBookigngShowVM>();
            foreach (var item in Booking)
            {
                TourBookigngShowVM tourBookigngShowVM = new TourBookigngShowVM();
                tourBookigngShowVM.Id = item.Id;
                tourBookigngShowVM.CustomerName = _dbContext.Registration.Where(x => x.ID == item.CustomerId).Select(c => c.Name).FirstOrDefault();
                tourBookigngShowVM.NoOfPerson = item.NoOfPersons;
                tourBookigngShowVM.Total = item.Total;
                var tour = _dbContext.Tour.Where(x => x.Id == item.TourId) .FirstOrDefault();
                tourBookigngShowVM.TourName = tour.TourName;
                tourBookigngShowVM.From = _dbContext.City.Where(x=>x.CityID== tour.TourFrom).Select(c=>c.CityName).FirstOrDefault();
                tourBookigngShowVM.TO = _dbContext.City.Where(x => x.CityID == tour.TourTo).Select(c => c.CityName).FirstOrDefault();
                tourBooking.Add(tourBookigngShowVM);
            }


            return View(tourBooking);
        }

        public IActionResult DeleteBooking( int Id)
        {
            var Tour = _dbContext.TourBooking.Where(x => x.Id == Id).FirstOrDefault();
            _dbContext.TourBooking.Remove(Tour);
            _dbContext.SaveChanges();
            return View();
        }

    }
}
