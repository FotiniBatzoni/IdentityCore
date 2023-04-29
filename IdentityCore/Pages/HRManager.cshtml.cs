using IdentityCore.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityCore.Pages
{
    [Authorize(Policy = "HRManagerOnly")]
    public class HRManagerModel : PageModel
    {
        private readonly IHttpClientFactory httpClientFactory;

        [BindProperty]
        public List<WeatherForecastDTO> WeatherForecastItems { get; set; }

        public HRManagerModel(IHttpClientFactory _httpClientFactory)
        {
           httpClientFactory = _httpClientFactory;
        }

        public async Task OnGet()
        {
            var httpClient = httpClientFactory.CreateClient("OurWebAPI");
            WeatherForecastItems = await httpClient.
                GetFromJsonAsync<List<WeatherForecastDTO>>("WeatherForecast");
        }
    }
}
