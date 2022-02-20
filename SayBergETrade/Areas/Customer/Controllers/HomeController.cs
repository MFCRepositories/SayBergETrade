using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SayBergETrade.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using SayBergETrade.Data;

namespace SayBergETrade.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
            
        }

        public IActionResult Index()
        {
            var products = _db.Products.Where(i=>i.IsHome).ToList();
            return View(products);
        }

        public IActionResult Details(int id)
        {
            var product = _db.Products.FirstOrDefault(x=>x.Id==id);
            ShoppingCart cart = new ShoppingCart()
            {
                Product = product,
                ProductId = product.Id,

            };
            return View(cart);
        } 
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart sCart )
        {
            sCart.Id = 0;
            if (ModelState.IsValid) //Cart boş ise yeni cart olustur
            {
                var claimIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
                sCart.ApplicationUserId = claim.Value;
                ShoppingCart cart = _db.ShoppingCarts.FirstOrDefault(u =>
                    u.ApplicationUserId == sCart.ApplicationUserId &&
                    u.ProductId == sCart.ProductId);
                if (cart==null)
                {
                    _db.ShoppingCarts.Add(sCart);
                }
                else
                {
                    cart.ProductCount += sCart.ProductCount;
                }

                _db.SaveChanges();
                var count = _db.ShoppingCarts.Where
                (i => i.ApplicationUserId == sCart.ApplicationUserId).ToList().Count;
                HttpContext.Session.SetInt32(Other.ssShoppingCart,count);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var product = _db.Products.FirstOrDefault(i => i.Id == sCart.Id);
                ShoppingCart cart = new ShoppingCart()
                {
                    Product = product,
                    ProductId = product.Id,

                };

            }
           
            return View(sCart);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
