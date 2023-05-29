using BookStore.DataAccess.Repository.IRepository;
using BookStore.Models;
using BookStore.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineBookStoreApplication.Data;
using System.Security.Claims;
using System;
using System.Runtime;
using Newtonsoft.Json.Linq;
using BookStore.Utility;
using Stripe.Checkout;

namespace OnlineBookStoreApplication.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ShoppingCartVM shoppingCart { get; set; }
        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var Claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCart = new ShoppingCartVM()
            {
                ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == Claims.Value, IncludeProperties: "ProductType"),
                OrderHeader=new()
            };
            foreach(var cart in shoppingCart.ShoppingCartList)
            {
                 cart.Price=GetPriceForListCart(cart);
                shoppingCart.OrderHeader.OrderTotal += cart.Price* (cart.count);

            }
            return View(shoppingCart);
        }
        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var Claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCart = new ShoppingCartVM()
            {
                ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == Claims.Value, IncludeProperties: "ProductType"),
                OrderHeader = new()
            };
            foreach (var cart in shoppingCart.ShoppingCartList)
            {
                cart.Price = GetPriceForListCart(cart);
                shoppingCart.OrderHeader.OrderTotal += cart.Price * (cart.count);

            }
            shoppingCart.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == Claims.Value, null);
            shoppingCart.OrderHeader.PhoneNumber = shoppingCart.OrderHeader.ApplicationUser.PhoneNumber;
            shoppingCart.OrderHeader.StreetAddress = shoppingCart.OrderHeader.ApplicationUser.Address;
            shoppingCart.OrderHeader.City = shoppingCart.OrderHeader.ApplicationUser.City;
            shoppingCart.OrderHeader.PostalCode = shoppingCart.OrderHeader.ApplicationUser.PostalCode;
            shoppingCart.OrderHeader.State = shoppingCart.OrderHeader.ApplicationUser.State;
            return View(shoppingCart);
        }
        [HttpPost]
        [ActionName("Summary")]
        public IActionResult SummaryPost(ShoppingCartVM shoppingCartVM)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCartVM.ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claims, IncludeProperties: "ProductType");
            foreach (var cart in shoppingCartVM.ShoppingCartList)
            {
                cart.Price = GetPriceForListCart(cart);
                shoppingCartVM.OrderHeader.OrderTotal += cart.Price * cart.count;
            }
            ApplicationUser applicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == claims);
            shoppingCartVM.OrderHeader.ApplicationUser = applicationUser;
            shoppingCartVM.OrderHeader.OrderDate = System.DateTime.Now;
            if(applicationUser.CompanyId==0)
            {
                shoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;
                shoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
            }
            else
            {
                shoppingCartVM.OrderHeader.OrderStatus = SD.StatusApproved;
                shoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusDelayedPayment;
            }
            _unitOfWork.OrderHeader.Add(shoppingCartVM.OrderHeader);
            _unitOfWork.Save();
            foreach(var cart in shoppingCartVM.ShoppingCartList )
            {
                OrderDetail orderDetail = new()
                {
                    ProductTypeId=cart.ProductTypeId,
                    Price=cart.Price,
                    Count=cart.count,
                    OrderHeaderId= shoppingCartVM.OrderHeader.Id,
                };
                _unitOfWork.OrderDetail.Add(orderDetail);
                _unitOfWork.Save();
            }
            //stripe settings
            if (applicationUser.CompanyId == 0)
            {
                var domain = "https://localhost:44378/";
                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string>
                    {
                        "card",
                    },
                    LineItems = new List<SessionLineItemOptions>(),
                    Mode = "payment",
                    SuccessUrl = domain + $"Customer/Cart/OrderConfirmation?id={shoppingCartVM.OrderHeader.Id}",
                    CancelUrl = domain + $"Customer/Cart/Index",
                };
                foreach (var item in shoppingCartVM.ShoppingCartList)
                {
                    var sessionLineItem = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)item.Price * 100,//20.00=2000
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.ProductType.Title,
                            },

                        },
                        Quantity = item.count,
                    };
                    options.LineItems.Add(sessionLineItem);
                }

                var service = new SessionService();
                Session session = service.Create(options);

                _unitOfWork.OrderHeader.updateStripePaymentID(shoppingCartVM.OrderHeader.Id, session.Id, session.PaymentIntentId);
                _unitOfWork.Save();
                Response.Headers.Add("Location", session.Url);
                return new StatusCodeResult(303);


                //_unitOfWork.ShoppingCart.RemoveRange(ShoppingCartVM.ListCart);
                //_unitOfWork.Save();
                //return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("OrderConfirmation", "Cart", new { id = shoppingCartVM.OrderHeader.Id });
            }
        }
        public IActionResult OrderConfirmation(int id)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == id,IncludeProperties:"ApplicationUser");
            if(orderHeader.PaymentStatus!=SD.PaymentStatusDelayedPayment)
            {
                //payment done by the customer
                var service = new SessionService();
                var session = service.Get(orderHeader.SessionId);
                if(session.PaymentStatus.ToLower()=="paid")
                {
                    _unitOfWork.OrderHeader.updateStripePaymentID(id,session.Id,session.PaymentIntentId);
                    _unitOfWork.OrderHeader.updateStatus(id,SD.StatusApproved,SD.PaymentStatusApproved);
                    _unitOfWork.Save();
                }
            }
            _unitOfWork.ShoppingCart.RemoveRange(_unitOfWork.ShoppingCart.GetAll(u=>u.ApplicationUserId==orderHeader.ApplicationUserId));
            _unitOfWork.ShoppingCart.Save();
            return View(id);
            

        }
        public IActionResult Plus(int cartid)
        {
            var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.Id == cartid);
            _unitOfWork.ShoppingCart.Increment(cart, 1);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Minus(int cartid)
        {
            var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.Id == cartid);
            _unitOfWork.ShoppingCart.Decrement(cart, 1);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        public double GetPriceForListCart(NewShoppingCartList shoppingCart)
        {
            if(shoppingCart.count<=50)
            {
                return shoppingCart.ProductType.Price;
            }
            else
            {
                if(shoppingCart.count<=100)
                {
                    return shoppingCart.ProductType.ListPrice50;
                }
                else
                {
                    return shoppingCart.ProductType.ListPrice100;
                }
            }
        }
    }
}
