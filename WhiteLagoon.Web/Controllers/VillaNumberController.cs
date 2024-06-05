using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WhiteLagoon.Application.Common.Interface;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;
using WhiteLagoon.Web.Models.ViewModels;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public VillaNumberController (IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var villaNumbers = _unitOfWork.VillaNumber.GetAll(includeProperties: "Villa");
            return View(villaNumbers);
        }
        public IActionResult Create()
        {
            VillaNumberVM villaNumberVM = new VillaNumberVM()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                })
            };
         
            return View(villaNumberVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(VillaNumberVM obj)
        {
            try
            {
                bool isRoomExits = _unitOfWork.VillaNumber.GetAll().Any(u => u.Villa_Number == obj.VillaNumber.Villa_Number);

                if (isRoomExits)
                {
                    obj.VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                    {
                        Text = u.Name, Value = u.Id.ToString()
                    });;

                    TempData["error"] = "Villa Already Exits!";
                    return View(obj);
                }
                //ModelState.Remove("Villa");
                if (ModelState.IsValid)
                {

                    _unitOfWork.VillaNumber.Add(obj.VillaNumber);
                    _unitOfWork.VillaNumber.Save();
                    TempData["success"] = "New Villa Number Add Successfully";
                    return RedirectToAction(nameof(Index));

                }
                TempData["error"] = "Something went Wrong!";
                return View(obj);
            }
            catch (Exception ex)
            {
                TempData["error"] = "Something went Wrong!";
                return View(obj);
            }
        }

        public IActionResult Update(int villaNumberId)
        {
            VillaNumberVM? villaNumberVM = new VillaNumberVM()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                { Text = u.Name,
                    Value = u.Id.ToString()
                }),
                VillaNumber = _unitOfWork.VillaNumber.Get(u => u.Villa_Number == villaNumberId)                
            };

            if(villaNumberVM.VillaNumber == null)
            {
                return NotFound();
            }
          


            return View(villaNumberVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(VillaNumberVM obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _unitOfWork.VillaNumber.Update(obj.VillaNumber);
                    _unitOfWork.VillaNumber.Save();
                    TempData["success"] = "Villa Number successfully Updated";
                    return RedirectToAction(nameof(Index));
                }

                obj.VillaList = _unitOfWork.Villa.GetAll().Select(u=> new SelectListItem
                {
                    Value = u.Name,
                    Text = u.Id.ToString()
                });

                TempData["error"] = "Something went Wrong";
                return View(obj);
            }
            catch(Exception ex)
            {
                TempData["error"] = "Something went Wrong";
                return View(obj);
            }
        }

        public IActionResult Delete(int villaNumberId)
        {



            VillaNumberVM? villaNumberVM = new VillaNumberVM()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                VillaNumber = _unitOfWork.VillaNumber.Get(u => u.Villa_Number == villaNumberId)
        };
            if(villaNumberVM == null)
            {
                return NotFound();
            }
           

            return View(villaNumberVM);



        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(VillaNumberVM obj)
        {

            try{

                VillaNumber? villaNumber = _unitOfWork.VillaNumber.Get(u => u.Villa_Number == obj.VillaNumber.Villa_Number);           

               if(villaNumber is not null)
                {
                    _unitOfWork.VillaNumber.Remove(villaNumber);
                    _unitOfWork.VillaNumber.Save();
                    TempData["success"] = "Villa Number Deleted Successfully";
                    return RedirectToAction(nameof(Index));
                }


                TempData["error"] = "Something Went Wrong!";
                return View(obj);
            }
            catch{
                TempData["error"] = "Something Went Wrong!";
                return View(obj);
            }

        }
    }
}
