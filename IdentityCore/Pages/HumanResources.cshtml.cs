using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityCore.Pages
{
    [Authorize(Policy = "MustBelongtoHRDepartment")]
    public class HumanResourcesModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
