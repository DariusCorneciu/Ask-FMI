using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Project_DAW.Data;
using Project_DAW.Models;
using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;

namespace Project_DAW.Controllers
{
    [Authorize(Roles= "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext db;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        /*
         public IActionResult Show(int id)
        {
            Imagine imagine = db.Imagini.Find(id);
            var imagineBase64 = Convert.ToBase64String(imagine.ImageData);
            var imagineSrc = $"data:{imagine.Type};base64,{imagineBase64}";

            ViewBag.ImagineSrc = imagineSrc;
            return View();
        }
         */

        public IActionResult Index()
        {
            var Moderatori = from moderatori in db.Users.Include("Imagini")
                             join UserRole in db.UserRoles on moderatori.Id equals UserRole.UserId
                             join Role in db.Roles on UserRole.RoleId equals Role.Id
                             where Role.Name == "Moderator" 
                             select moderatori;
            ViewBag.Moderatori = Moderatori;
            List<string> ImagesSrc = new List<string>();

            foreach(var mod in Moderatori)
            {
               
                if (db.Imagini.Where(img => img.UserId == mod.Id && img.Usage == "Profile").Count() > 0)
                {
                    Imagine img = db.Imagini.Where(img => img.UserId == mod.Id && img.Usage == "Profile").First();
                    var Sursa64 = Convert.ToBase64String(img.ImageData);
                    TempData["Test"] = img.Type;
                   var sursa = $"data:{img.Type};base64,{Sursa64}";
                    ImagesSrc.Add(sursa);

                }
                else
                {
                    ImagesSrc.Add("/images/default.jpg");
                }
                
            }

            
            return View(ImagesSrc);
        }

        public IActionResult Show(string id)
        {
            ApplicationUser Moderator = db.Users.Where(modid => modid.Id == id).First();
            if (Moderator.ProfilePicture)
            {
                Imagine Profile = db.Imagini.Where(p => p.UserId == Moderator.Id && p.Usage == "Profile").First();
                var Sursa64 = Convert.ToBase64String(Profile.ImageData);
                var sursa = $"data:{Profile.Type};base64,{Sursa64}";
                ViewBag.Src = sursa;
            }
            else
            {
                ViewBag.Src = "/images/default.jpg";
            }
           
            var nrraspunsuri = db.Raspunsuri.Where(mod => mod.UserId == id).Count();
            if (nrraspunsuri > 0)
            {
                var raspunsuri = db.Raspunsuri.Include(r=>r.Intrebare).Where(mod => mod.UserId == id).ToList();
                ViewBag.Raspunsuri = raspunsuri;
            }
            ViewBag.nrrasp = nrraspunsuri;


            return View(Moderator);
        }

        public IActionResult ReopenQuestion(int id)
        {
            Intrebare intrebare = db.Intrebari.
                Include(r => r.Raspuns).
                Where(i => i.Id == id).First();
           
            if (intrebare.Raspuns != null)
            {
                var mod = intrebare.Raspuns.UserId;
                db.Raspunsuri.Remove(intrebare.Raspuns);
                intrebare.IsOpen = true;
                db.SaveChanges();
                return Redirect("/Admin/Show/" + mod);


            }
            else { 
                TempData["Reopen"] = "A aparut o eroare la redeschiderea intrebarii!"; 
                return RedirectToAction("Index"); 
            }
        }
        public IActionResult Remove(string id)
        {
            IdentityRole modrole = db.Roles.Where(r => r.Name == "Moderator").First();
                ApplicationUser Moderator = db.Users.Where(i=>i.Id == id).First();
                

                if (Moderator != null && modrole !=null)
                {
                IdentityUserRole<string> delet = db.UserRoles.Where(r => r.UserId == Moderator.Id && r.RoleId == modrole.Id).First();
                db.UserRoles.Remove(delet);
                db.SaveChanges();
                  }
                else
                {
                TempData["Remove"] = "Nu s-a putut gasii userul";
                }

            TempData["Remove"] = "Moderator a fost eliminat cu succes";
            return RedirectToAction("Index");
        }
        public IActionResult Users()
        {
            var Users = from user in db.Users
                        select user;
            ViewBag.Useri = Users;

            List<string> ImagesSrc = new List<string>();
            IEnumerable<SelectListItem> roluri = GetAllRoles();

            dynamic roluri_imagini = new ExpandoObject();
            roluri_imagini.ImagesSrc = ImagesSrc;
            roluri_imagini.roluri = roluri;
            foreach (var user in Users)
            {

                if (db.Imagini.Where(img => img.UserId == user.Id && img.Usage == "Profile").Count() > 0)
                {
                    Imagine img = db.Imagini.Where(img => img.UserId == user.Id && img.Usage == "Profile").First();
                    var Sursa64 = Convert.ToBase64String(img.ImageData);
                    TempData["Test"] = img.Type;
                    var sursa = $"data:{img.Type};base64,{Sursa64}";
                    ImagesSrc.Add(sursa);

                }
                else
                {
                    ImagesSrc.Add("/images/default.jpg");
                }

            }


            return View(roluri_imagini);
        }
        [HttpPost]
        public IActionResult Users([FromForm]string roleid, [FromForm]string userid)
        {
            if(db.Roles.Find(roleid) != null)
            {
                IdentityRole rol = db.Roles.Find(roleid);

                ApplicationUser requser = db.Users.Find(userid);
                if (requser == null) {
                    TempData["Test"] = "Astea doua: "+userid + "," + roleid;
                    return RedirectToAction("Users");
                }
                if (db.UserRoles
                    .Where(ur=>ur.UserId == requser.Id && ur.RoleId == roleid).Count() == 0)
                {
                    IdentityUserRole<string> addrole = new IdentityUserRole<string> 
                    { 
                        RoleId = roleid, UserId = requser.Id
                    };
                    db.UserRoles.Add(addrole);
                    db.SaveChanges();
                    TempData["AddRole"] = "Userului i s-a atribuit cu succes rolul!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["AddRole"] = "Userul are deja acest rol!";
                    return View(roleid,userid);
                }


            }
            else
            {
                TempData["AddRole"] = "Rolul nu exista!";
                return RedirectToAction("Index");
            }
            
          

        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllRoles()
        {
            var roles = new List<SelectListItem>();
            var roluri = from rol in db.Roles select rol;
            foreach (var r in roluri)
            {
                roles.Add(new SelectListItem(
                    value: r.Id,
                    text: r.Name
                    ));
            }

            return roles;

        }

    }
}
