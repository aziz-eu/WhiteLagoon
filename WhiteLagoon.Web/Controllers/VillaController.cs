using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly ApplicationDbContext _db;

        public VillaController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var villas = _db.Villas.ToList();
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

                    _db.Add(obj);
                    _db.SaveChanges();
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
