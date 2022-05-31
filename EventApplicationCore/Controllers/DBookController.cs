using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EventApplicationCore.Concrete;
using EventApplicationCore.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace EventApplicationCore.Controllers
{
  
    public class DBookController : Controller
    {

        private readonly DatabaseContext _db;

        public DBookController(DatabaseContext db)
        {
            _db = db;
        }
     

        public async Task<IActionResult> Index()
        {
            IndexViewModel IndexVM = new IndexViewModel
            {
                MenuItem = await _db.MenuItem.Include(m => m.Category).Include(m => m.SubCategory).ToListAsync(),
                Category = await _db.Category.ToListAsync(),
                Coupon = await _db.Coupon.Where(c => c.isActive == true).ToListAsync()
            };

             
            if (HttpContext.Session.GetString("UserID") != null)
            {
                var cnt = _db.ShoppingCart.Where(u => u.ApplicationUserId == HttpContext.Session.GetString("UserID")).ToList().Count;
                HttpContext.Session.SetInt32("ssShoppingCarCount", cnt); 
            }

            return View(IndexVM);
        }

        public async Task<IActionResult> Search(string data)
        {
            IndexViewModel IndexVM = new IndexViewModel
            {
                MenuItem = await _db.MenuItem.Include(m => m.Category).Where(n => n.Category.Name==data).Include(m => m.SubCategory).ToListAsync(),
                Category = await _db.Category.Where(u=>u.Name == data).ToListAsync(),
                Coupon = await _db.Coupon.Where(c => c.isActive == true).ToListAsync()
            };

    
            

            return View(IndexVM);
        }

        public  async Task<IActionResult> Details(int id)
        {
            var MenuItemFromDb = await _db.MenuItem.Include(m => m.Category).Include(m => m.SubCategory).Where(m => m.Id == id).FirstOrDefaultAsync();
            ShoppingCart cartobj = new ShoppingCart()
            {
                MenuItem = MenuItemFromDb,
                MenuItemId = MenuItemFromDb.Id,
                ApplicationUserId= HttpContext.Session.GetString("UserID")
            };
            return View(cartobj);

        }
         [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(ShoppingCart CartObject)
        {

            CartObject.Id = 0;
            if (ModelState.IsValid)
            {


                //  CartObject.ApplicationUserId = claim.Value;
                var id = HttpContext.Session.GetString("UserID");
                CartObject.ApplicationUserId = HttpContext.Session.GetString("UserID");
                ShoppingCart cartFromDb = await _db.ShoppingCart.
                    Where(c => c.ApplicationUserId == CartObject.ApplicationUserId && c.MenuItemId == CartObject.MenuItemId).FirstOrDefaultAsync();
                if (cartFromDb==null)
                {
                   
                    await _db.ShoppingCart.AddAsync(CartObject);
                }
                else
                {
                    cartFromDb.Count = cartFromDb.Count + CartObject.Count;

                }
                await _db.SaveChangesAsync();

                var count = _db.ShoppingCart.Where(c => c.ApplicationUserId == CartObject.ApplicationUserId).ToList().Count();
                HttpContext.Session.SetInt32("ssShoppingCarCount", count);
                return RedirectToAction("Index");


            }
            else
            {
                var MenuItemFromDb = await _db.MenuItem.Include(m => m.Category).Include(m => m.SubCategory).
                    Where(m => m.Id ==CartObject.MenuItemId ).FirstOrDefaultAsync();
                ShoppingCart cartobj = new ShoppingCart()
                {
                    MenuItem = MenuItemFromDb,
                    MenuItemId = MenuItemFromDb.Id
                };
                return RedirectToAction("Index");
            }
        }


            public IActionResult Privacy()
        {
            return View();
        }

         
    }
}
