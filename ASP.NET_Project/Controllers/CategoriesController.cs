using Microsoft.AspNetCore.Mvc;
using ASP.NET_Project.Models.Category;
using ASP.NET_Project.Data;
using AutoMapper;
using ASP.NET_Project.Data.Entities;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Webp;
using ASP.NET_Project.Interfaces;
using Microsoft.AspNetCore.Authorization;
using ASP.NET_Project.Constants;

namespace ASP.NET_Project.Controllers
{
    public class CategoriesController(AspNetProjectDbContext context, 
        IMapper mapper, IImageService imageService) : Controller
    {
        public IActionResult Index() //Це будь-який web результат - View - сторінка, Файл, PDF, Excel
        {
            ViewBag.Title = "Категорії";
            var model = mapper.ProjectTo<CategoryItemViewModel>(context.Categories).ToList();
            return View(model);
        }

        [HttpGet] // Атрибут, який вказує, що метод відповідає на GET запит
        public IActionResult Create() // Метод, який відповідає за створення нової категорії
        {
            return View(); // Повертаємо View з назвою Create
        }
        
        [HttpPost] // Атрибут, який вказує, що метод відповідає на Post запит
        public async Task<IActionResult> Create(CategoryCreateViewModel model) // Метод, який відповідає за створення нової категорії
        {
            // Перевіряємо, чи існує категорія з такою назвою
            var entity = await context.Categories.SingleOrDefaultAsync(x => x.Name == model.Name);
            if (entity != null) // Якщо категорія існує
            {
                ModelState.AddModelError("Name", "Категорія з такою назвою вже існує"); // Додаємо помилку в модель
                return View(model); // Повертаємо View з назвою Create
            }

            entity = mapper.Map<CategoryEntity>(model); // Перетворюємо модель в CategoryEntity
            entity.ImageUrl = await imageService.SaveImageAsync(model.ImageFile); // Присвоюємо URL зображення категорії
            await context.Categories.AddAsync(entity); // Додаємо нову категорію в базу даних
            await context.SaveChangesAsync(); // Зберігаємо зміни в базі даних
            return RedirectToAction(nameof(Index)); // Якщо все добре, то перенаправляємо на метод Index
        }

        [HttpGet] //Тепер він працює методом GET - це щоб побачити форму
        public async Task<IActionResult> Edit(int id)
        {
            var category = await context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            //Динамічна колекція, яка збергає динамічні дані, які можна вкиористати на View
            ViewBag.ImageName = category.ImageUrl;

            //TempData["ImageUrl"] = category.ImageUrl;

            var model = mapper.Map<CategoryEditViewModel>(category);
            return View(model);
        }

        [HttpPost] //Тепер він працює методом GET - це щоб побачити форму
        public async Task<IActionResult> Edit(CategoryEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var existing = await context.Categories.FirstOrDefaultAsync(x => x.Id == model.Id);
            if (existing == null)
            {
                return NotFound();
            }

            var duplicate = await context.Categories
                .FirstOrDefaultAsync(x => x.Name == model.Name && x.Id != model.Id);
            if (duplicate != null)
            {
                ModelState.AddModelError("Name", "Another category with this name already exists");
                return View(model);
            }

            existing = mapper.Map(model, existing);

            if (model.ImageFile != null)
            {
                await imageService.DeleteImageAsync(existing.ImageUrl);
                existing.ImageUrl = await imageService.SaveImageAsync(model.ImageFile);
            }
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost] // Атрибут, який вказує, що метод відповідає на Post запит
        [Authorize(Roles = $"{Roles.Admin}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await context.Categories.SingleOrDefaultAsync(x => x.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(category.ImageUrl))
            {
                await imageService.DeleteImageAsync(category.ImageUrl);
            }

            context.Categories.Remove(category);
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}