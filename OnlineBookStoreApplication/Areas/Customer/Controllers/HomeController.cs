using BookStore.DataAccess.Repository.IRepository;
using BookStore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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
            ProductType Product = _UnitOfWork.ProductType.GetFirstOrDefault(u=>u.Id==id,IncludeProperties: "Category");
            return View(Product);
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