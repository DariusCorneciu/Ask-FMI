using Ganss.Xss;
using Microsoft.AspNetCore.Authorization;
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
            var subcategorii = from subcategorie in db.SubCategorii.Include(sc => sc.Intrebari).ThenInclude(i => i.Comentarii).ThenInclude(u => u.User)
                                orderby subcategorie.Title
                                select subcategorie;
            ViewBag.Subcategorii = subcategorii;
            return View();
        }
        public ActionResult Show(int id)
        {
            SubCategorie subcategorie = db.SubCategorii.Find(id);
            var questions = db.Intrebari.Include(i => i.Comentarii).ThenInclude(uc=>uc.User).Include(u => u.User).Where(i => i.SubCategorieId == subcategorie.Id);
            int _perPage = 5;
            int totalQuestions = 0;
            var currentPage = Convert.ToInt32(HttpContext.Request.Query["page"]);
            var offset = 0;
            if(!currentPage.Equals(0))
            {
                offset = (currentPage - 1) * _perPage;
            }
            var paginatedQuestions = questions.Skip(offset).Take(_perPage);
            ViewBag.lastPage = Math.Ceiling((float)totalQuestions / (float)_perPage);
            ViewBag.Questions = paginatedQuestions;
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
            var sanitizer = new HtmlSanitizer();
            if (ModelState.IsValid)
            {
                subcategorie.Description = sanitizer.Sanitize(subcategorie.Description);
                subcategorie.Description = (subcategorie.Description);
                db.SubCategorii.Add(subcategorie);
                db.SaveChanges();
                TempData["message"] = "Subcategoria a fost adaugata!";
                return RedirectToAction("Index");
            }
            else
            {
                subcategorie.Categ = GetAllCategories();

                return View(subcategorie);
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            SubCategorie sb = db.SubCategorii.Include("Categorie").Include("Intrebari").Where(sc => sc.Id == id).First();
            sb.Categ = GetAllCategories();
            return View(sb);
        }
        [HttpPost]
        public ActionResult Edit(int id, SubCategorie requestedSb)
        {
            SubCategorie sb = db.SubCategorii.Find(id);
            var sanitizer = new HtmlSanitizer();
           if (ModelState.IsValid)
            {
                requestedSb.Description = sanitizer.Sanitize(requestedSb.Description);
                sb.Title = requestedSb.Title;
                sb.CategorieId = requestedSb.CategorieId;
                sb.Description = requestedSb.Description;
                db.SaveChanges();
                TempData["message"] = "Subcategoria a fost modificata!";
                return RedirectToAction("Index");
            }
            else
            {
                requestedSb.Categ = GetAllCategories();
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
