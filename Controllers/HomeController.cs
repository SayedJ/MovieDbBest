using System.Diagnostics;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webapp_cloudrun.Models;
using webapp_cloudrun.Models.MtoGetJsons;
using webapp_cloudrun.Repositories;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace webapp_cloudrun.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IMoviesRepo _repo;
    private readonly IHttpContextAccessor _contextAccessor;

    public HomeController(ILogger<HomeController> logger, IMoviesRepo repo, IHttpContextAccessor contextAccessor)
    {
        _logger = logger;
        _repo = repo;
        _contextAccessor = contextAccessor; 
    }
    [Authorize()]
    public async Task<IActionResult> Index()
    {
        var hundredMovies = await _repo.GetAllMoviesWithDetails();

        var movies = await _repo.GetAllMoviesWithDetails();

        ViewBag.User = _contextAccessor.HttpContext.User.Identity.Name;


        return View(hundredMovies);
    }
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Login(UserLogin login)
    {
     
            User loggedIn = await _repo.Login(login);
            if (loggedIn != null)
            {

                var claims = new List<Claim>{
                 new Claim(ClaimTypes.NameIdentifier, loggedIn.Username),
                new Claim(ClaimTypes.Name, loggedIn.Username),
                new Claim(ClaimTypes.Role, loggedIn.Role),
                new Claim("DisplayName", loggedIn.Name),
                new Claim("Email", loggedIn.Email),
            };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {

                    RedirectUri = "/Home/Index",
                    
                };
          

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);
            return Redirect(authProperties.RedirectUri);
            }

            return Redirect("/Accout/Error");
        
        
    }

    public IActionResult Registration()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Registration(User user)
    {
        user.Role = "user";
        try
        {
            if (user != null)
            {
                await _repo.SaveUser(user);

                ViewBag.success = "Sign Up successfully.";
               
            }
         

        }
        catch (Exception ex)
        {
            
            ViewBag.error =  $"!!There is some error.{ex.Message}";
        }
        return View();
    }

    public async Task<IActionResult> AddToFav(int id)
    {
        throw new Exception();

    }

    public IActionResult Privacy()
    {
        return View();
    }


    public string GetDataFromSession(User user)
    {
        return _contextAccessor.HttpContext.Session.GetString(user.Username);
    }


}


  
