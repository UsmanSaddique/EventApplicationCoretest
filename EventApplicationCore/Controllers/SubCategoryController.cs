using System;
using System.Collections.Generic;
 
using System.Linq;
using System.Threading.Tasks;
using EventApplicationCore.Concrete;
using EventApplicationCore.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;



namespace EventApplicationCore.Controllers
{

    public class SubCategoryController : Controller
    {
        private readonly DatabaseContext _db;
        [TempData]
        public string StatusMassage { get; set; }
        public SubCategoryController(DatabaseContext db)
        {
            _db = db;
        }
        //GET 
        public async Task<IActionResult> Index()
        {

            var SubCategories = await _db.SubCategory.Include(s => s.Category).ToListAsync();
            return View(SubCategories);
        }

        //get create
        public async Task<IActionResult> Create()
        {

            SubCategoryAndCategoryViewModel model = new SubCategoryAndCategoryViewModel()
            {
                CategoryList = await _db.Category.ToListAsync(),

                SubCategory = new SubCategory(),
                SubCategoryList = await _db.SubCategory.OrderBy(n => n.Name).Select(n => n.Name).Distinct().ToListAsync()

            };
            return View(model);
        }
        //post create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SubCategoryAndCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var doesCategoryExits = _db.SubCategory.Include(p => p.Category).Where(p => p.Name == model.SubCategory.Name && p.Category.Id == model.SubCategory.CategoryId);

                if (doesCategoryExits.Count() > 0)
                {
                    //Error
                    StatusMassage = "Error : Sub Category Exist under " + doesCategoryExits.First().Category.Name + " category. please use another name";

                }
                else
                {
                    _db.SubCategory.Add(model.SubCategory);
                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

            }
            SubCategoryAndCategoryViewModel modelVM = new SubCategoryAndCategoryViewModel()
            {
                CategoryList = await _db.Category.ToListAsync(),
                SubCategory = model.SubCategory,
                SubCategoryList = await _db.SubCategory.OrderBy(n => n.Name).Select(n => n.Name).Distinct().ToListAsync(),
                StatusMessage = StatusMassage
            };
            return View(modelVM);
        }


        [ActionName("GetSubCategory")]
        public async Task<IActionResult> GetSubCategory(int id)
        {
            List<SubCategory> subCategories = new List<SubCategory>();

            subCategories = await (from subCategory in _db.SubCategory
                                   where subCategory.CategoryId == id
                                   select subCategory).ToListAsync();
            return Json(new SelectList(subCategories, "Id", "Name"));
        }



        //get Edit
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
            {

                return NotFound();
            }

            var subCategory = await _db.SubCategory.SingleOrDefaultAsync(m => m.Id == id);
            if (subCategory == null)
            {
                return NotFound();
            }
            SubCategoryAndCategoryViewModel model = new SubCategoryAndCategoryViewModel()
            {
                CategoryList = await _db.Category.ToListAsync(),
                SubCategory = subCategory,
                SubCategoryList = await _db.SubCategory.OrderBy(n => n.Name).Select(n => n.Name).Distinct().ToListAsync()

            };
            return View(model);
        }
        //post edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SubCategoryAndCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var doesCategoryExits = _db.SubCategory.Include(p => p.Category).Where(p => p.Name == model.SubCategory.Name && p.Category.Id == model.SubCategory.CategoryId);

                if (doesCategoryExits.Count() > 0)
                {
                    //Error
                    StatusMassage = "Error : Sub Category Exist under " + doesCategoryExits.First().Category.Name + " category. please use another name";

                }
                else
                {

                    var subCatFromDb = await _db.SubCategory.FindAsync(model.SubCategory.Id);
                    subCatFromDb.Name = model.SubCategory.Name;

                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

            }
            SubCategoryAndCategoryViewModel modelVM = new SubCategoryAndCategoryViewModel()
            {
                CategoryList = await _db.Category.ToListAsync(),
                SubCategory = model.SubCategory,
                SubCategoryList = await _db.SubCategory.OrderBy(n => n.Name).Select(n => n.Name).Distinct().ToListAsync(),
                StatusMessage = StatusMassage
            };
            return View(modelVM);
        }

        [HttpGet]

        public async Task<IActionResult> Delete(int Id)
        {
            var subcatfordel = await _db.SubCategory.FirstOrDefaultAsync(u => u.Id == Id);
            if (subcatfordel == null)
            {
                return NotFound();
            }
            _db.SubCategory.Remove(subcatfordel);
            await _db.SaveChangesAsync();
            return View();

        }
    }
}
