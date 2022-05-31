using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

 

namespace EventApplicationCore.Controllers
{
    public class ErrorController : Controller
    {
        // GET: /<controller>/
        public IActionResult Error()
        {
         
            return View("Error");
        }
    }
}
