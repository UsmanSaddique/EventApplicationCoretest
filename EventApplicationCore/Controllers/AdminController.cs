using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EventApplicationCore.Filters;


namespace EventApplicationCore.Controllers
{
    [ValidateAdminSession]
    public class AdminController : Controller
    {
        // GET: /<controller>/
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
