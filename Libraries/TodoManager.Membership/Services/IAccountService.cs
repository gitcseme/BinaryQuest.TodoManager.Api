using TodoManager.Membership.AuthModels;
using TodoManager.Shared.Entities;

namespace TodoManager.Membership.Services
{
    public interface IAccountService
    {
        Task<ApplicationUser> SignupAsync(SignupModel model);
        Task<ApplicationUser> SigninAsync(SigninModel model);
    }
}