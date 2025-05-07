using ASP.NET_Project.Data;
using ASP.NET_Project.Data.Entities.Identity;
using ASP.NET_Project.Interfaces;
using ASP.NET_Project.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AspNetProjectDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Додаємо налаштування для UserManager, RoleManager, SignInManager - займається cookies
builder.Services.AddIdentity<UserEntity, RoleEntity>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
})
    .AddEntityFrameworkStores<AspNetProjectDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); // Додаємо AutoMapper для перетворення моделей. Це потрібно для того, щоб перетворювати моделі з одного формату в інший. Наприклад, з CategoryEntity в CategoryItemViewModel

builder.Services.AddScoped<IImageService, ImageService>(); // Додаємо сервіс для роботи з зображеннями. Це потрібно для того, щоб зберігати зображення в базі даних

// У нас будуть View - це такі сторінки, де можна писати на C# Index.cshtml
// ASP.NET_Project - вихідний файл проекту
// контролер - це клас на C#, який приймає запити від клієнта і виконує усю логіку роботи
// Результати роботи (Model) контролера передаються у View для відображення
builder.Services.AddControllersWithViews(); // Налаштування контейнерів, сервісів, контролерів та представлень

var app = builder.Build(); // Створюється збірка на основі даних налаштувань вище

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error"); // Якщо не в режимі розробки, то обробляємо помилки, і нас кидає на сторінку /Home/Error
}
app.UseRouting(); // Підтримка маршрутизації - це механізм, який дозволяє ASP.NET Core визначити, як обробляти HTTP-запити на основі URL-адреси запиту та HTTP-методу (GET, POST тощо).

app.UseAuthorization(); // Підтримка авторизації - це механізм, який дозволяє контролювати доступ до ресурсів програми на основі прав користувачів.

app.MapStaticAssets(); // Використання статичних ресурсів - це механізм, який дозволяє ASP.NET Core обслуговувати статичні файли (наприклад, CSS, JavaScript, зображення) безпосередньо з файлової системи. У нас буде працювати папка wwwroot

// Налаштування для маршрутів. У нас є контролери - Вони мають називатия HomeController, ProductsController, OrdersController
// При цьому враховується лише HomeController, а інші контролери не враховуються. Методи цього класу називаються Action - це дії, які виконуються при запиті до контролера
// Для того, щоб при запуску сайту ми бачили, що щось визивається згідно налаштувань HomeController
// і його метод Index при цьому може бути параметр у маршруті id - але там є знак питання, тому він не обов'язковий
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Categories}/{action=Index}/{id?}")
    .WithStaticAssets();

var dir = builder.Configuration["ImagesDir"]; // Отримуємо директорію, в якій будуть зберігатися зображення. Це потрібно для того, щоб зберігати туди зображення, які ми будемо завантажувати на сайт
string path = Path.Combine(Directory.GetCurrentDirectory(), dir); // Отримуємо поточну директорію, в якій запущено додаток, і з'єднуємо її з директорією, в якій будуть зберігатися зображення.
Directory.CreateDirectory(path); // Створюємо директорію, якщо її немає. Це потрібно для того, щоб зберігати туди зображення, які ми будемо завантажувати на сайт

// Налаштування для статичних файлів
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(path), // Вказуємо, що ми будемо використовувати фізичну директорію, в якій будуть зберігатися зображення
    RequestPath = $"/{dir}" // Вказуємо, що ми будемо використовувати директорію, в якій будуть зберігатися зображення
});

await app.SeedData(); // Викликаємо метод, який буде заповнювати базу даних даними. Це асинхронний метод, тому чекаємо його завершення

app.Run(); // Запускає наш хост (Сервер) і ми бачимо консоль

// Тут код не компілюється, тому що це не клас, а просто файл, який компілюється в проекті. Код тут писати не можна