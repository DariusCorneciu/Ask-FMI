using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Project_DAW.Data;
using Project_DAW.Models;

namespace Project_DAW.Controllers
{
    [Authorize(Roles ="User,Admin")]
    public class ModeratorController : Controller
    {
        private readonly ApplicationDbContext db;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        public ModeratorController(
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
            ApplicationUser user = db.Users.Find(_userManager.GetUserId(User));

            if (!user.Moderator)
            {
                if (User.IsInRole("Admin")) { } else
                {
                    TempData["Mod"] = "Ai fost scos din functia de moderator si nu mai poti accesa pagina!";

                    return Redirect("/Home/Index");

                }

            }
            
            var intrebari_fara_raspuns = db.Intrebari.Include("User").Where(a => a.IsOpen == true).ToList();

            ViewBag.intrebari_fara_raspuns = intrebari_fara_raspuns;
            return View();
        }
        [HttpPost]
        public IActionResult Index([FromForm] Raspuns raspuns)
        {
            {
                raspuns.Date = DateTime.Now;
                raspuns.UserId = _userManager.GetUserId(User);
                ApplicationUser user = db.Users.Find(raspuns.UserId);
                if (!user.Moderator)
                {
                    TempData["Mod"] = "Ai fost scos din functia de moderator si nu mai poti accesa pagina!";
                    return Redirect("/Home/Index");

                }
                if (ModelState.IsValid)
                {
                   
                    if (db.Raspunsuri.Where(rasp => rasp.IntrebareId == raspuns.IntrebareId).ToList().Count() > 0)
                    {
                        TempData["Exista"] = "Cineva a raspuns la acest ticket deja!";
                    }
                    else
                    {
                        Intrebare intrebare = db.Intrebari.Where(intr => intr.Id == raspuns.IntrebareId).First();
                        intrebare.IsOpen = false;
                        db.Raspunsuri.Add(raspuns);
                        db.SaveChanges();
                        var intrebari_fara_raspuns = db.Intrebari.Include("User").Where(a => a.IsOpen == true).ToList();
                        ViewBag.intrebari_fara_raspuns = intrebari_fara_raspuns;

                        }
                    return View();

                }
                else
                {
                    var intrebari_fara_raspuns = db.Intrebari.Include("User").Where(a => a.IsOpen == true).ToList();
                    ViewBag.intrebari_fara_raspuns = intrebari_fara_raspuns;

                    return View();
                }
            }
        }
    }
}
