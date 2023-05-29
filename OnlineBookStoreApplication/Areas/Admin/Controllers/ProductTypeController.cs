using BookStore.DataAccess.Repository;
using BookStore.DataAccess.Repository.IRepository;
using BookStore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BookStore.Models.ViewModels;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authorization;
using BookStore.Utility;

namespace OnlineBookStoreApplication.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.Role_Admin)]
    public class ProductTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductTypeController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        // GET: ProductTypeController
        public ActionResult Index()
        {
            IEnumerable<ProductType> ProductsFromDb = _unitOfWork.ProductType.
                GetAll(IncludeProperties:"Category");
            //return Json(new { data = ProductsFromDb });
            return View(ProductsFromDb);
        }

        // GET: ProductTypeController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProductTypeController/Create
        public ActionResult Upsert(int?id)
        {
            ProductViewModel productViewModel = new()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Product = new ProductType()

            };
            if (id == 0 || id == null)
            {
                return View(productViewModel);
            }
            else
            {
                productViewModel.Product = _unitOfWork.ProductType.GetFirstOrDefault(u => u.Id == id);
                return View(productViewModel);
            }
        }

        // POST: ProductTypeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upsert(ProductViewModel productVM,IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string WwwrootPath = _webHostEnvironment.WebRootPath;
                if(file!=null)
                {
                    string FileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(WwwrootPath, @"Images\Product");
                    if(!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        var oldpath=Path.Combine(WwwrootPath, productVM.Product.ImageUrl);
                        if(System.IO.File.Exists(oldpath))
                        {
                            System.IO.File.Delete(oldpath);
                        }
                    }
                    using (var fileStream=new FileStream(Path.Combine(productPath, FileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productVM.Product.ImageUrl = @"\Images\Product\" + FileName;
                }
                if(productVM.Product.Id==0)
                {
                    _unitOfWork.ProductType.Add(productVM.Product);
                    TempData["success"] = "Product Created Successfully";
                }
                else
                {
                    _unitOfWork.ProductType.Update(productVM.Product);
                    TempData["success"] = "Product Updated Successfully";
                }
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            else
            {
                productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                return View(productVM);
            }
        }


        // POST: ProductTypeController/Delete/5
        [HttpDelete]
        public ActionResult Delete(int ? id )
        {
            var obj = _unitOfWork.ProductType.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
            _unitOfWork.ProductType.Remove(obj);
            _unitOfWork.ProductType.Save();
            return Json(new { success = true, message = "Product deleted successfully" });
        }
        #region
        //API Calls
        [HttpGet]
        public IActionResult GetAll()
        {
            IEnumerable<ProductType> products = _unitOfWork.ProductType.GetAll(IncludeProperties:"Category");
            return Json(new { data=products });
        }
        #endregion
    }
}
