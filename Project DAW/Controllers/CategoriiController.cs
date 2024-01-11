using Ganss.Xss;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_DAW.Data;
using Project_DAW.Models;
using System.Data;

namespace Project_DAW.Controllers
{
    public class CategoriiController : Controller
    {
        private readonly ApplicationDbContext db;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        public CategoriiController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }
            var categorii = from category in db.Categorii.Include(u=>u.SubCategorii).ThenInclude(u=>u.Intrebari).ThenInclude(u=>u.User)
                            orderby category.Name
                            select category;
            ViewBag.Categorii = categorii;
            SetAccessRights();
            return View();
        }
        public ActionResult Show(int id)
        {
            Categorie category = db.Categorii.Find(id);
            var subcategorii = db.SubCategorii.Include(u => u.Intrebari).ThenInclude(u => u.User).Where(sc => sc.CategorieId == category.Id);
            int _perPage = 5;
            int totalSC = 0;
            var currentPage = Convert.ToInt32(HttpContext.Request.Query["page"]);
            var offset = 0;
            if(!currentPage.Equals(0))
            {
                offset = (currentPage - 1) * _perPage;
            }
            var paginatedSC = subcategorii.Skip(offset).Take(_perPage);
            ViewBag.lastPage = Math.Ceiling((float)totalSC / (float)_perPage);
            ViewBag.SubCategorii = paginatedSC;
            SetAccessRights();
            return View(category);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult New() 
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id) 
        {
            Categorie categorie = db.Categorii.Find(id);
            return View(categorie);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
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
        private void SetAccessRights()
        {
            ViewBag.AfisareButoane = false;

            if (User.IsInRole("Moderator"))
            {
                ViewBag.AfisareButoane = true;
            }

            ViewBag.EsteAdmin = User.IsInRole("Admin");

            ViewBag.UserCurent = _userManager.GetUserId(User);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            Categorie categorie = db.Categorii.Include(c => c.SubCategorii).Where(c => c.Id == id).First();
            db.Categorii.Remove(categorie);
            TempData["message"] = "Categoria a fost stearsa";
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
