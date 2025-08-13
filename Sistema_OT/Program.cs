var builder = WebApplication.CreateBuilder(args);

// Agregar servicios necesarios para manejar las sesiones
builder.Services.AddDistributedMemoryCache(); // Usar memoria para almacenar sesiones
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Tiempo de expiración de la sesión
    options.Cookie.HttpOnly = true; // Aumenta la seguridad de las cookies
    options.Cookie.IsEssential = true; // Necesario para las cookies funcionales
});

// Agregar servicios para autenticación con cookies
builder.Services.AddAuthentication("CookieAuth")
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Redirige al login si no está autenticado
        options.AccessDeniedPath = "/Account/AccessDenied"; // Página de acceso denegado
    });

// Agregar servicios para MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Configurar el pipeline de HTTP
app.UseSession(); // Habilitar sesiones
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Habilitar la autenticación y autorización
app.UseAuthentication(); // Para manejar la autenticación
app.UseAuthorization();  // Para manejar la autorización

// Rutas
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}"); // Redirige al login por defecto


app.Run();

