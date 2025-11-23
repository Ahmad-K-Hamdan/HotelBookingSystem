using HotelBookingSystem.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace HotelBookingSystem.Infrastructure.Identity.Seeders;

public static class IdentitySeeder
{
    public static async Task SeedAsync(
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        var roles = new[] { "Manager", "User" };

        foreach (var roleName in roles)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole { Name = roleName });
            }
        }

        // Add Manager
        var managerEmail = "manager@booking.com";
        var manager = await userManager.FindByEmailAsync(managerEmail);
        if (manager == null)
        {
            manager = new User
            {
                Email = managerEmail,
                UserName = managerEmail,
                FirstName = "Hotel",
                LastName = "Manager",
                BirthDate = new DateOnly(1973, 12, 4),
                EmailConfirmed = true
            };

            await userManager.CreateAsync(manager, "Manager123!");
            await userManager.AddToRoleAsync(manager, "Manager");
        }
    }
}
