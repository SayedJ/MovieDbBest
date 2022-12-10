using Microsoft.EntityFrameworkCore;

namespace webapp_cloudrun.Data
{
    public class MovieContext : DbContext
    {
        public MovieContext(DbContextOptions<MovieContext> options) : base(options)
        {
            
        }

    }
}
