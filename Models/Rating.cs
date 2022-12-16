using System;
using System.Collections.Generic;

namespace webapp_cloudrun.Models;

public partial class Rating
{
    public int? MovieId { get; set; }

    public float? Rating1 { get; set; }

    public int? Votes { get; set; }
}
