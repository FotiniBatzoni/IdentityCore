using IdentityCore.Authorization;
using IdentityCore.DTO;
using IdentityCore.Pages.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http.Headers;

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

            var res = await httpClient.PostAsJsonAsync("auth",
                new Credential { UserName = "admin", Password = "Password" });

            res.EnsureSuccessStatusCode();

            string strJwt = await res.Content.ReadAsStringAsync();

            var token = JsonConvert.DeserializeObject<JwtToken>(strJwt);

            httpClient.DefaultRequestHeaders.Authorization = new
                        AuthenticationHeaderValue("Bearer", token.AccessToken);

            WeatherForecastItems = await httpClient.
                GetFromJsonAsync<List<WeatherForecastDTO>>("WeatherForecast");
        }
    }
}
