using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_DAW.Data;
using Project_DAW.Models;
using System.Dynamic;

namespace Project_DAW.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext db;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [Authorize(Roles ="Admin")]
        public IActionResult Index()
        {
            var Users = from user in db.Users
                        select user;
            ViewBag.Useri = Users;

            List<string> ImagesSrc = new List<string>();
            List<string> roluri = new List<string> { "Moderator", "Admitere", "Licenta", "Master" };
            dynamic roluri_imagini = new ExpandoObject();
            roluri_imagini.ImagesSrc = ImagesSrc;
            roluri_imagini.roluri = roluri;
            foreach (var user in Users)
            {
                ImagesSrc.Add(user.ProfileImage());

            }


            return View(roluri_imagini);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Index([FromForm] string role, [FromForm] string userid)
        {
            ApplicationUser user = db.Users.Find(userid);
            if (user != null)
            {
                bool semafor = false;
                if (role == "Moderator" && !user.Moderator) { user.Moderator = true; semafor = true; }
                if (role == "Admitere" && !user.Admitere) { user.Admitere = true; semafor = true; }
                if (role == "Master" && !user.Master) { user.Master = true; semafor = true; }
                if (role == "Licenta" && !user.Licenta) { user.Licenta = true; semafor = true; }

                if (semafor)
                {
                    db.SaveChanges();
                    TempData["message-alert"] = "alert alert-success";
                    TempData["AddRole"] = "Userului i s-a atribuit cu succes rolul!";
                    return Redirect("/Admin/Index");
                }
                else
                {
                    TempData["message-alert"] = "alert alert-danger";
                    TempData["AddRole"] = "Userul are deja acest rol!";
                    return RedirectToAction("Index");
                }



            }
            else
            {
                return Redirect("/Admin/Index");
            }



        }


        public IActionResult Show(string id)
        {
            var currentPage = Convert.ToInt32(HttpContext.Request.Query["page"]);
            ViewBag.pagina = currentPage;
           
            
            ApplicationUser user = db.Users.Include(u => u.Intrebari).Include(u => u.Comentarii)
                .Where(u => u.Id == id).First();
            
            
            if(user.Comentarii != null)
            {
                user = db.Users.Include(u => u.Intrebari).Include(u => u.Comentarii)
                    .ThenInclude(u => u.Intrebare).Where(u=>u.Id == id).First();


            }
            if (user == null)
            {
                return NotFound();
            }
            else
            {
              
                return View(user);
            }
        }
    }
}
