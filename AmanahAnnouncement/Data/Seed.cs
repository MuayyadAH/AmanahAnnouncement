using AmanahAnnouncement.Models;
using Microsoft.AspNetCore.Identity;

namespace AmanahAnnouncement.Data
{
    public class Seed
    {
        public static void SeedData(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                context.Database.EnsureCreated();

                if (!context.Announcments.Any())
                {
                    context.Announcments.AddRange(new List<Announcment>()
                    {
                        new Announcment()
                        {
                           Title = "First Advertisement",
                           Content = "Descripition 1",
                           CreationDate = DateTime.Now,
                           Attachement = null
                        },
                        new Announcment()
                        {
                           Title = "Second Advertisement",
                           Content = "Descripition 2",
                           CreationDate = DateTime.Now,
                           Attachement = null
                        },
                        new Announcment()
                        {
                           Title = "Third Advertisement",
                           Content = "Descripition 3",
                           CreationDate = DateTime.Now,
                           Attachement = null
                        },
                        new Announcment()
                        {
                           Title = "Fourth Advertisement",
                           Content = "Descripition 4",
                           CreationDate = DateTime.Now,
                           Attachement = null
                        },
                        new Announcment()
                        {
                           Title = "Fifth Advertisement",
                           Content = "Descripition 5",
                           CreationDate = DateTime.Now,
                           Attachement = null
                        },
                    });
                    context.SaveChanges();

                }
            }
        }
        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                //Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync("MediaHead"))
                    await roleManager.CreateAsync(new IdentityRole("MediaHead"));
                if (!await roleManager.RoleExistsAsync("MediaEmployee"))
                    await roleManager.CreateAsync(new IdentityRole("MediaEmployee"));

                //Users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<Users>>();
                string adminUserEmail = "malhazmy@jeddah.gov.sa";

                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                if (adminUser == null)
                {
                    var newAdminUser = new Users()
                    {
                        UserName = adminUserEmail,
                        firstName = "مؤيد",
                        lastName = "الحازمي",
                        Email = adminUserEmail,
                        EmailConfirmed = true,
                        jobTitle = "مدير ادارة المركز الاعلامي"
                    };
                    await userManager.CreateAsync(newAdminUser, "Coding@1234?");
                    await userManager.AddToRoleAsync(newAdminUser, "MediaHead");
                }

                string appUserEmail = "aalhazmy@jeddah.gov.sa";

                var appUser = await userManager.FindByEmailAsync(appUserEmail);
                if (appUser == null)
                {
                    var newAppUser = new Users()
                    {
                        UserName = appUserEmail,
                        firstName = "احمد",
                        lastName = "الحازمي",
                        Email = appUserEmail,
                        EmailConfirmed = true,
                        jobTitle = "موظف المركز الاعلامي"
                    };
                    await userManager.CreateAsync(newAppUser, "Coding@1234?");
                    await userManager.AddToRoleAsync(newAppUser, "MediaEmployee");
                }
            }
        }

    }
}
