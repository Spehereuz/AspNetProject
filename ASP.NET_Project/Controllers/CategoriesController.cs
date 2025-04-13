using Microsoft.AspNetCore.Mvc;
using ASP.NET_Project.Models.Category;
using ASP.NET_Project.Data;
using AutoMapper;

namespace ASP.NET_Project.Controllers
{
    public class CategoriesController(AspNetProjectDbContext context, IMapper mapper) : Controller
    {
        public IActionResult Index() // Будь-який web результат - View, Файл, Json, Redirect, PDF, тощо
        {
            var model = mapper.ProjectTo<CategoryItemViewModel>(context.Categories).ToList();
            return View(model); // Повертаємо View з назвою Index
        }
    }
}
