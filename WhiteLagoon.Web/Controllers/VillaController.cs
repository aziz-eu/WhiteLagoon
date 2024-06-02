using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using WhiteLagoon.Application.Common.Interface;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public VillaController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ;
        }
        public IActionResult Index()
        {
            var villas = _unitOfWork.Villa.GetAll();
            return View(villas);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Villa obj)
        {
            try
            {
                if (obj.Name == obj.Description)
                {
                    ModelState.AddModelError("", "Villa Name and Drscription Can't be Same!");
                }
                if (ModelState.IsValid)
                {

                    _unitOfWork.Villa.Add(obj);
                    _unitOfWork.Villa.Save();
                    TempData["success"] = "New Villa Add Successfully";
                    return RedirectToAction(nameof(Index));

                }
                TempData["error"] = "Something went Wrong";
                return View(obj);
            }
            catch (Exception ex)
            {
                TempData["error"] = "Something went Wrong";
                return View(obj);
            }
        }

        public IActionResult Update(int id)
        {
           
            Villa? villa = _unitOfWork.Villa.Get(u=> u.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            return View(villa);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Villa obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _unitOfWork.Villa.Update(obj);
                    _unitOfWork.Villa.Save();
                    TempData["success"] = "Villa successfully Updated";
                    return RedirectToAction(nameof(Index));
                }
                TempData["error"] = "Something went Wrong";
                return View(obj);
            }
            catch(Exception ex)
            {
                TempData["error"] = "Something went Wrong";
                return View(obj);
            }
        }

        public IActionResult Delete(int id)
        {
            Villa? villa = _unitOfWork.Villa.Get(u=>u.Id == id);

            if(villa == null) 
                return NotFound();

            return View(villa);


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Villa villaObj)
        {
            try{
                Villa? villa = _unitOfWork.Villa.Get(u => u.Id == villaObj.Id);
                if (villa == null)
                    return NotFound();

                _unitOfWork.Villa.Remove(villa);
                _unitOfWork.Villa.Save();
                TempData["success"] = "Villa successfully deleted";
                return RedirectToAction(nameof(Index));
            }
            catch{
                TempData["error"] = "Something went wrong!";
                return View(villaObj);
            }

        }
    }
}
