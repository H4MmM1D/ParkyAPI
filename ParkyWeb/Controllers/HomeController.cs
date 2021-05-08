using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ParkyWeb.Models;
using ParkyWeb.Models.ViewModels;
using ParkyWeb.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ParkyWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INationalParkRepository _npRepo;
        private readonly ITrailRepository _trailRepository;
        private readonly IAccountRepository _accountRepository;

        public HomeController(ILogger<HomeController> logger, INationalParkRepository npRepo, ITrailRepository trailRepository, IAccountRepository accountRepository)
        {
            _logger = logger;
            _npRepo = npRepo;
            _trailRepository = trailRepository;
            _accountRepository = accountRepository;
        }

        public async Task<IActionResult> Index()
        {
            var indexViewModel = new IndexViewModel() 
            {
                NationalParks = await _npRepo.GetAllAsync(StaticDetail.NationalParkAPIPath, HttpContext.Session.GetString("JWToken")),
                Trails = await _trailRepository.GetAllAsync(StaticDetail.TrailAPIPath, HttpContext.Session.GetString("JWToken"))
            };

            return View(indexViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult Login() 
        {
            var user = new User();
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User model) 
        {
            var user = await _accountRepository.LoginAsync(StaticDetail.AccountAPIPath + "authenticate", model);

            if (user.Token == null) 
            {
                return View();
            }

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Name, user.Username));
            identity.AddClaim(new Claim(ClaimTypes.Role, user.Role));

            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            HttpContext.Session.SetString("JWToken", user.Token);
            TempData["alert"] = "Welcome " + user.Username;
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User model)
        {
            var result = await _accountRepository.RegisterAsync(StaticDetail.AccountAPIPath + "Register", model);

            if (!result)
            {
                return View();
            }

            TempData["alert"] = "Registration was successful";
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> Logout() 
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.SetString("JWToken", "");
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AccessDenied() 
        {
            return View();
        }
    }
}
