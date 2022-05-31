using EventApplicationCore.Concrete;
using EventApplicationCore.Filters;
using EventApplicationCore.Interface;
using EventApplicationCore.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EventApplicationCore.Controllers
{
    [ValidateAdminSession]
    public class TourController : Controller
    {

        private DatabaseContext _dbContext;
        public TourController(DatabaseContext context) {
            _dbContext = context;
        }

       public IActionResult Index() {
            var tours = _dbContext.Tour.ToList();
            var TourVm = new List<TourVM>();

            foreach (var item in tours)
            {
                var tourvms = new TourVM();
                tourvms.Id = item.Id;
                tourvms.TourName = item.TourName;
                tourvms.Cost = item.Cost;
                tourvms.TourDate = item.TourDate;
                
                tourvms.TourFrom = _dbContext.City.Where(x => x.CityID == item.TourFrom).Select(x => x.CityName).FirstOrDefault();
                tourvms.TourTo = _dbContext.City.Where(x => x.CityID == item.TourTo).Select(x => x.CityName).FirstOrDefault();
                TourVm.Add(tourvms);
            }

            return View(TourVm);
        }

        public IActionResult Create(int ?id)
        {
            ViewBag.From = new SelectList(_dbContext.City, "CityID", "CityName");
            ViewBag.To = new SelectList(_dbContext.City, "CityID", "CityName");
            Tour tour = new Tour();
            if (id==null)
            {
                return View(tour);
            }
            else if (id!=0)
            {
                tour = _dbContext.Tour.Where(x => x.Id == id).FirstOrDefault();
                return View(tour);
            }
             
            return View();
        }
        [HttpPost]
        public IActionResult Create(Tour tour)
        {
           
            if (tour.Id == 0)
            {
                _dbContext.Tour.Add(tour);
                _dbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            else if (tour.Id != 0)
            {
               var model = _dbContext.Tour.Where(x => x.Id == tour.Id).FirstOrDefault();
                model.TourName= tour.TourName  ;
                model.TourFrom= tour.TourFrom  ;
                model.TourTo= tour.TourTo  ;
                model.TransportType = tour.TransportType;
                model.TourDate = tour.TourDate;
                model.EndDate = tour.EndDate;
                model.Cost = tour.Cost;
                model.PerHead = tour.PerHead;
                model.CreatedDate = DateTime.Now;
                _dbContext.Tour.Update(model);
                _dbContext.SaveChanges();
                ViewBag.From = new SelectList(_dbContext.City, "CityID", "CityName");
                ViewBag.To = new SelectList(_dbContext.City, "CityID", "CityName");
                return RedirectToAction(nameof(Index));
            }

            return View();
        }
    }
}
