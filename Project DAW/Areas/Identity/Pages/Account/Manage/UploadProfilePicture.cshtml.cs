// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Project_DAW.Data;
using Project_DAW.Models;

namespace Project_DAW.Areas.Identity.Pages.Account.Manage
{
    public class UploadProfilePictureModel : PageModel
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UploadProfilePictureModel(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager
            )
        {
            db = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required(ErrorMessage ="Nu poti da submit la nimic!")]
            [Display(Name ="Poza de profil")]
            public IFormFile Imagine { get; set; }

        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
          

            Username = userName;

           
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }
            Imagine img = new Imagine();

           if(user.ProfilePicture)
            {
                var old = db.Imagini.Where(imga => imga.UserId == user.Id).Where(imga => imga.Usage == "Profile");
                StatusMessage = "Ti-am sters automat poza vechie de profil si am inlocuito cu cea noua";
                user.ProfilePicture = false;
                foreach(Imagine oldimg in old)
                {
                    user.Imagini.Remove(oldimg);
                }
                
            }
            else
            {
                StatusMessage = "Ti-am adaugat noua poza de profil <3";
            }
            try
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    await Input.Imagine.CopyToAsync(memoryStream);
                    img.ImageData = memoryStream.ToArray();
                }

                img.Name = Input.Imagine.FileName;
                if(((float)Input.Imagine.Length)/1000 > 250)
                {
                    StatusMessage = "Error : Dimensiunile fisierului sunt prea mari!";
                    return Page();

                }
                img.Type = Input.Imagine.ContentType;
                img.UserId = user.Id;
                img.Usage = "Profile";
                img.Size = Input.Imagine.Length;
                user.ProfilePicture = true;
                db.Imagini.Add(img);
                db.SaveChanges();

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error uploading image: {ex.Message}");
                return Page();
            }

            



            await _signInManager.RefreshSignInAsync(user);
            return RedirectToPage();
        }
    }
}
