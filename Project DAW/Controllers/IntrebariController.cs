using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project_DAW.Data;
using Project_DAW.Models;

namespace Project_DAW.Controllers
{
    public class IntrebariController : Controller
    {

        private readonly ApplicationDbContext db;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        public IntrebariController(
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
            var intrebari = db.Intrebari.Include("Raspuns");

            ViewBag.intrebari = intrebari;
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
                ViewBag.Alert = TempData["messageType"];
            }
            return View();
        }
        public IActionResult Show(int id)
        {
            Intrebare intrebare = db.Intrebari.Include("Raspuns").Include("Comentarii").Where(q => q.Id == id).First();
            return View(intrebare);
        }
        [Authorize(Roles = "Moderator,Admitere,Licenta,Master,Admin")]
        public IActionResult New()
        {
            Intrebare intrebare = new Intrebare();

            if (User.IsInRole("Licenta"))
            {
                intrebare.SubCateg = GetAllCategories("Licenta");
            }
            else if(User.IsInRole("Master"))
            {
                intrebare.SubCateg = GetAllCategories("Master");
            }
            else if (User.IsInRole("Admitere"))
            {
                intrebare.SubCateg = GetAllCategories("Admitere");
            }
            return View(intrebare);
         }
        [HttpPost]
        [Authorize(Roles = "Moderator,Admitere,Licenta,Master,Admin")]

        public IActionResult New(Intrebare intrebare)
        {
            intrebare.Date = DateTime.Now;
            intrebare.UserId = _userManager.GetUserId(User);
            if(ModelState.IsValid)
            {
                db.Intrebari.Add(intrebare);
                db.SaveChanges();
                TempData["message"] = "Intrebarea a fost adaugata!";
                TempData["messageType"] = "alert-success";
                return RedirectToAction("Index");
            }
            else
            {
                if(User.IsInRole("Licenta"))
                {
                    intrebare.SubCateg = GetAllCategories("Licenta");
                }
                else if (User.IsInRole("Master"))
                {
                    intrebare.SubCateg = GetAllCategories("Master");
                }
                else if (User.IsInRole("Admitere"))
                {
                    intrebare.SubCateg = GetAllCategories("Admitere");
                }
                return View(intrebare);
            }
        }

        [Authorize(Roles = "Admitere,Licenta,Master,Admin")]
        public IActionResult Edit(int id)
        {
            Intrebare intrebare = db.Intrebari.Include("Raspunsuri").Where(i => i.Id == id).First();
            if (User.IsInRole("Licenta"))
            {
                intrebare.SubCateg = GetAllCategories("Licenta");
            }
            else if (User.IsInRole("Master"))
            {
                intrebare.SubCateg = GetAllCategories("Master");
            }
            else if (User.IsInRole("Admitere"))
            {
                intrebare.SubCateg = GetAllCategories("Admitere");
            }
            if (intrebare.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
                return View(intrebare);
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra unei intrebare care a fost pusa de dumneavoastra";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("Index");
            }
        }

        [Authorize(Roles = "Admitere,Licenta,Master,Admin")]
        public IActionResult Edit(int id, Intrebare returnedq)
        {
            Intrebare q = db.Intrebari.Find(id);
            if(ModelState.IsValid)
            {
                if(q.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
                {
                    q.Name = returnedq.Name;
                    q.Continut = returnedq.Continut;
                    q.SubCategorie = returnedq.SubCategorie;
                    q.SubCategorieId = returnedq.SubCategorieId;
                    TempData["message"] = "Intrebarea a fost modificata";
                    TempData["messageType"] = "alert-success";
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra unei intrebare care a fost pusa de dumneavoastra";
                    TempData["messageType"] = "alert-danger";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                if (User.IsInRole("Licenta"))
                {
                    returnedq.SubCateg = GetAllCategories("Licenta");
                }
                else if (User.IsInRole("Master"))
                {
                    returnedq.SubCateg = GetAllCategories("Master");
                }
                else if (User.IsInRole("Admitere"))
                {
                    returnedq.SubCateg = GetAllCategories("Admitere");
                }
                return View(returnedq);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Moderator,Admitere,Licenta,Master,Admin")]
        public ActionResult Delete(int id)
        {
            Intrebare intrebare = db.Intrebari.Include("Raspunsuri").Include("Comentarii")
                                         .Where(art => art.Id == id)
                                         .First();

            if (intrebare.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
                db.Intrebari.Remove(intrebare);
                db.SaveChanges();
                TempData["message"] = "Articolul a fost sters";
                TempData["messageType"] = "alert-success";
                return RedirectToAction("Index");
            }

            else
            {
                TempData["message"] = "Nu aveti dreptul sa stergeti o intrebare care nu va apartine";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("Index");
            }
        }

        public IEnumerable<SelectListItem> GetAllCategories(string rol)
        {
            var selectList = new List<SelectListItem>();

            var subcategorii = from subcat in db.SubCategorii.Include("Categorii").Where(sb => sb.Categorie.Name == rol)
                               select subcat;

            foreach (var subcategorie in subcategorii)
            {
                selectList.Add(new SelectListItem
                {
                    Value = subcategorie.Id.ToString(),
                    Text = subcategorie.Title.ToString()
                });
            }
            return selectList;
        }
    }
}
