using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project_DAW.Data;
using Project_DAW.Models;

namespace Project_DAW.Controllers
{
    [Authorize]

    public class RaspunsuriController : Controller
    {
        private readonly ApplicationDbContext db;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        public RaspunsuriController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

     
        [HttpPost]
        [Authorize(Roles="Moderator,Admin")]
        public IActionResult New(Raspuns raspuns)
        {
            raspuns.Date = DateTime.Now;
            raspuns.UserId = _userManager.GetUserId(User);
            if (ModelState.IsValid)
            {
                db.Raspunsuri.Add(raspuns);
                db.SaveChanges();
                return Redirect("/Intrebari/Show/" + raspuns.IntrebareId);

            }
            else
            {
                return View(raspuns);
            }
        }
    }
}
