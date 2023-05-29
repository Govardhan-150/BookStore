using BookStore.DataAccess.Repository;
using BookStore.DataAccess.Repository.IRepository;
using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace OnlineBookStoreApplication.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _UnitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _UnitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<ProductType> ProductsList = _UnitOfWork.ProductType.GetAll(IncludeProperties: "Category");
            return View(ProductsList);
        }
        public IActionResult Details(int id)
        {
            NewShoppingCartList cart = new NewShoppingCartList
            {
                ProductType = _UnitOfWork.ProductType.GetFirstOrDefault(u => u.Id == id, IncludeProperties: "Category"),
                count = 1,
                ProductTypeId = id
            };
            return View(cart);
        }
        [HttpPost]
        [Authorize]
        public IActionResult Details(NewShoppingCartList shoppingCart)
        {
            var claimsIdentity=(ClaimsIdentity)User.Identity;
            var UserId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCart.ApplicationUserId = UserId;
            var CartFromDb = _UnitOfWork.ShoppingCart.GetFirstOrDefault(u => u.ApplicationUser.Id == UserId &&
            u.ProductTypeId == shoppingCart.ProductTypeId);
            if(CartFromDb!=null)
            {
                //card is not empty
                CartFromDb.count += shoppingCart.count;
                _UnitOfWork.ShoppingCart.Update(shoppingCart);
            }
            else
            {
                //Cart is empty
                _UnitOfWork.ShoppingCart.Add(shoppingCart);
            }

            _UnitOfWork.Save();

            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}