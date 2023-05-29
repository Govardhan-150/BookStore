using BookStore.DataAccess.Repository.IRepository;
using BookStore.Models;
using BookStore.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OnlineBookStoreApplication.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.Role_Admin)]
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        public CoverTypeController(IUnitOfWork UnitOfWork)
        {
            _UnitOfWork = UnitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<CoverType> coverTypesFromDb = _UnitOfWork.CoverType.GetAll();
            return View(coverTypesFromDb);
        }

        //Get Method
        public IActionResult Create()
        {
            return View();
        }
        //Post Method
        [HttpPost]
        public IActionResult Create(CoverType obj)
        {
            if (ModelState.IsValid)
            {
                _UnitOfWork.CoverType.Add(obj);
                _UnitOfWork.Save();
                TempData["success"] = "CoverType Created Successfully";
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
            var objFromdb = _UnitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);
            if (objFromdb == null)
            {
                return NotFound();
            }
            return View(objFromdb);
        }
        //Post Method
        [HttpPost]
        public IActionResult Edit(CoverType obj)
        {
            if (ModelState.IsValid)
            {
                _UnitOfWork.CoverType.Update(obj);
                _UnitOfWork.Save();
                TempData["success"] = "CoverType Updated Successfully";
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
            var objFromdb = _UnitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);
            if (objFromdb == null)
            {
                return NotFound();
            }
            return View(objFromdb);
        }
        //Post Method
        [HttpPost]
        public IActionResult Delete(CoverType obj)
        {
            if (ModelState.IsValid)
            {
                _UnitOfWork.CoverType.Remove(obj);
                _UnitOfWork.Save();
                TempData["success"] = "CoverType Deleted Successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }
    }
}
