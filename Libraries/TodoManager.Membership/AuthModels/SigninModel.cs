namespace TodoManager.Membership.AuthModels;

public class SigninModel
{
    public string Email { get; set; }
    public string Password { get; set; }
    public bool RememberMe { get; set; }
}
