using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using webapp_cloudrun.Auth;
using webapp_cloudrun.Context;
using webapp_cloudrun.Models;
using webapp_cloudrun.Models.MtoGetJsons;
using static System.Net.Mime.MediaTypeNames;

namespace webapp_cloudrun.Repositories.Impl
{
    public class MovieRepoImpl : IMoviesRepo
    {

        private readonly HttpClient _httpClient;
        private readonly MovieDbContext _context;
        private readonly IAuthService _authService;

        public Action<ClaimsPrincipal> OnAuthStateChanged { get; set; }

        public MovieRepoImpl(MovieDbContext context, HttpClient httpClient, IAuthService authService)
        {
            _context = context;
            authService = _authService;
            _httpClient = httpClient;
           
        }

        public async Task<IEnumerable<Movie>> GetAllMovies()
        {
            var movies = await _context.Movies.Where(s => s.Year > 2000).OrderByDescending(j => j.Year).Take(100).ToListAsync();
            return movies;
        }

        public async Task<IEnumerable<MovieDetailsVM>> GetAllMoviesWithDetails()
        {
            return await GetFullInfo();
            
        }

        public async Task<Director> GetDirectorById(int? id)
        {
            var director = await _context.Directors.Where(c => c.MovieId == id).FirstOrDefaultAsync();
            if (director == null)
            {
                director = new Director() { MovieId = id, PersonId = 00 };
                return director;
            }
            return director;
        }

    
        public async Task<Movie> GetMovieById(long id)
        {

            var secondMovie = await _context.Movies.Where(c => c.Id == id).FirstOrDefaultAsync();
            return secondMovie;

        }

      

        public async Task<Person?> GetPersonById(int? id)
        {
            Person? person = await _context.People.Where(c => c.Id == id).FirstOrDefaultAsync();
            if (person == null)
            {
                person = new Person() { Id = id, Name = "Not available" };
                return person;
            }
            return person;
        }

        public async Task<Rating> GetRatingById(int? id)
        {

            Rating? rating = await _context.Ratings.Where(c => c.MovieId == id).FirstOrDefaultAsync();
            if(rating == null)
            {
                rating = new Rating() { MovieId = id, Rating1 = 00, Votes = 00 };
                return rating;
            }
            return rating;
        }

       
        public async Task<IEnumerable<Star>> GetStarById(int? id)
        {
            var stars = await _context.Stars.Where(c => c.MovieId == id).ToListAsync();
            if(stars == null)
            {
                stars = new List<Star>()
                {
                    new Star(){ MovieId = id, PersonId = 00}
                };
                return stars;
            }
            return stars;
        }

        public async Task<string> SaveImageUrl(string url, Movie movie)
        {
            var urlPath = new ImageUrl();

            if (string.IsNullOrEmpty(url))
            {

                urlPath = await GetImageUrl(movie);
                if (urlPath == null)
                {
                    urlPath.MovieId = movie.Id;
                    urlPath.Url = "This is not the right path";
                    return urlPath.Url;

                }
            }
                await _context.Imageurl.AddAsync(urlPath);
                await _context.SaveChangesAsync();
                return urlPath.Url;

            


        }
        public async Task<string> GetImageUrlPath(Movie movie)
        {
            string imageUrl = "";
            var imagePath = await _context.Imageurl.Where(c => c.MovieId == movie.Id).FirstOrDefaultAsync();
            if (imagePath == null)
            {
                imagePath = new ImageUrl();
                imageUrl = await SaveImageUrl(imageUrl, movie); 
                imagePath.MovieId = movie.Id;
                imagePath.Url = imageUrl;
                return imagePath.Url;
            }

            return imagePath.Url;

        }
        private async Task<IEnumerable<MovieDetailsVM>> GetFullInfo()
        {
            string defaultImagePath = "wallpapers";
            string? imageUrl = "";
            Person? directorInfo;
            MovieDetailsVM? movieInfo = new MovieDetailsVM();
            var ListOfmovieInfo = new List<MovieDetailsVM>();
            var movies = await GetAllMovies();
            foreach(var movie in movies)
            {
                IEnumerable<Star?>? stars = await GetStarById(movie.Id);
                Rating? rating = await GetRatingById(movie.Id);
                Director? director =await  GetDirectorById(movie.Id);
                directorInfo =await  GetPersonById(director.PersonId);
                imageUrl = await GetImageUrlPath(movie);
                if(!imageUrl.Contains(defaultImagePath))
                {
                    imageUrl = "https://image.tmdb.org/t/p/original" + imageUrl;
                }
                List<Person?>? people = new List<Person>();
                foreach(var star in stars)
                {
                    var starsNames = await GetPersonById(star.PersonId);
                    people.Add(starsNames);
                }

                movieInfo = new MovieDetailsVM() { Director = directorInfo, Stars = people, Movie = movie, Ratings = rating, Image_Url = imageUrl };
                 ListOfmovieInfo.Add(movieInfo);
            }

            return  ListOfmovieInfo;

        }


       

        [HttpGet]
        public async Task<ImageUrl> GetImageUrl(Movie movie)
        {
            string finalImagePath = "https://assets.wallpapersin4k.org/uploads/2017/04/Film-Roll-Wallpaper-17.jpg";
            ImageUrl? imageurl = new ImageUrl();
            var responseMsg = await _httpClient.GetAsync($"/3/search/movie?api_key=c068c27751633a9ac879823f703291c6&query={movie.Title}");
            imageurl.MovieId = movie.Id;
            var content = responseMsg.Content.ReadAsStringAsync().Result; ;
            if (!responseMsg.IsSuccessStatusCode)
            {
                throw new Exception("This is " + content);
            }
            content = content.Replace("null", $"\"{finalImagePath}\"");
            //content = content.Replace("backdrop_path:\")
            JObject? data = (JObject)JsonConvert.DeserializeObject(content);
            if (data.HasValues && data["results"].HasValues)
            {
                imageurl.Url = data["results"][0]["poster_path"].Value<string>();
            }
            else
            {
                imageurl.Url = finalImagePath;
            }

            return imageurl;
        }

        public async Task AddFavMovie(int userId, int id)
        {
            MyFavMovies favMov = new MyFavMovies();
            favMov.MovieId = id;
            favMov.userId = userId;
           await _context.FavMovies.AddAsync(favMov);
            await _context.SaveChangesAsync();
        }


        public  List<Claim> GenerateClaims(User user)
        {
            var claims = new[]
            {
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.Role, user.Role),
        new Claim("DisplayName", user.Name),
        new Claim("Email", user.Email)
    };
            return claims.ToList();
        }

        public async Task<User> Login(UserLogin userLogin)
        {
            try
            {
             
             User? user = await ValidateUser(userLogin.Username, userLogin.Password);
                if(user == null)
                {
                    throw new Exception("wrong email or password");

                }
                
                //ClaimsPrincipal principal = CreateClaimPrincipal(user);

                return user;
            }

            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public async Task SaveUser(User user)
        {
           
           await  _context.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        //public static ClaimsPrincipal CreateClaimPrincipal(User user)
        //{

        //    var claims = new List<Claim>
        //        {
        //        new Claim(ClaimTypes.NameIdentifier, user.Username),
        //        new Claim(ClaimTypes.Name, user.Username),
        //        new Claim(ClaimTypes.Role, user.Role),
        //        new Claim("DisplayName", user.Name),
        //        new Claim("Email", user.Email),
        //        };


        //    var claimsIdentity = new ClaimsIdentity(
        //        claims, CookieAuthenticationDefaults.AuthenticationScheme);

        //    return claimsIdentity;
        //}

        private static ClaimsPrincipal CreateClaimsPrincipal()
        {
            
                return new ClaimsPrincipal();
        
        }
        public async Task<ClaimsPrincipal> GetAuthAsync()
        {
            ClaimsPrincipal principal = CreateClaimsPrincipal();
            return principal;
        }


        public void LogoutAsync()
        { 
            ClaimsPrincipal principal = new();
            OnAuthStateChanged.Invoke(principal);
         
        }

        public async Task<User> ValidateUser(string username, string password)
        {
            User? existingUser = await _context.FilmUser.Where(u =>
                u.Username == username).FirstOrDefaultAsync();

            if (existingUser == null)
            {
                throw new Exception("User not found");
            }

            if (!existingUser.Password.Equals(password))
            {
                throw new Exception("Password mismatch");
            }

            return existingUser;
        }

    }
    //    public Movie GetMovieByTitle(string title)
    //    {
    //        var secondMovie = _context.Movies.Where(c => c.Title.Equals(title)).FirstOrDefault();
    //        return secondMovie;

    //    }
    //}

    //internal class AppDbContextMeClient
    //{
    //    private DbContextOptionsBuilder optionsBuilder;

    //    public AppDbContextMeClient(DbContextOptionsBuilder optionsBuilder)
    //    {
    //        this.optionsBuilder = optionsBuilder;
    //    }

    //    public DbSet<Movie> MoviesEntities { get;  set; }
    //}
}

