using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_DAW.Data;
using Project_DAW.Models;
using System.Data;

namespace Project_DAW.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoriiController : Controller
    {
        private readonly ApplicationDbContext db;

        public CategoriiController(ApplicationDbContext context)
        {
            db = context;
        }
        public IActionResult Index()
        {
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }
            var categorii = from category in db.Categorii
                            orderby category.Name
                            select category;
            ViewBag.Categorii = categorii;
            return View();
        }
        public ActionResult Show(int id)
        {
            Categorie category = db.Categorii.Include("SubCategorii").Where(s => s.Id == id).First();
            ViewBag.Categorii = category;
            
            return View(category);
        }
        public ActionResult New() 
        {

            return View();
        }
        [HttpPost]
        public ActionResult New(Categorie categorie)
        {
            if (ModelState.IsValid)
            {
                db.Categorii.Add(categorie);
                db.SaveChanges();
                TempData["message"] = "Categoria a fost adaugata!";
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var modelError in ModelState.Values.SelectMany(v => v.Errors))
                {
                    TempData["Test"] = "Eroare în model: " + modelError.ErrorMessage;
                }
                return View(categorie);
            }
        }
        public ActionResult Edit(int id) 
        {
            Categorie categorie = db.Categorii.Find(id);
            return View(categorie);
        }

        [HttpPost]
        public ActionResult Edit(int id, Categorie requestedCategory)
        {
            Categorie categorie = db.Categorii.Find(id);
            if (ModelState.IsValid)
            {
                categorie.Name = requestedCategory.Name;
                db.SaveChanges();
                TempData["message"] = "Categoria a fost modificata!";
                return RedirectToAction("Index");
            }
            else
            {
                return View(requestedCategory);
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            Categorie categorie = db.Categorii.Find(id);
            db.Categorii.Remove(categorie);
            TempData["message"] = "Categoria a fost stearsa";
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
