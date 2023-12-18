using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project_DAW.Data;
using Project_DAW.Models;

namespace Project_DAW.Controllers
{
    public class ImaginiController : Controller
    {
        private readonly ApplicationDbContext db;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        public ImaginiController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        
        public IActionResult New()
        {
            return View();
        }
        [HttpPost]
        public IActionResult New(IFormFile fisierImagine)
        {
            
            if (fisierImagine == null) { TempData["Error"] = "E null"; return View(); }
            var type = fisierImagine.ContentType.Split('/');
            if (type[0] != "image") { TempData["Error"] = "E nu e formatul corect"; return View(); }
            else if ((float)fisierImagine.Length/1000 > 250) { TempData["Error"] = "Imaginea este prea mare!"; return View(); }
            else
            {

                Imagine img = new Imagine();
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    fisierImagine.CopyTo(memoryStream);
                    img.ImageData = memoryStream.ToArray();
                }
                img.Name = fisierImagine.FileName;
                img.Type = fisierImagine.ContentType;
                img.UserId = _userManager.GetUserId(User);
                img.Size = fisierImagine.Length;
                db.Imagini.Add(img);
                db.SaveChanges();

                return Redirect("/Home/Index");


            }

        }

        public IActionResult Show(int id)
        {
            Imagine imagine = db.Imagini.Find(id);
            var imagineBase64 = Convert.ToBase64String(imagine.ImageData);
            var imagineSrc = $"data:{imagine.Type};base64,{imagineBase64}";

            ViewBag.ImagineSrc = imagineSrc;
            return View();
        }
    }
}
