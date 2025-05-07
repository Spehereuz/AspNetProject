using ASP.NET_Project.Constants;
using ASP.NET_Project.Data.Entities;
using ASP.NET_Project.Data.Entities.Identity;
using ASP.NET_Project.Models.Seeder;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace ASP.NET_Project.Data
{
    public static class DbSeeder
    {
        public static async Task SeedData(this WebApplication webApplication)
        {
            using var scope = webApplication.Services.CreateScope();

            // Цей об'єкт буде вертати посилання на контекст бази даних, який зареєстрований в Program.cs
            var context = scope.ServiceProvider.GetRequiredService<AspNetProjectDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserEntity>>();
            var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();

            context.Database.Migrate(); // Виконуємо міграцію бази даних, якщо вона ще не була виконана

            if (!context.Categories.Any())
            {
                var jsonFile = Path.Combine(Directory.GetCurrentDirectory(), "Helpers", "JsonData", "Categories.json");
                if (File.Exists(jsonFile))
                {
                    var jsonData = await File.ReadAllTextAsync(jsonFile);
                    try
                    {
                        var categories = JsonSerializer.Deserialize<List<SeederCategoryModel>>(jsonData);
                        var categoryEntities = mapper.Map<List<CategoryEntity>>(categories);
                        await context.AddRangeAsync(categoryEntities);
                        await context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error reading JSON file: {ex.Message}");
                        return;
                    }
                }
                else
                {
                    Console.WriteLine($"File \"Categories.json\" not found: {jsonFile}");
                }
            }

            if (!context.Roles.Any())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<RoleEntity>>();
                var admin = new RoleEntity { Name = Roles.Admin };
                var result = await roleManager.CreateAsync(admin);
                if (result.Succeeded)
                {
                    Console.WriteLine($"Роль {Roles.Admin} створено успішно");
                }
                else
                {
                    Console.WriteLine($"Помилка створення ролі:");
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"- {error.Code}: {error.Description}");
                    }
                }

                var user = new RoleEntity { Name = Roles.User };
                result = await roleManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    Console.WriteLine($"Роль {Roles.User} створено успішно");
                }
                else
                {
                    Console.WriteLine($"Помилка створення ролі:");
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"- {error.Code}: {error.Description}");
                    }
                }
            }

            if (!context.Users.Any())
            {
                string email = "admin@gmail.com";
                var user = new UserEntity
                {
                    UserName = email,
                    Email = email,
                    LastName = "Марко",
                    FirstName = "Онутрій"
                };

                var result = await userManager.CreateAsync(user, "123456");

                if (result.Succeeded)
                {
                    Console.WriteLine($"Користувача {user.LastName} {user.FirstName} створено успішно");
                    var roleResult = await userManager.AddToRoleAsync(user, Roles.Admin);
                    if (roleResult.Succeeded)
                    {
                        Console.WriteLine($"Користувачу {email} призначено роль {Roles.Admin}");
                    }
                    else
                    {
                        Console.WriteLine($"Помилка призначення ролі:");
                        foreach (var error in roleResult.Errors)
                        {
                            Console.WriteLine($"- {error.Code}: {error.Description}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"Помилка створення користувача:");
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"- {error.Code}: {error.Description}");
                    }
                }
            }
        }
    }
}
