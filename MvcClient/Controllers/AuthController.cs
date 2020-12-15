using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MvcClient.Models.Auth;
using System.Text.Json;

namespace MvcClient.Controllers
{
    public class AuthController : Controller
    {
        private readonly HttpClient _client = new HttpClient();
        private readonly ILogger<AuthController> _logger;

        public AuthController(ILogger<AuthController> logger)
        {
            _logger = logger;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(
            [Bind("Username, Password")]LoginModel model)
        {
            var requestBody = JsonSerializer.Serialize(model);
            var response = await _client.PostAsync("https://localhost:5001/api/Auth/login",
                new StringContent(requestBody));
            
            var contentStream = await response.Content.ReadAsStreamAsync();
            var token = JsonSerializer.DeserializeAsync<TokenModel>(contentStream);
            
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Register(
            [Bind("Username, Email, Password")] RegisterModel model)
        {
            var requestModel = JsonSerializer.Serialize(model);
            var response = await _client.PostAsync("https://localhost:5001/api/Auth/register",
                new StringContent(requestModel));
            
            var contentStream = await response.Content.ReadAsStreamAsync();
            //Todo validate response !!!
            return RedirectToAction("Login");
        }
    }
}