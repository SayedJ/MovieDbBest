using Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
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

        private readonly MovieDbContext _context;

        public Action<ClaimsPrincipal> OnAuthStateChanged { get; set; }

        public MovieRepoImpl(MovieDbContext context)
        {
            _context = context;
  
           
        }

        public MovieRepoImpl()
        {
        }

 

        public async Task<IEnumerable<Movie>> GetAllMovies()
        {
            return _context.Movies;
        }


        public async Task<IEnumerable<MovieDetailsVM>> GetAllMoviesWithDetails()
        {
            var movies = (await GetAllMovies()).AsQueryable();
            var movieDetails = await GetFullInfo(movies.ToList());
            movieDetails = movieDetails.Where(c => !c.Image_Url.Equals("https://image.tmdb.org/t/p/original"));
            return movieDetails;
        }

        public async Task<Director> GetDirectorById(MovieDbContext context, int? id)
        {
            return await context.Directors.FirstOrDefaultAsync(c => c.MovieId == id) ?? new Director();
        }
    

        public async Task<Movie> GetMovieById(long? id)
        {
            return await _context.Movies.Where(c => c.Id == id).FirstOrDefaultAsync();
        }



        public async Task<Person?> GetPersonById(MovieDbContext context, int? id)
        {
            return await context.People.FirstOrDefaultAsync(c => c.Id == id) ?? new Person();
        }

        public async Task<Rating> GetRatingById(MovieDbContext context,int? id)
        {
            return await context.Ratings.FirstOrDefaultAsync(c => c.MovieId == id) ?? new Rating();
        }

       
        public async Task<IEnumerable<Star>> GetStarById(MovieDbContext context, int? id)
        {
            return await context.Stars.Where(c => c.MovieId == id).ToListAsync() ?? new List<Star>();
        }

        public async Task<string> SaveImageUrl(string url, Movie movie)
        {
            var context = new MovieDbContext();
            var urlPath = new ImageUrl();
            

            if (string.IsNullOrEmpty(url))
            {
                urlPath= await GetImageUrlPath(context, movie);
                if (urlPath == null)
                {
                    urlPath = new ImageUrl()
                    {
                        MovieId = movie.Id,
                        Url = "This is not the right path"
                    };
                    await _context.Imageurl.AddAsync(urlPath);
                    await _context.SaveChangesAsync();
                    return urlPath.Url;
                }
            }
            else
            {
                urlPath = new ImageUrl()
                {
                    MovieId = movie.Id,
                    Url = url
                };
                await _context.Imageurl.AddAsync(urlPath);
                await _context.SaveChangesAsync();
                return urlPath.Url;
            }
            return urlPath.Url;
        }


        public async Task<IEnumerable<MovieDetailsVM>> GetFullInfo(List<Movie> AllMovies)
        {

            string defaultImagePath = "wallpapers";
            string? imageUrl = "";
            Person? directorInfo;
            var tasks = new List<Task<MovieDetailsVM>>();
            foreach (var movie in AllMovies)
            {
                tasks.Add(Task.Run(async () =>
                {
                    using (var context = new MovieDbContext())
                    {
                        var stars = await GetStarById(context, movie.Id );
                        var rating = await GetRatingById(context, movie.Id );
                        var director = await GetDirectorById(context, movie.Id);
                        directorInfo = await GetPersonById(context, director.PersonId);
                        var imageUrlPath = await GetImageUrlPath(context, movie);
                        imageUrl = imageUrlPath.Url;
                        if (!imageUrl.Contains(defaultImagePath))
                        {
                            imageUrl = "https://image.tmdb.org/t/p/original" + imageUrl;
                        }
                        var people = new List<Person>();
                        foreach (var star in stars)
                        {
                            var starsNames = await GetPersonById(context, star.PersonId);
                            people.Add(starsNames);
                        }
                        return new MovieDetailsVM() { Director = directorInfo, Stars = people, Movie = movie, Ratings = rating, Image_Url = imageUrl };
                    }
                }));
            }
            return await Task.WhenAll(tasks);
        }




        [HttpGet]
        public async Task<ImageUrl> GetImageUrlPath(MovieDbContext context, Movie movie)
        {
            var _httpClient = new HttpClient();
            string finalImagePath = "https://assets.wallpapersin4k.org/uploads/2017/04/Film-Roll-Wallpaper-17.jpg";
            ImageUrl? imageurl = new ImageUrl();
            var responseMsg = await _httpClient.GetAsync($"https://api.themoviedb.org/3/search/movie?api_key=c068c27751633a9ac879823f703291c6&query={movie.Title}");
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

        public async Task<string> AddFavMovie(int userId, int id)
        {
            if (await _context.FavMovies.AnyAsync(c => c.MovieId == id && c.userId == userId))
            {
                return "Already added.";
            }

            await _context.FavMovies.AddAsync(new MyFavMovies
            {
                MovieId = id,
                userId = userId
            });
            await _context.SaveChangesAsync();

            return "OK";
        }

        public List<Claim> GenerateClaims(User user) => new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim("DisplayName", user.Name),
            new Claim("Email", user.Email)
        }.ToList();
      
        public async Task<User> Login(UserLogin userLogin)
        {
            try
            {
                User? user = await ValidateUser(userLogin.Username, userLogin.Password);
                if (user == null)
                {
                    throw new Exception("wrong email or password");
                }

                return user;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public async Task SaveUser(User user)
        {
           
            _context.FilmUser.Add(user);
            await _context.SaveChangesAsync();
        }

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

       

        public async Task<IEnumerable<MyFavMovies>> GetAllFavMovies(int userId)
        {
            var models = await _context.FavMovies.Where(c => c.userId == userId).ToListAsync();
            if(models.Any())
            {
                return models;
            }
            return models ?? throw new Exception("somethng is wrong here");
        }


        public async Task<IEnumerable<MovieDetailsVM>> GetFavoriteMovieDetails(int userId)
        {
            var myFavMov = await GetAllFavMovies(userId);
            List<Movie> movies = new();
            foreach (var item in myFavMov)
            {
                var movie = await GetMovieById(item.MovieId);
                if (movie != null) movies.Add(movie);
            }

            return await GetFullInfo(movies);
        }

       
        public async Task RemoveFromFav(int userId, int movieId)
        {
            var movieToRemove = await _context.FavMovies.FirstOrDefaultAsync(c => c.userId == userId && c.MovieId == movieId);
            _context.FavMovies.Remove(movieToRemove);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<MovieDetailsVM>> IfExistAny(List<MovieDetailsVM> myFavs, List<MovieDetailsVM> allMovies) =>
            allMovies.Except(myFavs, Comparer<MovieDetailsVM>.Create(a => a.Movie.Id));

        public async Task<bool> IfAny(int userId, int movieId) =>
            await _context.FavMovies.AnyAsync(c => c.userId == userId && c.MovieId == movieId);


        public async Task<IEnumerable<UserFavMoviesInfo>> GetAllUsersFavMovies()
        {
            var favMoviesWithInfo = new List<UserFavMoviesInfo>();
            var movieWithDetails = new List<MovieDetailsVM>();
            UserFavMoviesInfo favMov;

            var usersFavorites = await _context.FavMovies.ToListAsync();
            var userIds = usersFavorites.Select(f => f.userId).Distinct();

            foreach (var userId in userIds)
            {
                var user = await _context.FilmUser.FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                {
                    continue;
                }

                var movies = await GetFavoriteMovieDetails((int)userId);
                favMov = new UserFavMoviesInfo();
                favMov.Username = user.Username;
                favMov.Movies = movies.ToList();
                favMov.Count = favMov.Movies.Count;
                favMoviesWithInfo.Add(favMov);
            }

            return favMoviesWithInfo;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _context.FilmUser.ToListAsync();
        }

    }
   


}

