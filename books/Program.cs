using books.Components;
using books.Services;
using books.Data;
using books.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Подключение базы данных SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=bookli.db"));

// Настройки регистрации и авторизации
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.User.RequireUniqueEmail = true;

        options.Password.RequiredLength = 6;
        options.Password.RequireDigit = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "BOOKLI.Auth";
    options.ExpireTimeSpan = TimeSpan.FromDays(30);
    options.SlidingExpiration = true;
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Сервисы Blazor и корзины
builder.Services.AddScoped<CartService>();
builder.Services.AddScoped<RegisterService>();
builder.Services.AddScoped<FavoriteService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<BookService>();
builder.Services.AddScoped<ReviewService>();
builder.Services.AddScoped<SiteReviewService>();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var bookService = scope.ServiceProvider
        .GetRequiredService<BookService>();

    var existingBooks = await bookService.GetBooksAsync();

    if (existingBooks.Count == 0)
    {
        var initialBooks = new List<Book>
        {
            new(
                "Три товарища",
                "Эрих Мария Ремарк",
                "Роман",
                535m,
                "purple",
                "📖",
                "Самый красивый в двадцатом столетии роман о любви. Самый увлекательный в двадцатом столетии роман о дружбе. Самый трагический и пронзительный роман о человеческих отношениях за всю историю двадцатого столетия.",
                "/images/three-comrades.jpg"),

            new(
                "Мастер и Маргарита",
                "Михаил Афанасьевич Булгаков",
                "Роман",
                848m,
                "red",
                "🐈",
                "Знаменитый роман о любви, добре и зле, мистике и загадочном визите Воланда в Москву.",
                "/images/master-margarita.jpg"),

            new(
                "Война миров",
                "Герберт Уэллс",
                "Фантастика",
                470m,
                "orange",
                "👽",
                "Один из самых известных романов Герберта Уэллса — о вторжении инопланетян на Землю — и культовая классика мировой научной фантастики.",
                "/images/war-worlds.jpg"),

            new(
                "Причуда мертвеца",
                "Агата Кристи",
                "Детектив",
                355m,
                "green",
                "🔎",
                "В поместье готовят игру «Найди жертву» — но развлечение оборачивается настоящим убийством. Эркюль Пуаро и Ариадна Оливер берутся за расследование, чтобы раскрыть преступление среди гостей и хозяев дома.",
                "/images/end-house.jpg"),

            new(
                "Кэрри",
                "Стивен Кинг",
                "Ужасы",
                742m,
                "dark",
                "👁️",
                "«Кэрри» — первый роман Стивена Кинга, принёсший ему мировую славу. История застенчивой школьницы, обладающей телекинезом, и выпускного бала, который город не забудет никогда.",
                "/images/carrie.jpg"),

            new(
                "Грозовой перевал",
                "Эмили Бронте",
                "Роман",
                1007m,
                "blue",
                "🌹",
                "История трагической любви двух диких сердец — одно из самых популярных и узнаваемых произведений в европейской литературе XIX века.",
                "/images/wuthering-heights.jpg")
        };

        foreach (var book in initialBooks)
        {
            await bookService.AddBookAsync(book);
        }
    }
}

// Создание роли администратора
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

    // Создаём роль Admin, если её ещё нет
    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    }

    // Укажи здесь почту своего зарегистрированного аккаунта
    var adminEmail = "grossstadtgefluster@gmail.com";

    var adminUser = await userManager.FindByEmailAsync(adminEmail);

    if (adminUser != null &&
        !await userManager.IsInRoleAsync(adminUser, "Admin"))
    {
        await userManager.AddToRoleAsync(adminUser, "Admin");
    }
}

// Настройки обработки запросов
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute(
    "/not-found",
    createScopeForStatusCodePages: true);

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapAuthEndpoints();

app.Run();