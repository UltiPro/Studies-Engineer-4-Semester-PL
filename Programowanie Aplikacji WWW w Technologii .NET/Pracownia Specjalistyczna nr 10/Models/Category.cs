#pragma warning disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models.Category;

public class Category
{
    public int id { get; set; }
    public string shortName { get; set; }
    public string longName { get; set; }
}

