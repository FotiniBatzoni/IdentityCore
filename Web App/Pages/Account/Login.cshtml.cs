using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Web_App.Data.Account;

namespace Web_App.Pages.Account
{
    public class LoginModel : PageModel
        {
            private readonly SignInManager<User> signInManager;

            public LoginModel(SignInManager<User> signInManager)
            {
                this.signInManager = signInManager;
            }

            [BindProperty]
            public CredentialViewModel Credential { get; set; }

            [BindProperty]
            public IEnumerable<AuthenticationScheme> ExternalLoginProviders { get; set; }

        public async Task OnGet()
            {
                this.ExternalLoginProviders =
                await signInManager.GetExternalAuthenticationSchemesAsync();
            }

            public async Task<IActionResult> OnPostAsync()
            {
                if (!ModelState.IsValid) return Page();

                var result = await signInManager.PasswordSignInAsync(
                    this.Credential.Email,
                    this.Credential.Password,
                    this.Credential.RememberMe,
                    false);

                if (result.Succeeded)
                {
                    return RedirectToPage("/Index");
                }
                else
                {
                    if (result.RequiresTwoFactor)
                    {
                        return RedirectToPage("/Account/LoginTwoFactorWithAuthenticator", new
                        {
                            RememberMe = this.Credential.RememberMe
                        });
                    }

                    if (result.IsLockedOut)
                    {
                        ModelState.AddModelError("Login", "You are locked out.");
                    }
                    else
                    {
                        ModelState.AddModelError("Login", "Failed to login.");
                    }

                    return Page();
                }
            }

            public IActionResult onPostLoginExternally(string provider)
            {
                var properties =
                signInManager.ConfigureExternalAuthenticationProperties(provider, null);

                properties.RedirectUri = Url.Action("ExternalLoginCallback", "Account");

                return Challenge(properties);
            }
    }

        public class CredentialViewModel
        {
            [Required]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remeber Me")]
            public bool RememberMe { get; set; }
        }

    }

    
