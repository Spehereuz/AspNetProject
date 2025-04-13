using ASP.NET_Project.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AspNetProjectDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

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

await app.SeedData(); // ��������� �����, ���� ���� ����������� ���� ����� ������. �� ����������� �����, ���� ������ ���� ����������

app.Run(); // ������� ��� ���� (������) � �� ������ �������

// ��� ��� �� �����������, ���� �� �� �� ����, � ������ ����, ���� ����������� � ������. ��� ��� ������ �� �����