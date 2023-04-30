using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Web_App.Data.Account;

namespace Web_App.Pages.Account.Logout
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<User> signInManager;
        public LogoutModel(SignInManager<User> signInManager)
        {
            this.signInManager = signInManager;
        }

        public async Task<IActionResult> OnPostAsync()
        {
          await signInManager.SignOutAsync();

            return RedirectToPage("/Account/Login");
        }
    }
}
