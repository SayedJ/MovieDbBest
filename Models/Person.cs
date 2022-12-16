using System;
using System.Collections.Generic;

namespace webapp_cloudrun.Models;

public partial class Person
{
    public int? Id { get; set; }

    public string? Name { get; set; }

    public long? Birth { get; set; }
}
