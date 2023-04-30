using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Web_App.Services;

namespace Web_App.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IEmailService emailService;

        public RegisterModel(UserManager<IdentityUser> userManager, IEmailService emailService)
        {
            this.userManager = userManager;
            this.emailService = emailService;

        }


        [BindProperty]
        public RegisterViewModel RegisterViewModel { get; set; }




        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if(!ModelState.IsValid) return Page();

            //Create the user
            var user = new IdentityUser
            {
                Email = RegisterViewModel.Email,
                UserName = RegisterViewModel.Email
            };

            var result =await this.userManager.CreateAsync(user, RegisterViewModel.Password);
            if (result.Succeeded)
            {
               var confirmationToken = 
                    await this.userManager.GenerateEmailConfirmationTokenAsync(user);

                var confirmationLink =Url.PageLink(pageName: "/Account/ConfirmEmail",
                    values : new { userId = user.Id , token = confirmationToken }
                    );

                
                await emailService.SendAsync("f.batzoni@gmail.com",
                    user.Email,
                    "Please confirm your Email",
                    $"Please click the link to confirm your Email : {confirmationLink}");

                return RedirectToAction("/Account/Login");
            }
            else
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("Register", error.Description);
                }

                return Page();
            }
        }

    }


    public class RegisterViewModel
    {
        [Required]
        [EmailAddress(ErrorMessage ="Invalid email address.")]
        public string Email { get; set;}
        [Required]
        [DataType(dataType:DataType.Password)]
        public string Password { get; set; }
    }
}
