using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project_DAW.Data;
using Project_DAW.Models;

namespace Project_DAW.Controllers
{
    public class SubCategoriiController : Controller
    {
        private readonly ApplicationDbContext db;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;
        public SubCategoriiController(
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
            SetAccessRights();
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
            SubCategorie subcategorie = db.SubCategorii.Include("Categorie").Include("Intrebari").Where(sc => sc.Id == id).First();
            TempData["Source"] = "SubCategorii";
            TempData["IdSC"] = subcategorie.Id;
            SetAccessRights();
            return View(subcategorie);
        }
        public ActionResult New()
        {
            SubCategorie subCategorie = new SubCategorie();
            subCategorie.Categ = GetAllCategories();
            return View(subCategorie);
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
        [NonAction]
        public IEnumerable<SelectListItem> GetAllCategories()
        {
            var selectList = new List<SelectListItem>();
            var categories = from c in db.Categorii
                             select c;
            foreach (var  c in categories)
            {
                selectList.Add(new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name.ToString(),
                });
            }
            return selectList;
        }
    }
}
