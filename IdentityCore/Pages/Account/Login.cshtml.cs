using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace IdentityCore.Pages.Account
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public Credential Credential { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();
    
            //Verify the credential
            if(Credential.UserName == "admin" && Credential.Password == "password")
            {
                //Creating the security context
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name , "admin"),
                    new Claim(ClaimTypes.Email , "admin@test.com"),
                    new Claim("Department","HR"),
                    new Claim("Admin", "true"),
                    new Claim("Manager", "true")
                };

                var identity = new ClaimsIdentity(claims, "MyCookieAuth") ;
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

                //serialize the ClaimsPrincipal into a string 
                //and then encrypt that string
                //and save it as a Cookie in th HttpContext 
                await HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal);

                return RedirectToPage("/index");
            }

            return Page();
        
        }
    }

        public class Credential
        {
            [Required]
            [Display(Name = "User Name")]
            public string UserName { get; set; }
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }
    
}
