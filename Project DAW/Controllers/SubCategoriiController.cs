using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_DAW.Data;
using Project_DAW.Models;

namespace Project_DAW.Controllers
{
    public class SubCategoriiController : Controller
    {
        private readonly ApplicationDbContext db;
        public SubCategoriiController(ApplicationDbContext context)
        {
            db = context;
        }
        public IActionResult Index()
        {
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }
            var subcategorii = from subcategorie in db.SubCategorii
                                orderby subcategorie.Title
                                select subcategorie;
            ViewBag.Subcategorii = subcategorii;
            return View();
        }
        public ActionResult Show(int id)
        {
            SubCategorie subcategorie = db.SubCategorii.Find(id); 
            return View(subcategorie);
        }
        public ActionResult New()
        {
            return View();
        }
        [HttpPost]
        public ActionResult New(SubCategorie subcategorie)
        {
            if (ModelState.IsValid)
            {
                db.SubCategorii.Add(subcategorie);
                db.SaveChanges();
                TempData["message"] = "Subcategoria a fost adaugata!";
                return RedirectToAction("Index");
            }
            else
            {
                return View(subcategorie);
            }
        }

        public ActionResult Edit(int id)
        {
            SubCategorie sb = db.SubCategorii.Include("Categorie").Include("Intrebari").Where(sc => sc.Id == id).First();
            return View(sb);
        }
        [HttpPost]
        public ActionResult Edit(int id, SubCategorie requestedSb)
        {
            SubCategorie sb = db.SubCategorii.Find(id);
            if (ModelState.IsValid)
            {
                sb.Title = requestedSb.Title;
                sb.CategorieId = requestedSb.CategorieId;
                db.SaveChanges();
                TempData["message"] = "Subcategoria a fost modificata!";
                return RedirectToAction("Index");
            }
            else
            {
                return View(requestedSb);
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            SubCategorie sb = db.SubCategorii.Include("Intrebari").Where(sc => sc.Id == id).First();
            db.SubCategorii.Remove(sb);
            TempData["message"] = "Subcategoria a fost stearsa";
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
