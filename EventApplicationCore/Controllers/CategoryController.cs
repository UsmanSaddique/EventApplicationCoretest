﻿using System;
using System.Collections.Generic;
 
using System.Linq;
using System.Threading.Tasks;
using EventApplicationCore.Concrete;
using EventApplicationCore.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



namespace EventApplicationCore.Controllers
{

    public class CategoryController : Controller
    {
        private readonly DatabaseContext _db;
        public CategoryController(DatabaseContext db)
        {
            _db = db;
        }
        //GET 
        public async Task<IActionResult> Index()
        {
            return View(await _db.Category.ToListAsync());
        }
        //Get for create
        public IActionResult Create()
        {
            return View();  
        }
        //post-create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _db.Category.Add(category);
                _ = await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        //Get-Edit
        public async Task<IActionResult> Edit(int? id) {

            if (id==null)
            {
                return NotFound();
            }
            var category = await _db.Category.FindAsync(id);

            if (category==null)
            {
                return NotFound();
            }
            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _db.Update(category);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            return View(category);
        }

        //Get-Delete
        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            var category = await _db.Category.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {

            var category = await _db.Category.FindAsync(id);
            if (category==null)
            {
                return View();
            }
            _db.Category.Remove(category);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


        }

        //Get-Details
        public async Task<IActionResult> Details(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            var category = await _db.Category.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
    }
}
