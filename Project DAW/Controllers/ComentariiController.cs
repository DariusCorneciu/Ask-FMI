using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_DAW.Data;
using Project_DAW.Models;

namespace Project_DAW.Controllers
{
    
    public class ComentariiController : Controller
    {

        private readonly ApplicationDbContext db;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        public ComentariiController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        [Authorize(Roles ="User,Admin")]
        public IActionResult Delete(int id)
        {
            Comentariu comentariu = db.Comentarii.Find(id);
            ApplicationUser user = db.Users.Find(_userManager.GetUserId(User));
            if(comentariu.UserId == user.Id || User.IsInRole("Admin") ||user.Moderator )
            {
                db.Comentarii.Remove(comentariu);
                db.SaveChanges();
                return Redirect("/Intrebari/Show/"+comentariu.IntrebareId);
            }
            else
            {
                TempData["NoAcces"] = "Nu ai acces sa stergi acest comentariu";
                TempData["type"] = "alert-danger";
                return Redirect("/Intrebari/Index");
            }
        }


        public IActionResult New(int id)
        {
            Intrebare Intrebare = db.Intrebari.Include(u => u.User).Where(i => i.Id == id).First();
            ViewBag.Intrebare = Intrebare;

            Comentariu newcomment = new Comentariu();
            return View(newcomment);
        }
        [HttpPost]
        public IActionResult New([FromForm]Comentariu newcom)
        {
           newcom.Date = DateTime.Now;
            newcom.UserId = _userManager.GetUserId(User);
            newcom.User = db.Users.Where(u => u.Id == newcom.UserId).FirstOrDefault();

            if (ModelState.IsValid)
            {
                db.Comentarii.Add(newcom);
                db.SaveChanges();
                return Redirect("/Intrebari/Show/" + newcom.IntrebareId);
            }
            else
            {
                Intrebare Intrebare = db.Intrebari.Include(u => u.User).Where(i => i.Id == newcom.IntrebareId).First();
                ViewBag.Intrebare = Intrebare;

                return View(newcom);

            }
        }

        public IActionResult Edit(int id)
        {
            Comentariu comentariu = db.Comentarii.Find(id);
            return View(comentariu);


            
        }
        [HttpPost]
        public IActionResult Edit(int id,Comentariu reqcomment)
        {
            Comentariu comment = db.Comentarii.Find(id);

            if(comment.UserId == _userManager.GetUserId(User))
            {
                if (ModelState.IsValid)
                {
                    comment.Continut = reqcomment.Continut;
                    db.SaveChanges();
                    TempData["EditCommSucc"] = "Ai editat comentariul cu succes!";
                    return Redirect("/Intrebari/Show/" + comment.IntrebareId);

                }
                else
                {
                    return View(reqcomment);
                }
                
            }
            else
            {
                TempData["EditError"] = "Nu ai voie sa editezi acest comentariu!";
                return RedirectToAction("/Intrebari/Show/" + comment.IntrebareId);
            }
        }
    }

}
