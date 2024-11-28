var builder = WebApplication.CreateBuilder(args);


// Agregar servicios necesarios para manejar las sesiones
builder.Services.AddDistributedMemoryCache(); // Usar memoria para almacenar sesiones
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Tiempo de expiración de la sesión
    options.Cookie.HttpOnly = true; // Aumenta la seguridad de las cookies
    options.Cookie.IsEssential = true; // Necesario para las cookies funcionales
});


// Add services to the container.
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
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}"); // Redirige al login por defecto

app.Run();
