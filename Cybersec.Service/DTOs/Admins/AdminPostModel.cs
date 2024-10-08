﻿using Microsoft.AspNetCore.Http;

namespace Cybersec.Service.DTOs.Admins;
public class AdminPostModel
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public IFormFile ImageUrl { get; set; }
}
