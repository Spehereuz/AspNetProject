using ASP.NET_Project.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

namespace ASP.NET_Project.Services
{
    public class ImageService(IConfiguration configuration) : IImageService
    {
        public async Task<string> SaveImageAsync(IFormFile file)
        {
            using MemoryStream ms = new(); // Створюємо новий потік пам'яті
            await file.CopyToAsync(ms); // Копіюємо файл в пам'ять
            var bytes = ms.ToArray(); // Отримуємо масив байтів з пам'яті

            var image = await SaveImageAsync(bytes); // Зберігаємо зображення в пам'яті
            return image; // Повертаємо назву зображення
        }

        private async Task<string> SaveImageAsync(byte[] bytes)
        {
            string imageName = $"{Path.GetRandomFileName()}.webp"; // Генеруємо випадкову назву для зображення
            var sizes = configuration.GetRequiredSection("ImageSizes").Get<List<int>>(); // Отримуємо розміри зображень з конфігурації

            // Створюємо асинхронні задачі для збереження зображень в різних розмірах
            Task[] tasks = sizes
                .AsParallel()
                .Select(s => SaveImageAsync(bytes, imageName, s))
                .ToArray();

            await Task.WhenAll(tasks); // Чекаємо завершення всіх задач

            return imageName; // Повертаємо назву зображення
        }

        private async Task SaveImageAsync(byte[] bytes, string name, int size)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), configuration["ImagesDir"]!, $"{size}_{name}");
            using var image = Image.Load(bytes);

            // Застосовуємо обробку зображення
            image.Mutate(async imgConext =>
            {
                imgConext.Resize(new ResizeOptions
                {
                    Size = new Size(size, size),
                    Mode = ResizeMode.Max
                });
                await image.SaveAsync(path, new WebpEncoder()); // Зберігаємо зображення в папку
            });
        }
    }
}
