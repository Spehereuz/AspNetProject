﻿using System.ComponentModel.DataAnnotations;

namespace ASP.NET_Project.Models.Category
{
    public class CategoryCreateViewModel
    {
        [Display(Name = "Назва Категорії")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Опис")]
        public string? Description { get; set; } = string.Empty;

        [Display(Name = "Оберіть фото")]
        public IFormFile ImageFile { get; set; } = null!;
    }
}
