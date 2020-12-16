using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MvcClient.Models.Auth;
using System.Text.Json;
using MvcClient.Views.Auth;

namespace MvcClient.Controllers
{
    public class AuthController : Controller
    {
        private readonly HttpClient _client = new HttpClient();
        private readonly ILogger<AuthController> _logger;
        private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };


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
            var requestBody = new StringContent(JsonSerializer.Serialize(model), Encoding.Default, "application/json");
            var response = await _client.PostAsync("https://localhost:5001/api/Auth/login",
                requestBody);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var contentStream = await response.Content.ReadAsStreamAsync();
                var token = await JsonSerializer.DeserializeAsync<TokenModel>(contentStream, _jsonSerializerOptions);
                CurrentUser.Login(token.Id, model.Username, token.Token);
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction(nameof(Login));
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Register(
            [Bind("Username, Email, Password, PasswordRepeat")] RegisterModel model)
        {
            if (model.Password == model.PasswordRepeat)
            {
                var requestModel = JsonSerializer.Serialize(model);
                var response = await _client.PostAsync("https://localhost:5001/api/Auth/register",
                    new StringContent(requestModel));
                if (response.StatusCode == HttpStatusCode.Created)
                {
                    return RedirectToAction(nameof(Login));
                }
            }
            
            return RedirectToAction(nameof(Register));
        }

        public IActionResult Logout()
        {
            CurrentUser.Logout();
            return RedirectToAction("Index", "Home");
        }
    }
}