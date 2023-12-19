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
            TempData["Source"] = "Intrebari";

            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
                ViewBag.Alert = TempData["messageType"];
            }
            return View();
        }
        public IActionResult Show(int id)
        {
            //  TempData["Test"] = "E in SHow";

            Intrebare intrebare = db.Intrebari
                                .Include(q => q.Comentarii)
                                .ThenInclude(c => c.User)
                                .Include(q => q.User)
                                .Include(q => q.Raspuns)
                                .Where(q => q.Id == id)
                                .First();
            ViewBag.Intoarcere = TempData["Source"];
            GetRole();
            return View(intrebare);
        }

        [HttpPost]
        public IActionResult Show([FromForm] Comentariu comentariu)
        {
             
            comentariu.Date = DateTime.Now;
            comentariu.UserId = _userManager.GetUserId(User);
            comentariu.User = db.Users.Where(u => u.Id  == comentariu.UserId).FirstOrDefault();

            if (ModelState.IsValid)
            {
                db.Comentarii.Add(comentariu);
                db.SaveChanges();
                GetRole();
                TempData["Test"] = "Este valid" + comentariu.Date+comentariu.IntrebareId+" "+ comentariu.Id;
                return Redirect("/Intrebari/Show/"+comentariu.IntrebareId);
            }
            else
            {
                TempData["Test"] = "Nu este valid" + comentariu.Date + comentariu.IntrebareId + " " + comentariu.Id; 
                Intrebare intrebare = db.Intrebari.Include("User").Include("Comentarii.User").Include("Comentarii").Include("Raspuns").Where(a => a.Id == comentariu.IntrebareId).First();
                GetRole();
                return View(intrebare);
            }

        }
        [Authorize(Roles = "Moderator,Admitere,Licenta,Master,Admin,User")]
        public IActionResult New(int? idsc)
        {
            Intrebare intrebare = new Intrebare();
            if (idsc != null)
            {
                intrebare.SubCategorieId = (int)idsc;
               
            }
            else
            {
                
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
                    else if (User.IsInRole("Admin"))
                    {
                        intrebare.SubCateg = GetAllCategories("Admin");
                    }
                    else if (User.IsInRole("Moderator"))
                    {
                        intrebare.SubCateg = GetAllCategories("Admin");
                    }
                
                
            }
            return View(intrebare);
         }
        [HttpPost]
        [Authorize(Roles = "Moderator,Admitere,Licenta,Master,Admin,User")]

        public IActionResult New(Intrebare intrebare)
        {
            if(ModelState.IsValid)
            {
                intrebare.Date = DateTime.Now;
                intrebare.UserId = _userManager.GetUserId(User);
                intrebare.IsOpen = true;
                db.Intrebari.Add(intrebare);
                db.SaveChanges();
                TempData["message"] = "Intrebarea a fost adaugata!";
                TempData["messageType"] = "alert-success";
                return RedirectToAction("Index");
            }
            else
            {
                if(intrebare.IsOpen == false)
                {
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
                    else if (User.IsInRole("Admin"))
                    {
                        intrebare.SubCateg = GetAllCategories("Admin");
                    }



                }



                return View(intrebare);
            }
        }

        [Authorize(Roles = "Admitere,Licenta,Master,Admin")]
        public IActionResult Edit(int id)
        {
            
            Intrebare intrebare = db.Intrebari.Where(i => i.Id == id).First();
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
                intrebare.SubCateg = GetAllCategories("Admin");
                return View(intrebare);
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra unei intrebare care a fost pusa de dumneavoastra";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
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

            
                Intrebare intrebare = db.Intrebari.Include(i=>i.Raspuns).Include(i=>i.Comentarii)
                                         .Where(art => art.Id == id)
                                         .First();

            
            
            if (intrebare.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
                db.Intrebari.Remove(intrebare);
                if(intrebare.Raspuns != null)
                {
                    db.Raspunsuri.Remove(intrebare.Raspuns);
                }
                if(intrebare.Comentarii != null)
                {
                    foreach(Comentariu comentariu in intrebare.Comentarii)
                    {
                        db.Comentarii.Remove(comentariu);
                    }
                }
                db.SaveChanges();
                TempData["message"] = "Intrebarea a fost stearsa";
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

        private void GetRole()
        {

            ViewBag.EsteMod = User.IsInRole("Moderator");
            ViewBag.EsteAdmin = User.IsInRole("Admin");
            ViewBag.EsteUser = User.IsInRole("User") || User.IsInRole("Admitere") || User.IsInRole("Admin") || User.IsInRole("Moderator") || User.IsInRole("Master") || User.IsInRole("Licenta");

            ViewBag.UserCurent = _userManager.GetUserId(User);
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

        public IEnumerable<SelectListItem> GetAllCategories(string rol)
        {
            var selectList = new List<SelectListItem>();
            var subcategorii = from subcat in db.SubCategorii.Include("Categorie").Where(sb => sb.Categorie.Name == rol)
                               select subcat;
            if (rol == "Admin")
            {
                subcategorii = from subcat in db.SubCategorii.Include("Categorie")
                                   select subcat;
            }
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
