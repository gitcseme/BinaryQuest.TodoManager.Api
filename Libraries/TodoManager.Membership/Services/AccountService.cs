using Microsoft.AspNetCore.Identity;
using System.Text;
using TodoManager.Membership.AuthModels;
using TodoManager.Membership.Entities;

namespace TodoManager.Membership.Services;

public class AccountService : IAccountService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<ApplicationUser> SigninAsync(SigninModel model)
    {
        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
        if (result.Succeeded)
            return await _userManager.FindByEmailAsync(model.Email);

        throw new Exception("Failed email or password is invalid");
    }

    public async Task<ApplicationUser> SignupAsync(SignupModel model)
    {
        var user = new ApplicationUser()
        {
            Email = model.Email,
            UserName = model.Email
        };
        
        var result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
            await _signInManager.SignInAsync(user, isPersistent: true);
        else
        {
            var error = result.Errors.FirstOrDefault()?.Description;
            throw new Exception(error);
        }
        
        return user;
    }
}
