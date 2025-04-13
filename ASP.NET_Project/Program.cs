using ASP.NET_Project.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AspNetProjectDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

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

await app.SeedData(); // Викликаємо метод, який буде заповнювати базу даних даними. Це асинхронний метод, тому чекаємо його завершення

app.Run(); // Запускає наш хост (Сервер) і ми бачимо консоль

// Тут код не компілюється, тому що це не клас, а просто файл, який компілюється в проекті. Код тут писати не можна