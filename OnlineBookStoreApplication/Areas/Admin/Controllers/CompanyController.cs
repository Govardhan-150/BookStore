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
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        // GET: CompanyController
        public ActionResult Index()
        {
            IEnumerable<Company> CompaniesFromDb = _unitOfWork.Company.GetAll();
            //return Json(new { data = ProductsFromDb });
            return View(CompaniesFromDb);
        }


        // GET: CompanyController/Create
        public ActionResult Upsert(int? id)
        {
            Company comp = new Company();
            if (id == 0 || id == null)
            {
                return View(comp);
            }
            else
            {
                Company company = _unitOfWork.Company.GetFirstOrDefault(u => u.Id == id);
                return View(company);
            }
        }

        // POST: CompanyController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upsert(Company company)
        {
            if (ModelState.IsValid)
            {
                if(company.Id==0)
                {
                    _unitOfWork.Company.Add(company);
                    TempData["success"] = "Company Created Successfully";
                }
                else
                {
                    _unitOfWork.Company.Update(company);
                    TempData["success"] = "Company Updated Successfully";
                }
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(company);
        }


        // POST: ProductTypeController/Delete/5
        [HttpDelete]
        public ActionResult Delete(int ? id )
        {
            var obj = _unitOfWork.Company.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _unitOfWork.Company.Remove(obj);
            _unitOfWork.Company.Save();
            return Json(new { success = true, message = "Company deleted successfully" });
        }
        #region
        //API Calls
        [HttpGet]
        public IActionResult GetAll()
        {
            IEnumerable<Company> Companies = _unitOfWork.Company.GetAll();
            return Json(new { data= Companies });
        }
        #endregion
    }
}
