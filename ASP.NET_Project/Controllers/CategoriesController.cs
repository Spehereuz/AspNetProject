using Microsoft.AspNetCore.Mvc;
using ASP.NET_Project.Models.Category;
using ASP.NET_Project.Data;
using AutoMapper;
using ASP.NET_Project.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_Project.Controllers
{
    public class CategoriesController(AspNetProjectDbContext context, IMapper mapper) : Controller
    {
        public IActionResult Index() // Будь-який web результат - View, Файл, Json, Redirect, PDF, тощо
        {
            // Отримуємо список категорій з бази даних і перетворюємо його в список CategoryItemViewModel
            var model = mapper.ProjectTo<CategoryItemViewModel>(context.Categories).ToList();
            
            return View(model); // Повертаємо View з назвою Index
        }

        [HttpGet] // Атрибут, який вказує, що метод відповідає на GET запит
        public IActionResult Create() // Метод, який відповідає за створення нової категорії
        {
            return View(); // Повертаємо View з назвою Create
        }
        
        [HttpPost] // Атрибут, який вказує, що метод відповідає на Post запит
        public async Task<IActionResult> Create(CategoryCreateViewModel model) // Метод, який відповідає за створення нової категорії
        {
            var item = await context.Categories.SingleOrDefaultAsync(x => x.Name == model.Name); // Перевіряємо, чи існує категорія з такою назвою
            if (item != null) // Якщо категорія існує
            {
                ModelState.AddModelError("Name", "Категорія з такою назвою вже існує"); // Додаємо помилку в модель
                return View(model); // Повертаємо View з назвою Create
            }
            
            item = mapper.Map<CategoryEntity>(model); // Перетворюємо модель в CategoryEntity
            await context.Categories.AddAsync(item); // Додаємо нову категорію в базу даних
            await context.SaveChangesAsync(); // Зберігаємо зміни в базі даних
            return RedirectToAction(nameof(Index)); // Якщо все добре, то перенаправляємо на метод Index
        }

        [HttpGet] // Атрибут, який вказує, що метод відповідає на GET запит
        public IActionResult Edit()
        {
            // Отримуємо список категорій з бази даних
            var categories = context.Categories
                .Select(c => c.Name)
                .ToList();

            ViewBag.Categories = categories; // Передаємо список категорій в ViewBag
            return View(); // Повертаємо View з назвою Edit
        }

        [HttpPost] // Атрибут, який вказує, що метод відповідає на Post запит
        public async Task<IActionResult> Edit(CategoryEditViewModel model)
        {
            // Перевіряємо чи така категорія існує в базі
            var category = await context.Categories.SingleOrDefaultAsync(c => c.Name == model.Name);

            if (category == null)
            {
                ModelState.AddModelError("Name", "Категорія з такою назвою не знайдена.");
                ViewBag.Categories = await context.Categories.Select(c => c.Name).ToListAsync(); // Повторно передаємо список категорій для View у випадку помилки
                return View(model); // Повертаємо View з назвою Edit
            }

            // Якщо значення Description та ImageUrl однакові — не оновлюємо.
            bool isUpdated = false;

            if (category.Description != model.Description)
            {
                category.Description = model.Description; // Оновлюємо опис категорії
                isUpdated = true;
            }

            if (category.ImageUrl != model.ImageUrl)
            {
                category.ImageUrl = model.ImageUrl; // Оновлюємо URL зображення категорії
                isUpdated = true;
            }

            if (isUpdated)
            {
                await context.SaveChangesAsync(); // Зберігаємо зміни в базі даних
            }
            return RedirectToAction(nameof(Index)); // Після збереження — перенаправляємо на список категорій
        }

        [HttpGet] // Атрибут, який вказує, що метод відповідає на GET запит
        public IActionResult Delete()
        {
            var categories = context.Categories.Select(c => c.Name).ToList(); // Отримуємо список категорій з бази даних
            ViewBag.Categories = categories; // Передаємо список категорій в ViewBag
            return View(); // Повертаємо View з назвою Delete
        }
    }
}
