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
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Azure;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using MockQueryable.Moq;
using Moq;
using Castle.Core.Internal;

namespace webapp_cloudrun.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IMoviesRepo _repo;
    private readonly IHttpContextAccessor _contextAccessor;
    private int userId;
    public HomeController(ILogger<HomeController> logger, IMoviesRepo repo, IHttpContextAccessor contextAccessor)
    {
        _logger = logger;
        _repo = repo;
        _contextAccessor = contextAccessor;
      
    }
    [Authorize()]
    public async Task<IActionResult> Index(string sortOrder,
    string currentFilter,
    string searchString,
    int? pageNumber)
    {
        ViewData["CurrentSort"] = sortOrder;
        if (searchString != null)
        {
            pageNumber = 1;
        }
        else
        {
            searchString = currentFilter;
        }
        var idOfUser = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        userId = Convert.ToInt32(idOfUser);

        var hundredMovies = await _repo.GetAllMoviesWithDetails();

        ViewBag.User = _contextAccessor.HttpContext.User.Identity.Name;

        ViewBag.UserId = idOfUser;

        var myMovies = await _repo.GetFavoriteMovieDetails(userId);
        ViewBag.Movies = myMovies;
        var notOnMyList = await _repo.IfExistAny(myMovies.ToList(), hundredMovies.ToList());
        hundredMovies = hundredMovies.Where(c => !c.Image_Url.Contains("orginal"));
        var mock = hundredMovies.AsQueryable().BuildMock();
       
        int pageSize = 20;

        return View(await PaginatedList<MovieDetailsVM>.CreateAsync(mock.AsNoTracking(), pageNumber ?? 1, pageSize));
        
      
        
    }

    [HttpPost]
    public async Task<IActionResult> Index(string searchString, int? pageNumber) {

        var movies = await _repo.GetAllMoviesWithDetails();

        var myMovies = await _repo.GetFavoriteMovieDetails(userId);
        ViewBag.Movies = myMovies;
        int pageSize = 10;
        if (!String.IsNullOrEmpty(searchString))
        {
            var filteredMovies = movies.Where(s => s.Movie.Title!.Contains(searchString));
            var mock = filteredMovies.AsQueryable().BuildMock();
           
            return View(await PaginatedList<MovieDetailsVM>.CreateAsync(mock.AsNoTracking(), pageNumber ?? 1, pageSize));
        }
        var mockMovies = movies.AsQueryable().BuildMock();
        return View(await PaginatedList<MovieDetailsVM>.CreateAsync(mockMovies.AsNoTracking(), pageNumber ?? 1, pageSize));
    }

 
    public async Task<IActionResult> MyFavMovies(int? pageNumber)
    {
        var idOfUser = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        userId = Convert.ToInt32(idOfUser);
        var myFavoriteMovies = await  _repo.GetFavoriteMovieDetails(userId);
        ViewBag.Movies = myFavoriteMovies;
        int pageSize = 10;
        var mock = myFavoriteMovies.AsQueryable().BuildMock();
        return View("Index", await PaginatedList<MovieDetailsVM>.CreateAsync(mock.AsNoTracking(), pageNumber ?? 1, pageSize));
    } 


    public async Task<IActionResult> NotOnMyList(int? pageNumber)
    {
        var idOfUser = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        userId = Convert.ToInt32(idOfUser);
        var hundredMovies = await _repo.GetAllMoviesWithDetails();
        var myMovies = await _repo.GetFavoriteMovieDetails(userId);
        ViewBag.Movies = myMovies;
        var notOnMyList = await _repo.IfExistAny(myMovies.ToList(), hundredMovies.ToList());
        var mock = notOnMyList.AsQueryable().BuildMock();
        int pageSize = 10;
        return View("Index",await PaginatedList<MovieDetailsVM>.CreateAsync(mock.AsNoTracking(), pageNumber ?? 1, pageSize));
    }

    public async Task<IActionResult> AllUsersFavorites()
    {
        var usersFav = await _repo.GetAllUsersFavMovies();

        ViewBag.Users = usersFav;
        return View(usersFav);
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
              
                new Claim(ClaimTypes.NameIdentifier, loggedIn.Id.ToString()),
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
    [HttpPost]
    public async  Task<IActionResult> AddToFav(int movieId)
    {
        string respond;
        //string host = _contextAccessor.HttpContext.Request.Host.Value;
        //string path = _contextAccessor.HttpContext.Request.Path.Value;

        var userId = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId != null)
        {
            var userIdToInt = Convert.ToInt32(userId);
            respond = await _repo.AddFavMovie(userIdToInt, movieId);
       
           
            TempData["Success"] = respond;
            return RedirectToAction("Index");
        }

        return View("Index", "Home");

    }

    [HttpPost]
    public async Task<IActionResult> RemoveFromFav(int movieId)
    {
        //string host = _contextAccessor.HttpContext.Request.Host.Value;
        //string path = _contextAccessor.HttpContext.Request.Path.Value;

        var userId = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId != null)
        {
            var userIdToInt = Convert.ToInt32(userId);
           await _repo.RemoveFromFav(userIdToInt, movieId);
            return RedirectToAction(nameof(Index));
        }

        return RedirectToAction(nameof(Index));

    }

    public IActionResult Privacy()
    {
        return View();
    }


    public string GetDataFromSession(User user)
    {
        return _contextAccessor.HttpContext.Session.GetString(user.Username);
    }
    public void IfExistAny(List<MovieDetailsVM> myFavs, List<MovieDetailsVM> allMovies)
    {
       

    }
    private Task<bool> IsExist(int id)
    {
        var isThere = _repo.IfAny(userId, id);
        return isThere;
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _contextAccessor.HttpContext.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}


  
