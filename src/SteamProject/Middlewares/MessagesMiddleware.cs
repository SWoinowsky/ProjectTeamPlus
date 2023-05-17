using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SteamProject.DAL.Abstract;
using SteamProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace SteamProject.Middlewares
{
    public class MessagesMiddleware
    {
        private readonly RequestDelegate _next;

        public MessagesMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IServiceScopeFactory scopeFactory)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var userId = userManager.GetUserId(context.User);
                if (userId is null)
                {
                    Console.WriteLine("user is null");
                }
                else
                {
                    string messengerId = userRepository.GetUser(userId).Id.ToString();
                    context.Items["MessengerId"] = messengerId;
                }
            }
            await _next(context);
        }
    }
}