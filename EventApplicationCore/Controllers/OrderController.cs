using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using EventApplicationCore.Concrete;
using EventApplicationCore.Model;
using EventApplicationCore.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
 

namespace EventApplicationCore.Controllers
{ 
    public class OrderController : Controller
    {

        private readonly DatabaseContext _db;


        public OrderController(DatabaseContext db)
        {
            _db = db;
        }
         
        public async Task<IActionResult> Confirm(int id)
        {
            try
            {
                var idsa = HttpContext.Session.GetString("UserID");
                var tt = _db.OrderHeader.FirstOrDefault(o => o.Id == id && o.UserId ==idsa);
                var asd = _db.OrderDetails.Where(o => o.OrderId == id).ToList();
                OrderDetailsViewModel orderDetailsViewModel = new OrderDetailsViewModel()
                {
                    OrderHeader = await _db.OrderHeader.FirstOrDefaultAsync(o => o.Id == id && o.UserId == HttpContext.Session.GetString("UserID")),
                    OrderDetails = await _db.OrderDetails.Where(o => o.OrderId == id).ToListAsync()
                };
                return View(orderDetailsViewModel);
            }
            catch (Exception ex)
            {

                throw;
            }

           

            
        }
         
        public async Task<IActionResult> OrderHistory(int productPage=1)
        {
             

            OrderListViewModel orderListViewModel = new OrderListViewModel()
            {
                Orders = new List<OrderDetailsViewModel>()
            };



                 List<OrderHeader> OrderHeaderList = await _db.OrderHeader.Where(u => u.UserId == HttpContext.Session.GetString("UserID")).ToListAsync();

            foreach (OrderHeader item in OrderHeaderList)
            {
                OrderDetailsViewModel individual = new OrderDetailsViewModel
                {
                    OrderHeader = item,
                    OrderDetails = await _db.OrderDetails.Where(o => o.OrderId == item.Id).ToListAsync()
                };
                orderListViewModel.Orders.Add(individual);
            }

            var count = orderListViewModel.Orders.Count;
            orderListViewModel.Orders = orderListViewModel.Orders.
                OrderByDescending(p => p.OrderHeader.Id).ToList();
            
            return View(orderListViewModel);
        }

         
        public async Task<IActionResult> ManageOrder()
        {
            List<OrderDetailsViewModel> orderlistVM = new List<OrderDetailsViewModel>();


            List<OrderHeader> OrderHeaderList = await _db.OrderHeader
                .Where(u => u.Status==SD.StatusSubmitted || u.Status == SD.StatusInProcess).OrderByDescending(u=>u.PickUpTime).ToListAsync();

            foreach (OrderHeader item in OrderHeaderList)
            {
                OrderDetailsViewModel individual = new OrderDetailsViewModel
                {
                    OrderHeader = item,
                    OrderDetails = await _db.OrderDetails.Where(o => o.OrderId == item.Id).ToListAsync()
                };
                orderlistVM.Add(individual);
            }

            return View(orderlistVM.OrderBy(o=>o.OrderHeader.PickUpTime).ToList());
        }




        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [ActionName("Details")]
        public async Task<IActionResult> GetOrderDetails(int Id) {
           
            OrderDetailsViewModel orderDetailsViewModel = new OrderDetailsViewModel()
            {
                OrderHeader = await _db.OrderHeader.FirstOrDefaultAsync(m => m.Id == Id),
                OrderDetails = await _db.OrderDetails.Where(m => m.OrderId == Id).ToListAsync()

        };
            //orderDetailsViewModel.OrderHeader.UserId = await _db.ApplicationUser.FirstOrDefaultAsync(u => u.Id == orderDetailsViewModel.OrderHeader.UserId);
            return PartialView("_IndividualOrderDetails",orderDetailsViewModel);
        }


        public   IActionResult OrderStatus(int Id)
        {


            return PartialView("_OrderStatusPartialView", _db.OrderHeader.Where(m => m.Id == Id).FirstOrDefault().Status);
        }

         
        public async Task<IActionResult> OrderPrepare(int OrderId) {

            OrderHeader orderHeader = await _db.OrderHeader.FindAsync(OrderId);
            orderHeader.Status = SD.StatusInProcess;
            await _db.SaveChangesAsync();
            return RedirectToAction("ManageOrder", "Order");
        }

         
        public async Task<IActionResult> OrderReady(int OrderId)
        {

            OrderHeader orderHeader = await _db.OrderHeader.FindAsync(OrderId);
            orderHeader.Status = SD.StatusReady;
            await _db.SaveChangesAsync();

            //Email to notify the user about order is ready for pickup
            return RedirectToAction("ManageOrder", "Order");
        
        }
         
        public async Task<IActionResult> OrderCancel(int OrderId)
        {

            OrderHeader orderHeader = await _db.OrderHeader.FindAsync(OrderId);
            orderHeader.Status = SD.StatusCancelled;
            await _db.SaveChangesAsync();

            //Email to notify the user about order is ready for pickup
            return RedirectToAction("ManageOrder", "Order");

        }


        public async Task<IActionResult> OrderPickup()
        {
            //var claimsIdentity = (ClaimsIdentity)User.Identity;
            //var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            OrderListViewModel orderListViewModel = new OrderListViewModel()
            {
                Orders = new List<OrderDetailsViewModel>()
            };



            List<OrderHeader> OrderHeaderList = await _db.OrderHeader.Where(u => u.Status==SD.StatusReady).ToListAsync();

            foreach (OrderHeader item in OrderHeaderList)
            {
                OrderDetailsViewModel individual = new OrderDetailsViewModel
                {
                    OrderHeader = item,
                    OrderDetails = await _db.OrderDetails.Where(o => o.OrderId == item.Id).ToListAsync()
                };
                orderListViewModel.Orders.Add(individual);
            }

            orderListViewModel.Orders = orderListViewModel.Orders.
                OrderByDescending(p => p.OrderHeader.Id).ToList();

            return View(orderListViewModel);
        }

         
        [HttpPost]
        [ActionName("OrderPickup")]
        public async Task<IActionResult> OrderPickupPost(int orderId) {


            OrderHeader orderHeader = await _db.OrderHeader.FindAsync(orderId);
            orderHeader.Status = SD.StatusCompleted;
            orderHeader.PaymentStatus = SD.PaymentStatusApproved;
            await _db.SaveChangesAsync();

            //Email to notify the user about order is ready for pickup
            return RedirectToAction("OrderPickup", "Order");
        }

    }
}
