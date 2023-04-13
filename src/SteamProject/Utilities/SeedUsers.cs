using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SteamProject.Data;
using SteamProject.Models;

namespace SteamProject.Utilities
{
    public class SeedUsers
    {
        /// <summary>
        /// Initialize seed data for users.  Creates users for login using Identity and also application users.  One password
        /// is used for all accounts.
        /// </summary>
        /// <param name="serviceProvider">The fully configured service provider for this app that can provide a UserManager and the applications dbcontext</param>
        /// <param name="seedData">Array of seed data holding all the attributes needed to create the user objects</param>
        /// <param name="testUserPw">Password for all seed accounts</param>
        /// <returns></returns>
        public static async Task Initialize(IServiceProvider serviceProvider, UserInfoData[] seedData, string testUserPw)
        {
            try
            {
                // Get our application db context
                //   For later reference -- this uses the "Service Locator anti-pattern", not usually a good pattern
                //   but unavoidable here
                using (var context = new SteamInfoDbContext(serviceProvider.GetRequiredService<DbContextOptions<SteamInfoDbContext>>()))
                {
                    // Get the Identity user manager
                    var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

                    foreach (var u in seedData)
                    {
                        // Ensure this user exists or is newly created (Email is used for username since that is the default in Register and Login -- change those and then use username here if you want it different than email
                        var identityID = await EnsureUser(userManager, testUserPw, u.Email, u.Email, u.EmailConfirmed);
                        // Create a new FujiUser if this one doesn't already exist
                        User fu = new SteamProject.Models.User { AspNetUserId = identityID, };
                        if (!context.Users.Any(x => x.AspNetUserId == fu.AspNetUserId))
                        {
                            // Doesn't already exist, so add a new user
                            context.Add(fu);
                            await context.SaveChangesAsync();
                        }
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                // Thrown if there is no service of the type requested from the service provider
                // Catch it (and don't throw the exception below) if you don't want it to fail (5xx status code)
                throw new Exception("Failed to initialize user seed data, service provider did not have the correct service");
            }
        }


        /// <summary>
        /// Helper method to ensure that the Identity user exists or has been newly created.  Modified from
        /// <a href="https://docs.microsoft.com/en-us/aspnet/core/security/authorization/secure-data?view=aspnetcore-5.0#create-the-test-accounts-and-update-the-contacts">create the test accounts and update the contacts (in Contoso University example)</a>
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="password"></param>
        /// <param name="username"></param>
        /// <param name="email"></param>
        /// <param name="emailConfirmed"></param>
        /// <returns>The Identity ID of the user</returns>
        private static async Task<string> EnsureUser(UserManager<IdentityUser> userManager, string password, string username, string email, bool emailConfirmed)
        {
            var user = await userManager.FindByNameAsync(username);
            if (user == null)
            {
                user = new IdentityUser
                {
                    UserName = username,
                    Email = email,
                    EmailConfirmed = emailConfirmed
                };
                await userManager.CreateAsync(user, password);
            }

            if (user == null)
            {
                throw new Exception("The password is probably not strong enough!");
            }

            return user.Id;
        }
    }
}
