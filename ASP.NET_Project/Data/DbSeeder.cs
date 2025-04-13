using ASP.NET_Project.Data.Entities;
using ASP.NET_Project.Models.Seeder;
using AutoMapper;
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
        }
    }
}
