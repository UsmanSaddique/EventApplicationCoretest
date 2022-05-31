using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EventApplicationCore.Interface;
using Microsoft.AspNetCore.Http;
using EventApplicationCore.Filters;
using EventApplicationCore.Model;

 

namespace EventApplicationCore.Controllers
{
    [ValidateUserSession]
    public class UserProfileController : Controller
    {
        IRegistration _IRepository;
        public UserProfileController(IRegistration IRepository)
        {
            _IRepository = IRepository;
        }

        [HttpGet]
        public IActionResult Profile()
        {
            try
            {
                var profile = _IRepository.Userinformation(Convert.ToInt32(HttpContext.Session.GetString("UserID")));
                return View(profile);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public IActionResult Update(RegistrationViewModel userinformation)
        {
            try
            {
                _IRepository.UpdateUserProfile(userinformation);
               // var profile = _IRepository.Userinformation(Convert.ToInt32(HttpContext.Session.GetString("UserID")));
                return RedirectToAction(nameof(Profile));
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
