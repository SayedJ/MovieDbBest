using System;
using System.Collections.Generic;

namespace webapp_cloudrun.Models;

public partial class Movie
{
    public int? Id { get; set; }

    public string? Title { get; set; }

    public long? Year { get; set; }
}
