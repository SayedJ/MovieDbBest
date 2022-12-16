using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace webapp_cloudrun.Models
{
    public partial class ImageUrl
    {
        [Key]
        public int ImageId { get; set; }
        public int? MovieId { get; set; }
        public string? Url { get; set; }
    }
}
