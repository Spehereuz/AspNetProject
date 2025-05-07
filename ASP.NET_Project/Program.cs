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

// ������ ������������ ��� UserManager, RoleManager, SignInManager - ��������� cookies
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

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); // ������ AutoMapper ��� ������������ �������. �� ������� ��� ����, ��� ������������� ����� � ������ ������� � �����. ���������, � CategoryEntity � CategoryItemViewModel

builder.Services.AddScoped<IImageService, ImageService>(); // ������ ����� ��� ������ � ������������. �� ������� ��� ����, ��� �������� ���������� � ��� �����

// � ��� ������ View - �� ��� �������, �� ����� ������ �� C# Index.cshtml
// ASP.NET_Project - �������� ���� �������
// ��������� - �� ���� �� C#, ���� ������ ������ �� �볺��� � ������ ��� ����� ������
// ���������� ������ (Model) ���������� ����������� � View ��� �����������
builder.Services.AddControllersWithViews(); // ������������ ����������, ������, ���������� �� ������������

var app = builder.Build(); // ����������� ����� �� ����� ����� ����������� ����

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error"); // ���� �� � ����� ��������, �� ���������� �������, � ��� ���� �� ������� /Home/Error
}
app.UseRouting(); // ϳ������� ������������� - �� �������, ���� �������� ASP.NET Core ���������, �� ��������� HTTP-������ �� ����� URL-������ ������ �� HTTP-������ (GET, POST ����).

app.UseAuthorization(); // ϳ������� ����������� - �� �������, ���� �������� ������������ ������ �� ������� �������� �� ����� ���� ������������.

app.MapStaticAssets(); // ������������ ��������� ������� - �� �������, ���� �������� ASP.NET Core ������������� ������� ����� (���������, CSS, JavaScript, ����������) ������������� � ������� �������. � ��� ���� ��������� ����� wwwroot

// ������������ ��� ��������. � ��� � ���������� - ���� ����� ��������� HomeController, ProductsController, OrdersController
// ��� ����� ����������� ���� HomeController, � ���� ���������� �� ������������. ������ ����� ����� ����������� Action - �� 䳿, �� ����������� ��� ����� �� ����������
// ��� ����, ��� ��� ������� ����� �� ������, �� ���� ���������� ����� ����������� HomeController
// � ���� ����� Index ��� ����� ���� ���� �������� � ������� id - ��� ��� � ���� �������, ���� �� �� ����'�������
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Categories}/{action=Index}/{id?}")
    .WithStaticAssets();

var dir = builder.Configuration["ImagesDir"]; // �������� ���������, � ��� ������ ���������� ����������. �� ������� ��� ����, ��� �������� ���� ����������, �� �� ������ ������������� �� ����
string path = Path.Combine(Directory.GetCurrentDirectory(), dir); // �������� ������� ���������, � ��� �������� �������, � �'������ �� � ���������, � ��� ������ ���������� ����������.
Directory.CreateDirectory(path); // ��������� ���������, ���� �� ����. �� ������� ��� ����, ��� �������� ���� ����������, �� �� ������ ������������� �� ����

// ������������ ��� ��������� �����
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(path), // �������, �� �� ������ ��������������� ������� ���������, � ��� ������ ���������� ����������
    RequestPath = $"/{dir}" // �������, �� �� ������ ��������������� ���������, � ��� ������ ���������� ����������
});

await app.SeedData(); // ��������� �����, ���� ���� ����������� ���� ����� ������. �� ����������� �����, ���� ������ ���� ����������

app.Run(); // ������� ��� ���� (������) � �� ������ �������

// ��� ��� �� �����������, ���� �� �� �� ����, � ������ ����, ���� ����������� � ������. ��� ��� ������ �� �����