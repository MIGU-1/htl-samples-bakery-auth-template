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

                if(!userManager.Users.Any(u => u.UserName == "admin@itag.at"))
                {
                    await roleManager.CreateAsync(new IdentityRole<int>("Admin"));
                    var admin = new Customer { CustomerNr = "007" ,Firstname = "Admin", Lastname = "Admin", UserName = "admin@itag.at", Email = "Admin@itag.at" };
                    await userManager.CreateAsync(admin, "Admin#123");
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
