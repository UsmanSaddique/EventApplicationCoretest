using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using EventApplicationCore.Concrete;
using EventApplicationCore.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace EventApplicationCore.Controllers
{

    public class MenuItemController : Controller
    {

        private readonly DatabaseContext _db;
        private readonly IHostingEnvironment _hostEnvironment;

      
        public MenuItemViewModel MenuItemVM { get; set; }
        public MenuItemController(DatabaseContext db, IHostingEnvironment hostEnvironment)
        {
            _db = db;
            _hostEnvironment = hostEnvironment;
            MenuItemVM = new MenuItemViewModel()
            {
                Category = _db.Category,
                SubCategory = _db.SubCategory,
                MenuItem = new  MenuItem()
            };
        }
        public async Task<IActionResult> Index()
        {
            var menuItem = await _db.MenuItem.Include(m=>m.Category).Include(m=>m.SubCategory).ToListAsync();

            return View(menuItem);
        }
        //get create 
        public IActionResult Create()
        {

            return View(MenuItemVM);
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public  IActionResult CreatePOST(MenuItemViewModel menuItemViewModel)
        {
           // MenuItemVM.MenuItem.SubCategoryId = Convert.ToInt32(Request.Form["SubCategoryId"].ToString());

            if (!ModelState.IsValid)
            {
                return View(menuItemViewModel);
            }
            _db.MenuItem.Add(menuItemViewModel.MenuItem);
              _db.SaveChanges();

            //work on the image saving section
            string webRootPath = _hostEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;

            var menuItemFromDb =  _db.MenuItem.Find(menuItemViewModel.MenuItem.Id);

            if (files.Count > 0)
            {
                //files has ben uploaded
                var uploads = Path.Combine(webRootPath, "img1");
                var extention = Path.GetExtension(files[0].FileName);

                using (var fileStream = new FileStream(Path.Combine(uploads, menuItemViewModel.MenuItem.Id + extention), FileMode.Create))
                {

                    files[0].CopyTo(fileStream);
                }
                menuItemFromDb.Image = @"\img1\" + menuItemViewModel.MenuItem.Id + extention;
            }
            else
            {
                //no file has been uploaded,so use default 

                var uploads = Path.Combine(webRootPath, @"Images\"  );
                System.IO.File.Copy(uploads, webRootPath + @"\Images\" + menuItemViewModel.MenuItem.Id + ".jpg");
                menuItemFromDb.Image = @"\Images\" + menuItemViewModel.MenuItem.Id + ".jpg";

            }
             _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        //get edit 
        public async Task<IActionResult> Edit(int? id)
        {
            if (id==null)
            {
                return NotFound();
            }
            MenuItemVM.MenuItem = await _db.MenuItem.Include(m => m.Category).Include(m => m.SubCategory).SingleOrDefaultAsync(m => m.Id == id);
            MenuItemVM.SubCategory = await _db.SubCategory.Where(s => s.CategoryId == MenuItemVM.MenuItem.CategoryId).ToListAsync();

            if (MenuItemVM.MenuItem==null)
            {
                return NotFound();
            }
            return View(MenuItemVM);
        }

       

        [HttpPost,ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPOST(MenuItemViewModel MenuItemVM)
        {
          //  MenuItemVM.MenuItem.SubCategoryId = Convert.ToInt32(Request.Form["SubCategoryId"].ToString());

            if (!ModelState.IsValid)
            {
                MenuItemVM.SubCategory = await _db.SubCategory.Where(s => s.CategoryId == MenuItemVM.MenuItem.CategoryId).ToListAsync();
                return View(MenuItemVM);
            }


            //work on the image saving section
            string webRootPath = _hostEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;

            var menuItemFromDb = await _db.MenuItem.FindAsync(MenuItemVM.MenuItem.Id);

            if (files.Count > 0)
            {
                // new imges files has ben uploaded
                var uploads = Path.Combine(webRootPath, "img1");
                var extention_new = Path.GetExtension(files[0].FileName);

                if (menuItemFromDb.Image!=null)
                {
                    var imagePath = Path.Combine(webRootPath, menuItemFromDb.Image.TrimStart('\\'));

                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }

                }
 
                using (var fileStream = new FileStream(Path.Combine(uploads, MenuItemVM.MenuItem.Id + extention_new), FileMode.Create))
                {

                    files[0].CopyTo(fileStream);
                }
                menuItemFromDb.Image = @"\img1\" + MenuItemVM.MenuItem.Id + extention_new;
            }
            menuItemFromDb.Name = MenuItemVM.MenuItem.Name;
            menuItemFromDb.Discription = MenuItemVM.MenuItem.Discription;
            menuItemFromDb.Price = MenuItemVM.MenuItem.Price;
            menuItemFromDb.Spicyness = MenuItemVM.MenuItem.Spicyness;
            menuItemFromDb.CategoryId = MenuItemVM.MenuItem.CategoryId;
            menuItemFromDb.SubCategoryId = MenuItemVM.MenuItem.SubCategoryId;

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            MenuItemVM.MenuItem = await _db.MenuItem.Include(m => m.Category).Include(m => m.SubCategory).SingleOrDefaultAsync(m => m.Id == id);
            MenuItemVM.SubCategory = await _db.SubCategory.Where(s => s.CategoryId == MenuItemVM.MenuItem.CategoryId).ToListAsync();

            if (MenuItemVM.MenuItem == null)
            {
                return NotFound();
            }
            return View(MenuItemVM);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var MenuItemfromDb = await _db.MenuItem.FindAsync(id);

            if (MenuItemfromDb == null)
            {
                return NotFound();
            }
            _db.MenuItem.Remove(MenuItemfromDb);
            await _db.SaveChangesAsync();

            var menuItem = await _db.MenuItem.Include(m => m.Category).Include(m => m.SubCategory).ToListAsync();

            return View(nameof(Index),"MenuItem");
        }

    }
}
