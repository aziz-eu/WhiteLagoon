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
    public class AmenityController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AmenityController (IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var amenity = _unitOfWork.Amenity.GetAll(includeProperties: "Villa");
            return View(amenity);
        }
        public IActionResult Create()
        {
            AmenityVM amenityVM = new AmenityVM()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                })
            };
         
            return View(amenityVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AmenityVM obj)
        {
            try
            {
               
                //ModelState.Remove("Villa");
                if (ModelState.IsValid)
                {
                    _unitOfWork.Amenity.Add(obj.Amenity);
                    _unitOfWork.Save();
                    TempData["success"] = "New Amenity Add Successfully";
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

        public IActionResult Update(int id)
        {
            AmenityVM? amenityVM = new AmenityVM()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                { Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Amenity = _unitOfWork.Amenity.Get(u => u.Id == id)                
            };

            if(amenityVM.Amenity == null)
            {
                return NotFound();
            }
          


            return View(amenityVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(AmenityVM obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _unitOfWork.Amenity.Update(obj.Amenity);
                    _unitOfWork.Save();
                    TempData["success"] = "Amenity successfully Updated";
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

        public IActionResult Delete(int id)
        {



            AmenityVM? amenityVM = new AmenityVM()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Amenity = _unitOfWork.Amenity.Get(u => u.Id == id)
        };
            if(amenityVM == null)
            {
                return NotFound();
            }
           

            return View(amenityVM);



        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(AmenityVM obj)
        {

            try{

                Amenity? amenity = _unitOfWork.Amenity.Get(u => u.Id == obj.Amenity.Id);           

               if(amenity is not null)
                {
                    _unitOfWork.Amenity.Remove(amenity);
                    _unitOfWork.Save();
                    TempData["success"] = "Amenity Deleted Successfully";
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
