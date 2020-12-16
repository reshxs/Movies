using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MvcClient.Models;
using MvcClient.Models.Actors;
using MvcClient.Models.Auth;
using MvcClient.Models.Movies;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace MvcClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _client = new HttpClient();
        private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // GET
        public async Task<IActionResult> Index()
        {
            ViewBag.Movies = await GetMovies();
            return View();
        }


        public async Task<IActionResult> ByRating()
        {
            ViewBag.Movies = await GetMovies("OrderByRating");
            return View("Index");
        }
        
        public async Task<IActionResult> ByTitle()
        {
            ViewBag.Movies = await GetMovies("OrderByTitle");
            return View("Index");
        }
        
        public async Task<IActionResult> ByDate()
        {
            ViewBag.Movies = await GetMovies("OrderByDate");
            return View("Index");
        }
        
        private async Task<IEnumerable<ListMovie>> GetMovies(string order=null)
        {
            var response = await _client.GetAsync($"https://localhost:5001/api/Movies/{order}");
            var content = await response.Content.ReadAsStreamAsync();
            var movies = await JsonSerializer.DeserializeAsync<IEnumerable<ListMovie>>(content, _jsonSerializerOptions);
            return movies;
        }

        // GET: /Movie/1
        public async Task<IActionResult> Movie(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.Movie = await GetMovieAsync(id);
            ViewBag.Mark = await GetMarkAsync("Movie", id);
            
            return View();
        }

        private async Task<DetailedMovie> GetMovieAsync(int? id)
        {
            var response = await _client.GetAsync($"https://localhost:5001/api/Movies/{id}");
            var content = await response.Content.ReadAsStreamAsync();
            var movie = await JsonSerializer.DeserializeAsync<DetailedMovie>(content, _jsonSerializerOptions);
            return movie;
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Movie(int id, string mark)
        {
            var movie = await GetMovieAsync(id);
            if (CurrentUser.Authorized)
            {
                var token = CurrentUser.Token;
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            
            var markModel = new MovieMark() {MovieId = id, Mark = int.Parse(mark)};
            var requestBody = new StringContent(JsonSerializer.Serialize(markModel), Encoding.Default, "application/json");
            var response = await _client.PostAsync($"https://localhost:5001/api/MovieMarks", requestBody);
            return RedirectToAction(nameof(Movie));
        }

        // GET: Actor/1
        public async Task<IActionResult> Actor(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            ViewBag.Actor = await GetActorAsync(id);
            ViewBag.Mark = await GetMarkAsync("Actor", id);
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Actor(int id, string mark)
        {
            if (CurrentUser.Authorized)
            {
                var token = CurrentUser.Token;
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            
            var markModel = new ActorMark() {ActorId = id, Mark = int.Parse(mark)};
            var requestBody = new StringContent(JsonSerializer.Serialize(markModel), Encoding.Default, "application/json");
            await _client.PostAsync($"https://localhost:5001/api/ActorMarks", requestBody);
            return RedirectToAction(nameof(Actor));
        }
        
        private async Task<DetailedActor> GetActorAsync(int? id)
        {
            var response = await _client.GetAsync($"https://localhost:5001/api/Actors/{id}");
            var content = await response.Content.ReadAsStreamAsync();
            var actor = await JsonSerializer.DeserializeAsync<DetailedActor>(content, _jsonSerializerOptions);
            return actor;
        }

        private async Task<int?> GetMarkAsync(string type, int? id)
        {
            int? result;
            if (CurrentUser.Authorized)
            {
                var token = CurrentUser.Token;
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await _client.GetAsync($"https://localhost:5001/api/{type}Marks/{id}");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsStreamAsync();
                var mark = await JsonSerializer.DeserializeAsync<AbstractMark>(content, _jsonSerializerOptions);
                return mark?.Mark;
            }

            return null;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}