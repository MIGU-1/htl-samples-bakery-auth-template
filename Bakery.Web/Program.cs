using Bakery.Core.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Bakery.Web
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetService<UserManager<Customer>>();
                var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole<int>>>();

                if (!userManager.Users.Any(u => u.UserName == "admin@htl.at"))
                {
                    await roleManager.CreateAsync(new IdentityRole<int>("Admin"));

                    var admin = new Customer
                    {
                        Firstname = "Admin",
                        Lastname = "User",
                        UserName = "admin@htl.at",
                        Email = "admin@htl.at",
                        CustomerNr = new Random().Next(1000).ToString()
                    };
                    await userManager.CreateAsync(admin, "Admin12345!");
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}
