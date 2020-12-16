using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MvcClient.Models;
using MvcClient.Models.Actors;
using MvcClient.Models.Movies;

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
            var response = await _client.GetAsync("https://localhost:5001/api/MoviesServer");
            var content = await response.Content.ReadAsStreamAsync();
            var movies = await JsonSerializer.DeserializeAsync<IEnumerable<ListMovie>>(content, _jsonSerializerOptions);
            ViewBag.Movies = movies;
            return View();
        }

        // GET: /Movie/1
        public async Task<IActionResult> Movie(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var response = await _client.GetAsync($"https://localhost:5001/api/MoviesServer/{id}");
            var content = await response.Content.ReadAsStreamAsync();
            var movie = await JsonSerializer.DeserializeAsync<DetailedMovie>(content, _jsonSerializerOptions);
            ViewBag.Movie = movie;
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Movie(string mark)
        {
            //TODO implement
            return RedirectToAction(nameof(Movie));
        }

        // GET: Actor/1
        public async Task<IActionResult> Actor(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var response = await _client.GetAsync($"https://localhost:5001/api/Actors/{id}");
            var content = await response.Content.ReadAsStreamAsync();
            var actor = await JsonSerializer.DeserializeAsync<DetailedActor>(content, _jsonSerializerOptions);
            ViewBag.Actor = actor;
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Actor(string mark)
        {
            throw new NotImplementedException();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}