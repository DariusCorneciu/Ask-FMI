using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        [Authorize(Roles ="User,Admin,Moderator")]
        public IActionResult Delete(int id)
        {
            Comentariu comentariu = db.Comentarii.Find(id);
            if(comentariu.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin") || User.IsInRole("Moderator"))
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
