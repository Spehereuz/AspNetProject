namespace ASP.NET_Project.Interfaces
{
    public interface IImageService
    {
        Task<string> SaveImageAsync(IFormFile file); // Метод для збереження зображення
        Task DeleteImageAsync(string name); // Метод для видалення зображення
    }
}
