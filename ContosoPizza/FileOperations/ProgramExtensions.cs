using System.IO;

namespace ContosoPizza.FileOperations;

public static class ProgramExtensions
{
    public static void AddSalesReportFunctionality(this WebApplication app)
    {
        // Create a sales directory with sample data
        var salesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "SalesData");
        if (!Directory.Exists(salesDirectory))
        {
            Directory.CreateDirectory(salesDirectory);
            
            // Create sample sales files
            var sampleData = new Dictionary<string, decimal>
            {
                { "sales_2024_01.txt", 12500.75m },
                { "sales_2024_02.txt", 18750.25m },
                { "sales_2024_03.txt", 23400.50m },
                { "sales_2024_04.txt", 15675.80m },
                { "sales_2024_05.txt", 28900.95m }
            };
            
            foreach (var data in sampleData)
            {
                File.WriteAllText(Path.Combine(salesDirectory, data.Key), data.Value.ToString());
            }
            
            Console.WriteLine($"Created sample sales data in: {salesDirectory}");
        }
        
        // Create Reports directory
        var reportsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Reports");
        if (!Directory.Exists(reportsDirectory))
        {
            Directory.CreateDirectory(reportsDirectory);
            Console.WriteLine($"Created reports directory in: {reportsDirectory}");
        }
    }
}