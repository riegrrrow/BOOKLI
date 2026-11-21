using books.Data;
using books.Models;
using Microsoft.AspNetCore.Identity;

namespace books.Services;

public class RegisterService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public RegisterService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<(bool Success, string Error)> RegisterAsync(
        RegisterModel model)
    {
        var existingUser = await _userManager.FindByEmailAsync(model.Email);

        if (existingUser != null)
        {
            return (false, "Пользователь с такой почтой уже существует");
        }

        var user = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            FullName = model.FullName
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            return (false, string.Join("; ",
                result.Errors.Select(error => error.Description)));
        }

        return (true, "");
    }
}