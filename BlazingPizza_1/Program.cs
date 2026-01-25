using BlazingPizza;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpClient();
builder.Services.AddScoped<OrderState>();

// Register DbContext - USE ONLY ONE OF THESE:
builder.Services.AddDbContext<PizzaStoreContext>(options => 
    options.UseSqlite("Data Source=pizza.db"));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
app.UseRouting();

app.MapRazorPages();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

// Map API controllers
app.MapControllers();

// Initialize the database
var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using (var scope = scopeFactory.CreateScope())
{
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<PizzaStoreContext>();
        Console.WriteLine("Attempting to create/connect to database...");
        
        if (db.Database.EnsureCreated())
        {
            Console.WriteLine("Database created successfully");
            SeedData.Initialize(db);
            Console.WriteLine("Seed data initialized");
        }
        else
        {
            Console.WriteLine("Database already exists");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Database initialization error: {ex.Message}");
        Console.WriteLine($"Stack trace: {ex.StackTrace}");
        // Re-throw to see the error
        throw;
    }
}


app.MapGet("/test", () => "Server is running!");

app.Run();