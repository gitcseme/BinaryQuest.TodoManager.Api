﻿using System.ComponentModel.DataAnnotations;

namespace TodoManager.Membership.AuthModels;

public class SignupModel
{
    [Required, EmailAddress, MaxLength(255, ErrorMessage = "Maximum length is 255 characters")]
    public string Email { get; set; }

    [Required, MinLength(4, ErrorMessage = "Length must be greater than 4 characters")]
    public string Password { get; set; }
    public string UserName { get; set; }
}
