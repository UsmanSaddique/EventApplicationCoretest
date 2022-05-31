using EventApplicationCore.Filters;
using EventApplicationCore.Interface;
using EventApplicationCore.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace EventApplicationCore.Controllers
{
    [ValidateUserSession]
    public class BookEquipmentController : Controller
    {
        private IEquipment _IEquipment;
        private IBookEquipment _IBookEquipment;
        private IBookFlower _IBookFlower;
        private IFlower _IFlower;
        IBookFood _IBookFood;
        IFood _IFood;
        IDishtypes _IDishtypes;
        IBookingLight _IBookingLight;
        ILight _ILight;
        IBookingVenue _IBookingVenue;
        public BookEquipmentController(IEquipment IEquipment, IBookEquipment IBookEquipment,
            IBookFlower IBookFlower, IFlower IFlower,
            IBookFood IBookFood, IDishtypes IDishtypes, IFood IFood,
            IBookingLight IBookingLight, ILight ILight,IBookingVenue IBookingVenue)
        {
            _IEquipment = IEquipment;
            _IBookEquipment = IBookEquipment;
            _IFlower = IFlower;
            _IBookFlower = IBookFlower;
            _IBookFood = IBookFood;
            _IFood = IFood;
            _IDishtypes = IDishtypes;
            _IBookingLight = IBookingLight;
            _ILight = ILight;
            _IBookingVenue = IBookingVenue;
        }


        [HttpGet]
        public IActionResult Equipment()
        {
            try
            {
                BookingVM objeq = new BookingVM();
                objeq.bookingFood = new BookingFood();
                objeq.bookingLight = new BookingLight();
                objeq.bookingFlower = new BookingFlower();
                objeq.bookingEquipment = new BookingEquipment();
                objeq.bookingEquipment.EquipmentList = _IEquipment.GetAllEquipment();

                objeq.bookingFlower.FlowerList = _IFlower.GetAllFlower();

                objeq.bookingFood.FoodList = _IFood.GetAllFood();
                objeq.bookingFood.FoodType = "1";
                objeq. bookingFood.MealType = "1";

                objeq. bookingLight.LightList = _ILight.GetAllLight();
                objeq.bookingLight.LightType = "1";

                SetSlider();
                return View(objeq);
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Equipment(BookingVM bookingVM)
        {
            try
            {
                var result = 0;
                var validateChecked = 0;

                if (false)
                {
                    return View("Equipment", bookingVM);
                }
                 
                for (int i = 0; i < bookingVM.bookingEquipment.EquipmentList.Count(); i++)
                {
                    if (bookingVM.bookingEquipment.EquipmentList[i].EquipmentChecked == true)
                    {
                        validateChecked = 1;

                        BookingEquipment bk = new BookingEquipment()
                        {
                            EquipmentID = Convert.ToInt32(bookingVM.bookingEquipment.EquipmentList[i].EquipmentID),
                            BookingID = Convert.ToInt32(HttpContext.Session.GetInt32("BookingID")),
                            Createdby = Convert.ToInt32(HttpContext.Session.GetString("UserID")),
                            CreatedDate = DateTime.Now,
                            BookingEquipmentID = 0,

                        };
                        result = _IBookEquipment.BookEquipment(bk);
                    }
                }
                 

                for (int i = 0; i < bookingVM.bookingFood .FoodList.Count(); i++)
                {
                    if (bookingVM.bookingFood.FoodList[i].FoodChecked)
                    {
                        validateChecked = 1;

                        BookingFood objFood = new BookingFood()
                        {
                            FoodType = bookingVM.bookingFood.FoodType,
                            MealType = bookingVM.bookingFood.MealType,
                            DishType = bookingVM.bookingFood.DishType,
                            DishName = Convert.ToInt32(bookingVM.bookingFood.FoodList[i].FoodID),
                            BookingID = Convert.ToInt32(HttpContext.Session.GetInt32("BookingID")),
                            Createdby = Convert.ToInt32(HttpContext.Session.GetString("UserID")),
                            CreatedDate = DateTime.Now
                        };
                        result = _IBookFood.BookFood(objFood);
                    }
                }
              
                if (bookingVM.bookingLight != null && bookingVM.bookingLight.LightList != null)
                {
                     

                    for (int i = 0; i < bookingVM.bookingLight.LightList.Count(); i++)
                    {
                        if (bookingVM.bookingLight.LightList[i].LightChecked)
                        {
                            validateChecked = 1;


                            BookingLight objbookinglight = new BookingLight()
                            {
                                LightType = bookingVM.bookingLight.LightType,
                                LightIDSelected = Convert.ToInt32(bookingVM.bookingLight.LightList[i].LightID),
                                BookingID = Convert.ToInt32(HttpContext.Session.GetInt32("BookingID")),
                                Createdby = Convert.ToInt32(HttpContext.Session.GetString("UserID")),
                                CreatedDate = DateTime.Now
                            };
                            result = _IBookingLight.BookingLight(objbookinglight);

                        }
                    }

                    if (validateChecked == 0)
                    {
                        ModelState.AddModelError("", "You have not choose any Lighting !");
                    }

                    SetSlider();

                    
                }
                for (int i = 0; i < bookingVM.bookingFlower.FlowerList.Count(); i++)
                {
                    if (bookingVM.bookingFlower.FlowerList[i].FlowerChecked)
                    {
                        validateChecked = 1;

                        BookingFlower objbookingflower = new BookingFlower()
                        {
                            FlowerID = Convert.ToInt32(bookingVM.bookingFlower.FlowerList[i].FlowerID),
                            BookingID = Convert.ToInt32(HttpContext.Session.GetInt32("BookingID")),
                            Createdby = Convert.ToInt32(HttpContext.Session.GetString("UserID")),
                            CreatedDate = DateTime.Now
                        };

                        result = _IBookFlower.BookFlower(objbookingflower);
                        if (result > 0)
                        {
                            _IBookingVenue.UpdateBookingStatus(Convert.ToInt32(HttpContext.Session.GetInt32("BookingID")));
                        }
                    }
                }
                if (validateChecked == 0)
                {
                    ModelState.AddModelError("", "You have not choose Option !");
                }

                SetSlider();

                if (result > 0)
                {
                    ViewData["BookingEquipmentMessage"] = "  Booking Successfully";
                    return View("Success");
                }
                else
                {
                    return View("Equipment", bookingVM);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        [NonAction]
        private void SetSlider()
        {
            try
            {
                var Images = _IEquipment.ShowAllEquipment();
                ViewBag.SliderEquipmentImages = Images;
                var Imagess = _IFood.ShowAllFood();
                ViewBag.SliderFoodImages = Imagess;
                var Images1 = _IFlower.ShowAllFlower();
                ViewBag.SliderFlowerImages = Images1;
                var Images2 = _ILight.ShowAllLight();
                ViewBag.SliderLightImages = Images2;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
