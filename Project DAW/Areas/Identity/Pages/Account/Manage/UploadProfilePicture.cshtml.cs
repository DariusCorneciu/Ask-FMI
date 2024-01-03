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

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UploadProfilePictureModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
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
            [Display(Name ="Poza de profil")]
            public IFormFile Imagine { get; set; }

            [Display(Name = "Backround")]
            public IFormFile Backround { get; set; }


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
            

            if (Input.Imagine == null && Input.Backround == null)
            {
                await LoadAsync(user);
                StatusMessage = "Error :Nu ai schimbat nici poza de profil nici cea de fundal";
                return Page();
            }
            if(Input.Imagine != null)
            {
                if (user.ProfilePicture != null)
                {
                    StatusMessage = "Ti-am sters automat poza vechie de profil si am inlocuito cu cea noua";

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
                        user.ProfilePicture = memoryStream.ToArray();
                    }


                    if (((float)Input.Imagine.Length) / 1000 > 250)
                    {
                        StatusMessage = "Error : Dimensiunile fisierului sunt prea mari!";
                        return Page();

                    }
                    user.ProfileType = Input.Imagine.ContentType;

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Error uploading image: {ex.Message}");
                    return Page();
                }



            }
            if (Input.Backround != null)
            {
                if (user.BackroundPicture != null)
                {
                    StatusMessage = "Ti-am sters automat poza vechie de fundal si am inlocuito cu cea noua";

                }
                else
                {
                    StatusMessage = "Ti-am adaugat noua poza de fundal <3";
                }
                try
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        await Input.Backround.CopyToAsync(memoryStream);
                        user.BackroundPicture = memoryStream.ToArray();
                    }


                    if (((float)Input.Backround.Length) / 1000 > 250)
                    {
                        StatusMessage = "Error : Dimensiunile fisierului sunt prea mari!";
                        return Page();

                    }
                    user.BackroundType = Input.Backround.ContentType;

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"Error uploading image: {ex.Message}");
                    return Page();
                }



            }



            await _userManager.UpdateAsync(user);
            await _signInManager.RefreshSignInAsync(user);
            return RedirectToPage();
        }
    }
}
