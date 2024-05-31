using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;
using WhiteLagoon.Web.Models.ViewModels;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly ApplicationDbContext _db;

        public VillaNumberController (ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var villaNumbers = _db.VillaNumbers.Include(u=> u.Villa).ToList();
            return View(villaNumbers);
        }
        public IActionResult Create()
        {
            VillaNumberVM villaNumberVM = new VillaNumberVM()
            {
                VillaList = _db.Villas.ToList().Select(u => new SelectListItem
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
                bool isRoomExits = _db.VillaNumbers.Any(u => u.Villa_Number == obj.VillaNumber.Villa_Number);

                if (isRoomExits)
                {
                    obj.VillaList = _db.Villas.ToList().Select(u => new SelectListItem
                    {
                        Text = u.Name, Value = u.Id.ToString()
                    });;

                    TempData["error"] = "Villa Already Exits";
                    return View(obj);
                }
                //ModelState.Remove("Villa");
                if (ModelState.IsValid)
                {

                    _db.VillaNumbers.Add(obj.VillaNumber);
                    _db.SaveChanges();
                    TempData["success"] = "New Villa Number Add Successfully";
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
           
            Villa? villa = _db.Villas.FirstOrDefault(u=> u.Id == id);
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
                    _db.Villas.Update(obj);
                    _db.SaveChanges();
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
            Villa? villa = _db.Villas.FirstOrDefault(u=>u.Id == id);

            if(villa == null) 
                return NotFound();

            return View(villa);


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Villa villaObj)
        {

            try{

               

                Villa? villa = _db.Villas.FirstOrDefault(u => u.Id == villaObj.Id);
                if (villa == null)
                    return NotFound();

                _db.Villas.Remove(villa);
                _db.SaveChanges();
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
