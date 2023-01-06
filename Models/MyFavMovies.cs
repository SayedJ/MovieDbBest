using System.ComponentModel.DataAnnotations;
using System.Reflection;
using webapp_cloudrun.Models.MtoGetJsons;

namespace webapp_cloudrun.Models
{
    public class MyFavMovies
    {
        [Key]
        public int Id { get; set; }
        public int? userId { get; set; }
        public int? MovieId { get; set; }
    }
}
