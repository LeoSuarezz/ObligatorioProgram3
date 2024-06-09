using Microsoft.EntityFrameworkCore;
using ObligatorioProgram3.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<ObligatorioProgram3Context>(options =>
//ANGEL
//options.UseSqlServer("Data Source=ANGELMACHADO;Initial Catalog=ObligatorioProgram3;Integrated Security=true; TrustServerCertificate=True"));

//FRAN
//options.UseSqlServer("Data Source=DESKTOP-LTBG5HI;Initial Catalog=ObligatorioProgram3;Integrated Security=true; TrustServerCertificate=True"));

//LEO
//options.UseSqlServer("Data Source=LAPTOP-83A9Q1R9;Initial Catalog=ObligatorioProgram3;Integrated Security=true; TrustServerCertificate=True"));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
