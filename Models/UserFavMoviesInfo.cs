using webapp_cloudrun.Models.MtoGetJsons;

namespace webapp_cloudrun.Models
{
    public class UserFavMoviesInfo
    {

        public string Username { get;set; }
        
        public List<MovieDetailsVM> Movies { get; set; }
        
        public int Count { get; set; } = 0;


    }
}
