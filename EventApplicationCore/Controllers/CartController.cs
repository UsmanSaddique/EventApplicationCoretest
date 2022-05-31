using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EventApplicationCore.Concrete;
using EventApplicationCore.Model;
using EventApplicationCore.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
 

namespace EventApplicationCore.Controllers
{ 
    public class CartController : Controller
    {
        private readonly DatabaseContext _db; 
         
        public OrderDetailsCart detailCart { get; set; }

        public CartController(DatabaseContext db)
        {
            _db = db;
          
        }

        public async Task<IActionResult> Index()
        {

            detailCart = new OrderDetailsCart()
            {
                OrderHeader = new  OrderHeader()
            };

            detailCart.OrderHeader.OrderTotal = 0;

             

            var cart = _db.ShoppingCart.Where(c => c.ApplicationUserId ==HttpContext.Session.GetString("UserID"));
            if (cart != null)
            {
                detailCart.listCart = cart.ToList();
            }

            foreach (var list in detailCart.listCart)
            {
                list.MenuItem = await _db.MenuItem.FirstOrDefaultAsync(m => m.Id == list.MenuItemId);
                detailCart.OrderHeader.OrderTotal = detailCart.OrderHeader.OrderTotal + (list.MenuItem.Price * list.Count);
                list.MenuItem.Discription = SD.ConvertToRawHtml(list.MenuItem.Discription);
                if (list.MenuItem.Discription.Length > 100)
                {
                    list.MenuItem.Discription = list.MenuItem.Discription.Substring(0, 99) + "...";
                }
            }
            detailCart.OrderHeader.OrderTotalOriginal = detailCart.OrderHeader.OrderTotal;

            if (HttpContext.Session.GetString(SD.ssCouponCode)!=null)
            {
                detailCart.OrderHeader.CouponCode = HttpContext.Session.GetString(SD.ssCouponCode);
                var couponfromdb = await _db.Coupon.Where(c => c.Name.ToLower() == detailCart.OrderHeader.CouponCode.ToLower()).FirstOrDefaultAsync();
                detailCart.OrderHeader.OrderTotal = SD.DiscountedPrice(couponfromdb, detailCart.OrderHeader.OrderTotalOriginal);
            }


            return View(detailCart);

        }


        public async Task<IActionResult> Summary()
        {

            detailCart = new OrderDetailsCart()
            {
                OrderHeader = new OrderHeader()
            };

            detailCart.OrderHeader.OrderTotal = 0;

            
         //   ApplicationUser applicationUser = await _db.ApplicationUser.Where(c => c.Id == claim.Value).FirstOrDefaultAsync();
            var cart = _db.ShoppingCart.Where(c => c.ApplicationUserId == HttpContext.Session.GetString("UserID"));
            if (cart != null)
            {
                detailCart.listCart = cart.ToList();
            }

            foreach (var list in detailCart.listCart)
            {
                list.MenuItem = await _db.MenuItem.FirstOrDefaultAsync(m => m.Id == list.MenuItemId);
                detailCart.OrderHeader.OrderTotal = detailCart.OrderHeader.OrderTotal + (list.MenuItem.Price * list.Count);

            }
            detailCart.OrderHeader.OrderTotalOriginal = detailCart.OrderHeader.OrderTotal;
            //detailCart.OrderHeader.PickupName = applicationUser.Name;
            //detailCart.OrderHeader.PhoneNumber = applicationUser.PhoneNumber;
            detailCart.OrderHeader.PickUpTime = DateTime.Now;


            if (HttpContext.Session.GetString(SD.ssCouponCode) != null)
            {
                detailCart.OrderHeader.CouponCode = HttpContext.Session.GetString(SD.ssCouponCode);
                var couponFromDb = await _db.Coupon.Where(c => c.Name.ToLower() == detailCart.OrderHeader.CouponCode.ToLower()).FirstOrDefaultAsync();
                detailCart.OrderHeader.OrderTotal = SD.DiscountedPrice(couponFromDb, detailCart.OrderHeader.OrderTotalOriginal);
            }


            return View(detailCart);

        }

         [HttpPost]
        [ValidateAntiForgeryToken]  
        [ActionName("Summary")]
        public async Task<IActionResult> SummaryPost()
        {
            detailCart = new OrderDetailsCart() { 
            listCart=new List<ShoppingCart>(),
            OrderHeader =new OrderHeader(),
            
        };

            var id = HttpContext.Session.GetString("UserID");
            var listcart = _db.ShoppingCart.Where(x=>x.ApplicationUserId==id).ToList();
            detailCart.listCart = listcart;

            detailCart.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
            detailCart.OrderHeader.OrderDate = DateTime.Now;
            detailCart.OrderHeader.UserId = HttpContext.Session.GetString("UserID");
            detailCart.OrderHeader.Status = SD.PaymentStatusPending;
            detailCart.OrderHeader.PickUpTime = Convert.ToDateTime(detailCart.OrderHeader.PickUpDate.ToShortDateString() + " " + detailCart.OrderHeader.PickUpTime.ToShortTimeString());

            List<OrderDetails> orderDetailsList = new List<OrderDetails>();
            _db.OrderHeader.Add(detailCart.OrderHeader);
            await _db.SaveChangesAsync();

            detailCart.OrderHeader.OrderTotalOriginal = 0;


            foreach (var item in detailCart.listCart)
            {
                item.MenuItem = await _db.MenuItem.FirstOrDefaultAsync(m => m.Id == item.MenuItemId);
                OrderDetails orderDetails = new OrderDetails
                {
                    MenuItemId = item.MenuItemId,
                    OrderId = detailCart.OrderHeader.Id,
                    Description = item.MenuItem.Discription,
                    Name = item.MenuItem.Name,
                    Price = item.MenuItem.Price,
                    Count = item.Count
                };
                detailCart.OrderHeader.OrderTotalOriginal += orderDetails.Count * orderDetails.Price;
                _db.OrderDetails.Add(orderDetails);

            }

            if (HttpContext.Session.GetString(SD.ssCouponCode) != null)
            {
                detailCart.OrderHeader.CouponCode = HttpContext.Session.GetString(SD.ssCouponCode);
                var couponFromDb = await _db.Coupon.Where(c => c.Name.ToLower() == detailCart.OrderHeader.CouponCode.ToLower()).FirstOrDefaultAsync();
                detailCart.OrderHeader.OrderTotal = SD.DiscountedPrice(couponFromDb, detailCart.OrderHeader.OrderTotalOriginal);
            }
            else
            {
                detailCart.OrderHeader.OrderTotal = detailCart.OrderHeader.OrderTotalOriginal;
            }
            detailCart.OrderHeader.CouponCodeDiscount = detailCart.OrderHeader.OrderTotalOriginal - detailCart.OrderHeader.OrderTotal;

            _db.ShoppingCart.RemoveRange(detailCart.listCart);

            HttpContext.Session.SetInt32(SD.ssShoppingCarCount, 0);
            detailCart.OrderHeader.Status = SD.StatusSubmitted;
            detailCart.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
            await _db.SaveChangesAsync();
            //  return RedirectToAction("Index", "Home");
            return RedirectToAction("Confirm", "Order", new { id = detailCart.OrderHeader.Id });
        }


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[ActionName("Summary")]
        //public async Task<IActionResult> SummaryPost(string stripeToken)
        //{
        //    var claimsIdentity = (ClaimsIdentity)User.Identity;
        //    var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);


        //    detailCart.listCart = await _db.ShoppingCart.Where(c => c.ApplicationUserId == claim.Value).ToListAsync();

        //    detailCart.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
        //    detailCart.OrderHeader.OrderDate = DateTime.Now;
        //    detailCart.OrderHeader.UserId = claim.Value;
        //    detailCart.OrderHeader.Status = SD.PaymentStatusPending;
        //    detailCart.OrderHeader.PickUpTime = Convert.ToDateTime(detailCart.OrderHeader.PickUpDate.ToShortDateString() + " " + detailCart.OrderHeader.PickUpTime.ToShortTimeString());

        //    List<OrderDetails> orderDetailsList = new List<OrderDetails>();
        //    _db.OrderHeader.Add(detailCart.OrderHeader);
        //    await _db.SaveChangesAsync();

        //    detailCart.OrderHeader.OrderTotalOriginal = 0;


        //    foreach (var item in detailCart.listCart)
        //    {
        //        item.MenuItem = await _db.MenuItem.FirstOrDefaultAsync(m => m.Id == item.MenuItemId);
        //        OrderDetails orderDetails = new OrderDetails
        //        {
        //            MenuItemId = item.MenuItemId,
        //            OrderId = detailCart.OrderHeader.Id,
        //            Description = item.MenuItem.Discription,
        //            Name = item.MenuItem.Name,
        //            Price = item.MenuItem.Price,
        //            Count = item.Count,
        //            ItemSize=item.ItemSize
        //        };
        //        detailCart.OrderHeader.OrderTotalOriginal += orderDetails.Count * orderDetails.Price;
        //        _db.OrderDetails.Add(orderDetails);

        //    }

        //    if (HttpContext.Session.GetString(SD.ssCouponCode) != null)
        //    {
        //        detailCart.OrderHeader.CouponCode = HttpContext.Session.GetString(SD.ssCouponCode);
        //        var couponFromDb = await _db.Coupon.Where(c => c.Name.ToLower() == detailCart.OrderHeader.CouponCode.ToLower()).FirstOrDefaultAsync();
        //        detailCart.OrderHeader.OrderTotal = SD.DiscountedPrice(couponFromDb, detailCart.OrderHeader.OrderTotalOriginal);
        //    }
        //    else
        //    {
        //        detailCart.OrderHeader.OrderTotal = detailCart.OrderHeader.OrderTotalOriginal;
        //    }
        //    detailCart.OrderHeader.CouponCodeDiscount = detailCart.OrderHeader.OrderTotalOriginal - detailCart.OrderHeader.OrderTotal;

        //    _db.ShoppingCart.RemoveRange(detailCart.listCart);

        //    HttpContext.Session.SetInt32(SD.ssShoppingCarCount, 0);

        //    //Stripe code for transactions

        //    var options = new ChargeCreateOptions
        //    {
        //        Amount = Convert.ToInt32(detailCart.OrderHeader.OrderTotal * 100),
        //        Currency = "usd",
        //        Description = "Order ID : " + detailCart.OrderHeader.Id,
        //        Source = stripeToken

        //    };
        //    var service = new ChargeService();
        //    Charge charge = service.Create(options);

        //    if (charge.BalanceTransactionId == null)
        //    {
        //        detailCart.OrderHeader.PaymentStatus = SD.PaymentStatusRejected;
        //    }
        //    else
        //    {
        //        detailCart.OrderHeader.TransactionId = charge.BalanceTransactionId;
        //    }

        //    if (charge.Status.ToLower() == "succeeded")
        //    {
        //        detailCart.OrderHeader.PaymentStatus = SD.PaymentStatusApproved;
        //        detailCart.OrderHeader.Status = SD.StatusSubmitted;
        //    }
        //    else
        //    {
        //        detailCart.OrderHeader.PaymentStatus = SD.PaymentStatusRejected;
        //    }

        //    await _db.SaveChangesAsync();
        //    //  return RedirectToAction("Index", "Home");
        //    return RedirectToAction("Confirm", "Order", new { id = detailCart.OrderHeader.Id });
        //}


        public IActionResult AddCoupon()
        {
            if (detailCart.OrderHeader.CouponCode==null)
            {
                detailCart.OrderHeader.CouponCode = "";
            }
            HttpContext.Session.SetString(SD.ssCouponCode, detailCart.OrderHeader.CouponCode);
            return RedirectToAction(nameof(Index));

        }
        public IActionResult RemoveCoupon()
        {
           
            HttpContext.Session.SetString(SD.ssCouponCode, string.Empty);
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> plus(int cartId)
        {
            var cart = await _db.ShoppingCart.FirstOrDefaultAsync(c => c.Id == cartId);
            cart.Count += 1;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Minus(int cartId)
        {
            var cart = await _db.ShoppingCart.FirstOrDefaultAsync(c => c.Id == cartId);
            if (cart.Count == 1)
            {
                _db.ShoppingCart.Remove(cart);
                await _db.SaveChangesAsync();

                var cnt = _db.ShoppingCart.Where(u => u.ApplicationUserId == cart.ApplicationUserId).ToList().Count;
                HttpContext.Session.SetInt32(SD.ssShoppingCarCount, cnt);
            }
            else
            {
                cart.Count -= 1;
                await _db.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Remove(int cartId)
        {
            var cart = await _db.ShoppingCart.FirstOrDefaultAsync(c => c.Id == cartId);

            _db.ShoppingCart.Remove(cart);
            await _db.SaveChangesAsync();

            var cnt = _db.ShoppingCart.Where(u => u.ApplicationUserId == cart.ApplicationUserId).ToList().Count;
            HttpContext.Session.SetInt32(SD.ssShoppingCarCount, cnt);


            return RedirectToAction(nameof(Index));
        }

       
    }
}