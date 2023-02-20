using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using webapp_cloudrun.Models.MtoGetJsons;

namespace webapp_cloudrun.Models
{
    public class MyFavMovies
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? userId { get; set; }
        public int? MovieId { get; set; }
    }
}
