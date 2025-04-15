using System.ComponentModel.DataAnnotations;

namespace ASP.NET_Project.Models.Category
{
    public class CategoryEditViewModel
    {
        [Display(Name = "Виберіть назву категорії")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Опис")]
        public string? Description { get; set; } = string.Empty;

        [Display(Name = "Url адреса зображення")]
        public string ImageUrl { get; set; } = string.Empty;
    }
}
