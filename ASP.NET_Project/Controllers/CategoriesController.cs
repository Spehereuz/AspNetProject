using Microsoft.AspNetCore.Mvc;
using ASP.NET_Project.Models.Category;

namespace ASP.NET_Project.Controllers
{
    public class CategoriesController : Controller
    {
        List<CategoryItemViewModel> categories = new List<CategoryItemViewModel>
        {
            new CategoryItemViewModel
            {
                Id = 1,
                Name = "Пригодницькі",
                Description = "Мультфільми, сповнені захопливих пригод та подорожей.",
                Image = "https://tripmydream.cc/travelhub/blog/blog/48/19/block_4819.png"
            },
            new CategoryItemViewModel
            {
                Id = 2,
                Name = "Комедійні",
                Description = "Мультфільми, що піднімуть настрій та розсмішать.",
                Image = "https://focus.ua/static/storage/thumbs/1200x630/e/9b/6b151b9a-6bae2967e0354bfa0a53c6aebfb5c9be.jpg?v=1328_1"
            },
            new CategoryItemViewModel
            {
                Id = 3,
                Name = "Фентезі",
                Description = "Магічні світи та чарівні істоти чекають на вас.",
                Image = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcScenMcTNIGu5yj977bQGEaTcGPaX6MmjfxPQ&s"
            },
            new CategoryItemViewModel
            {
                Id = 4,
                Name = "Наукова фантастика",
                Description = "Подорожі в космос та футуристичні технології.",
                Image = "https://lookintothe.space/images/202503/thumb_20250319_esa_swift_sail.jpg"
            },
            new CategoryItemViewModel
            {
                Id = 5,
                Name = "Класика Disney",
                Description = "Найкращі класичні мультфільми від студії Disney.",
                Image = "https://uakino.me/uploads/dle_collections/og/6.png"
            }
        };

        public IActionResult Index() // Будь-який web результат - View, Файл, Json, Redirect, PDF, тощо
        {
            return View(categories); // Повертаємо View з назвою Index
        }
    }
}
