// I, Noah Tomkins, student number 000790079, certify that this material is my
// original work. No other person's work has been used without due
// acknowledgement and I have not made my work available to anyone else.

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SSD_Lab2.Models;
using SSD_Lab2.Models.Identity;

namespace SSD_Lab2.Data
{
    public static class DbInitializer
    {

        public static async Task<int> SeedUsersAndRoles(IServiceProvider serviceProvider)
        {
            var settings = serviceProvider.GetService<IOptions<AppSettings>>()!.Value;

            // create the database if it doesn't exist
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();

            // Check if roles already exist and exit if there are
            if (roleManager.Roles.Count() > 0)
                return 1;

            // Seed roles
            int result = await SeedRoles(roleManager);
            
            if (result != 0)
                return 2;

            // Check if users already exist and exit if there are
            if (userManager.Users.Count() > 0)
                return 3;

            // Seed users
            result = await SeedUsers(userManager, settings);
            if (result != 0)
                return 4;

            result = await SeedTeams(dbContext);
            if (result != 0)
                return 5;

            return 0;
        }

        private static async Task<int> SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            // Create Manager Role
            var result = await roleManager.CreateAsync(new IdentityRole("Manager"));
            if (!result.Succeeded)
                return 1;

            // Create Player Role
            result = await roleManager.CreateAsync(new IdentityRole("Player"));
            if (!result.Succeeded)
                return 2;

            return 0;
        }

        private static async Task<int> SeedUsers(UserManager<ApplicationUser> userManager, AppSettings settings)
        {
            // Create Manager User
            var managerUser = new ApplicationUser {
                UserName = "manager@example.com",
                Email = "manager@example.com",
                FirstName = "Manager",
                LastName = "Example",
                EmailConfirmed = true,
                BirthDate = DateTime.Now
            };

            var result = await userManager.CreateAsync(managerUser, settings.DefaultPassword!);
            if (!result.Succeeded)
                return 1;

            // Assign user to Admin role
            result = await userManager.AddToRoleAsync(managerUser, "Manager");
            if (!result.Succeeded)
                return 2;


            // Create Player User
            var playerUser = new ApplicationUser
            {
                UserName = "player@example.com",
                Email = "player@example.com",
                FirstName = "Player",
                LastName = "Example",
                EmailConfirmed = true,
                BirthDate = DateTime.Now
            };

            result = await userManager.CreateAsync(playerUser, settings.DefaultPassword!);
            if (!result.Succeeded)
                return 3;
                
            // Assign user to Member role
            result = await userManager.AddToRoleAsync(playerUser, "Player");
            if (!result.Succeeded)
                return 4;
            return 0;
        }

        private static async Task<int> SeedTeams(ApplicationDbContext context)
        {
            if (context.Team.Count() > 0)
                return 1;

            var team1 = new Team
            {
                TeamName = "Team 1",
                Email = "team1@example.com",
                EstablishedDate = DateTime.Now
            };

            var team2 = new Team
            {
                TeamName = "Team 2",
                Email = "team2@example.com",
                EstablishedDate = DateTime.Now
            };

            context.Add(team1);
            context.Add(team2);

            await context.SaveChangesAsync();

            return 0;
        }

    }
}
