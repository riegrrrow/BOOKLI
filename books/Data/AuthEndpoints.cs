using books.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace books.Data;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        // ВХОД В АККАУНТ
        app.MapPost("/auth/login", async (
            [FromForm] string email,
            [FromForm] string password,
            [FromForm] string? returnUrl,
            SignInManager<ApplicationUser> signInManager) =>
        {
            var result = await signInManager.PasswordSignInAsync(
                email,
                password,
                isPersistent: true,
                lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return Results.Redirect(
                    !string.IsNullOrEmpty(returnUrl) &&
                    returnUrl.StartsWith("/") &&
                    !returnUrl.StartsWith("//")
                        ? returnUrl
                        : "/");
            }

            return Results.Redirect("/login?error=1");
        });

        // ВЫХОД ИЗ АККАУНТА
        app.MapPost("/auth/logout", async (
            SignInManager<ApplicationUser> signInManager) =>
        {
            await signInManager.SignOutAsync();

            return Results.Redirect("/");
        });
    }
}