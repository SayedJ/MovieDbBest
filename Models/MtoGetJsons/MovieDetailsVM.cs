namespace webapp_cloudrun.Models.MtoGetJsons
{
    public class MovieDetailsVM
    {
      public Movie? Movie { get; set; }
      public IEnumerable<Person?>? Stars { get; set; }
      public Rating? Ratings { get; set; }
      public Person? Director { get; set; }
      public string? Image_Url { get; set; }



    }
}
