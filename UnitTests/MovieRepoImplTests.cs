using NUnit.Framework;
using webapp_cloudrun.Context;
using webapp_cloudrun.Models;
using webapp_cloudrun.Repositories;
using webapp_cloudrun.Repositories.Impl;

namespace UnitTests
{
    public class MovieRepoImplTests
    {

        IMoviesRepo movieRepo;
        MovieDbContext dbContext;
        HttpClient httpClient;

       [SetUp]
       public void Setup()
       {
            dbContext = new MovieDbContext();
            httpClient = new HttpClient();
            movieRepo = new MovieRepoImpl(dbContext);
       }

        [Test]
        public async Task GetAllMovies_Works()
        {
            // Arrange
            List<Movie> allMovies = new();
            //Act
            var movies = await movieRepo.GetAllMovies();
            allMovies.AddRange(movies);
            //Assert
            Assert.That(allMovies, Is.Not.Empty);
        }
        [Test]
        public async Task Login_CanSignIN()
        {
             var user = new UserLogin();
             user.Username = "jalilhu";
            user.Password= "jalilhu";
            //act
           var userLoggedIn = await movieRepo.Login(user);
            Assert.That(userLoggedIn, Is.Not.Null);


        }
        [Test]
        public async Task AddFavMovie_CanAddedToList()
        {
            int userId = 1;
            int movieId = 5516026;
            //act
          var result=  await movieRepo.AddFavMovie(userId, movieId);
            Assert.That(result , Is.EqualTo("Already added."));
            Assert.That(result, Is.Not.Null);
            
        }
        [Test]
        public async Task RemoveFromFav_ActuallyRemoves()
        {
            int userId = 1;
            int movieId = 5516026;
           
           
            //act
            await movieRepo.RemoveFromFav(userId, movieId);
            var favMovies = await movieRepo.GetAllFavMovies(userId);
            bool isExist = true;
            foreach(var item in favMovies)
            {
                if(item.MovieId != 5516026)
                {
                    isExist = false;
                }
            }

            //assert
            Assert.IsTrue(isExist);
        }


    }
}