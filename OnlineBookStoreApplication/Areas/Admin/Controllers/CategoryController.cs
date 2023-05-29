using Microsoft.AspNetCore.Mvc;
using OnlineBookStoreApplication.Data;
using BookStore.Models;
using NuGet.Protocol.Core.Types;
using BookStore.DataAccess.Repository;
using BookStore.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using BookStore.Utility;

namespace OnlineBookStoreApplication.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        public CategoryController(IUnitOfWork UnitOfWork)
        {
            _UnitOfWork = UnitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> catgoriesFromDb = _UnitOfWork.Category.GetAll();
            return View(catgoriesFromDb);
        }

        //Get Method
        public IActionResult Create()
        {
            return View();
        }
        //Post Method
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "the dispalyOrder cannot match the name");
            }
            if (ModelState.IsValid)
            {
                _UnitOfWork.Category.Add(obj);
                _UnitOfWork.Save();
                TempData["success"] = "Category Created Successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        //Get Method
        public IActionResult Edit(int? id)
        {
            if (id == 0 || id == null)
            {
                return NotFound();
            }
            var category = _UnitOfWork.Category.GetFirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        //Post Method
        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _UnitOfWork.Category.Update(obj);
                _UnitOfWork.Save();
                TempData["success"] = "Category updated Successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        //Get Method
        public IActionResult Delete(int? id)
        {
            if (id == 0 || id == null)
            {
                return NotFound();
            }
            var category = _UnitOfWork.Category.GetFirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        //Post Method
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            var category = _UnitOfWork.Category.GetFirstOrDefault(c => c.Id == id);
            _UnitOfWork.Category.Remove(category);
            _UnitOfWork.Save();
            TempData["success"] = "Category deleted Successfully";
            return RedirectToAction("Index");
        }
    }
}
