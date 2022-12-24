#pragma warning disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models.User;

public class User
{
    [Display(Name = "Id")]
    public int id { get; set; }
    [Display(Name = "login")]
    public string login { get; set; }
    [Display(Name = "password")]
    public string password { get; set; }
    [Display(Name = "active")]
    public bool active { get; set; }
}

