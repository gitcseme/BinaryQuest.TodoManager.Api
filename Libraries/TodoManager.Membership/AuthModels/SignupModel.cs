﻿using System.ComponentModel.DataAnnotations;

namespace TodoManager.Membership.AuthModels;

public class SignupModel
{

    public string Email { get; set; }

    public string Password { get; set; }
}
