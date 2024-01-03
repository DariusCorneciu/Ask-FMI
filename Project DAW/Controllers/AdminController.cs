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
using System.Linq;

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
            var Moderatori = from moderatori in db.Users
                             where moderatori.Moderator ==true 
                             select moderatori;
            ViewBag.Moderatori = Moderatori;
            List<string> ImagesSrc = new List<string>();

           foreach(var mod in Moderatori)
            {
                ImagesSrc.Add(mod.ProfileImage());
                
            }

            
            return View(ImagesSrc);
        }

        public IActionResult Show(string id)
        {
            ApplicationUser Moderator = db.Users.Where(modid => modid.Id == id).First();
            ViewBag.Src = Moderator.ProfileImage();

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
                TempData["message-alert"] = "alert alert-succes";
                TempData["Reopen"] = "Intrebarea a fost redeschisa cu succes";
                return Redirect("/Admin/Show/" + mod);

            }
            else {
                TempData["message-alert"] = "alert alert-danger";

                TempData["Reopen"] = "A aparut o eroare la redeschiderea intrebarii!"; 
                return RedirectToAction("Index"); 
            }
        }
        public IActionResult Remove(string id)
        {
                ApplicationUser Moderator = db.Users.Where(i=>i.Id == id).First();
                

                if (Moderator != null)
                {
                Moderator.Moderator = false;
                db.SaveChanges();
                TempData["message-alert"] = "alert alert-success";

                TempData["Remove"] = "Moderator a fost eliminat cu succes";

            }
            else
                {
                TempData["message-alert"] = "alert alert-danger";

                TempData["Remove"] = "Nu s-a putut gasii Moderator!";
                }

            return RedirectToAction("Index");
        }
      
        /*
         
        ApplicationUser requser = db.Users.Find(userid);
                if (requser == null) {
                    TempData["Test"] = "Astea doua: "+userid + "," + roleid;
                    return RedirectToAction("Users");
                }
                if (db.UserRoles
                    .Where(ur=>ur.UserId == requser.Id && ur.RoleId == roleid).Count() == 0)
                {
                    db.SaveChanges();
                    TempData["message-alert"] = "alert alert-success";
                    TempData["AddRole"] = "Userului i s-a atribuit cu succes rolul!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["AddRole"] = "Userul are deja acest rol!";
                    return View(roleid,userid);
                }
                             
          
        
        
        [NonAction]
          public IEnumerable<SelectListItem> GetAllRoles()
          {

              var roles = new List<SelectListItem>();

              var roluri = new List<string> { "Moderator","Admitere","Licenta","Master"};           


              foreach (var r in roluri)
              {
                  roles.Add(new SelectListItem(
                      value:,
                      text: r
                      ));
              }

              return roles;

          }*/


    }
}
