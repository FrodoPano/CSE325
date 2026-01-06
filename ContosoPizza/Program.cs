using ContosoPizza.FileOperations;
using ContosoPizza.Models;
using ContosoPizza.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
    // Add sales report functionality in development
    app.AddSalesReportFunctionality();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Add a console interface for file operations
Console.WriteLine("=========================================");
Console.WriteLine("Sales Report Generator");
Console.WriteLine("=========================================");
Console.WriteLine("Options:");
Console.WriteLine("1. Generate Sales Report (Manual)");
Console.WriteLine("2. View Sales Data Directory");
Console.WriteLine("3. Exit Console Mode");
Console.WriteLine("=========================================");

bool continueRunning = true;
while (continueRunning)
{
    Console.Write("\nEnter option (1-3): ");
    var input = Console.ReadLine();
    
    switch (input)
    {
        case "1":
            GenerateManualReport();
            break;
        case "2":
            ShowSalesDirectory();
            break;
        case "3":
            continueRunning = false;
            Console.WriteLine("Exiting console mode. Web API is running...");
            break;
        default:
            Console.WriteLine("Invalid option. Please try again.");
            break;
    }
}

app.Run();

// Local functions
void GenerateManualReport()
{
    try
    {
        var salesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "SalesData");
        var outputPath = Path.Combine(Directory.GetCurrentDirectory(), "Reports", $"sales_summary_manual_{DateTime.Now:yyyyMMdd_HHmmss}.txt");
        
        if (!Directory.Exists(salesDirectory))
        {
            Console.WriteLine("Sales data directory not found. Creating sample data...");
            Directory.CreateDirectory(salesDirectory);
            
            // Create some sample sales files
            var rnd = new Random();
            for (int i = 1; i <= 5; i++)
            {
                var amount = rnd.Next(10000, 50000) + rnd.NextDouble();
                File.WriteAllText(Path.Combine(salesDirectory, $"manual_sales_{i}.txt"), amount.ToString("F2"));
            }
        }
        
        // Ensure Reports directory exists
        var reportsDirectory = Path.GetDirectoryName(outputPath);
        if (!Directory.Exists(reportsDirectory))
        {
            Directory.CreateDirectory(reportsDirectory!);
        }
        
        Console.WriteLine($"Generating report from: {salesDirectory}");
        SalesReportGenerator.GenerateSalesSummaryReport(salesDirectory, outputPath);
        
        if (File.Exists(outputPath))
        {
            var reportContent = File.ReadAllText(outputPath);
            Console.WriteLine("\n" + reportContent);
            Console.WriteLine($"\nReport saved to: {outputPath}");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}

void ShowSalesDirectory()
{
    var salesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "SalesData");
    
    if (Directory.Exists(salesDirectory))
    {
        Console.WriteLine($"\nSales Directory: {salesDirectory}");
        Console.WriteLine(new string('-', 50));
        
        var files = Directory.GetFiles(salesDirectory, "*.txt");
        if (files.Any())
        {
            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file);
                var content = File.ReadAllText(file);
                Console.WriteLine($"  {fileName}: ${content}");
            }
        }
        else
        {
            Console.WriteLine("No sales files found.");
        }
    }
    else
    {
        Console.WriteLine("Sales directory does not exist.");
    }
}