using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(
    options =>
    {
        options.IOTimeout = TimeSpan.FromMinutes(30);
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
    }
    );

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Login"; // Ruta de la p�gina de login
        options.AccessDeniedPath = "/AccessDenied"; // Ruta de la p�gina de acceso denegado
        options.ExpireTimeSpan = TimeSpan.FromDays(1); // Tiempo de vida de la cookie (1 d�a)
        options.SlidingExpiration = true; // Actualizar la duraci�n de la cookie con cada request
        options.Cookie.HttpOnly = true; // Mejorar la seguridad evitando el acceso desde el lado del cliente
    });

builder.Services.AddAuthorization(); // Habilitar la autorizaci�n




//inyeccion de dependencias
builder.Services.AddDbContext<Proyecto.ConexionDB>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Cadena"))
);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var ContextoDB = scope.ServiceProvider.GetRequiredService<Proyecto.ConexionDB>();
    ContextoDB.Database.Migrate();
}


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

app.UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Login}/{id?}");

app.Run();
