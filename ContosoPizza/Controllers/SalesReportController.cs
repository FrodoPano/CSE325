using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace ContosoPizza.FileOperations;

public class SalesReportGenerator
{
    public static void GenerateSalesSummaryReport(string salesDirectory, string outputFilePath)
    {
        try
        {
            Console.WriteLine($"Reading sales files from: {salesDirectory}");
            
            // Get all sales files (assuming they are text files with sales amounts)
            var salesFiles = Directory.GetFiles(salesDirectory, "*.txt")
                                     .Where(f => !f.EndsWith("summary.txt", StringComparison.OrdinalIgnoreCase))
                                     .ToList();
            
            if (!salesFiles.Any())
            {
                Console.WriteLine("No sales files found in the directory.");
                return;
            }

            decimal totalSales = 0;
            var salesDetails = new List<SalesDetail>();
            
            // Read each sales file
            foreach (var filePath in salesFiles)
            {
                try
                {
                    var fileName = Path.GetFileName(filePath);
                    var fileContent = File.ReadAllText(filePath);
                    
                    if (decimal.TryParse(fileContent.Trim(), out decimal salesAmount))
                    {
                        totalSales += salesAmount;
                        salesDetails.Add(new SalesDetail 
                        { 
                            FileName = fileName, 
                            Amount = salesAmount 
                        });
                        
                        Console.WriteLine($"Processed: {fileName} - ${salesAmount:N2}");
                    }
                    else
                    {
                        Console.WriteLine($"Warning: Could not parse sales amount from {fileName}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading file {filePath}: {ex.Message}");
                }
            }

            // Generate the report
            GenerateReportFile(outputFilePath, totalSales, salesDetails);
            
            Console.WriteLine($"\nSales summary report generated: {outputFilePath}");
            Console.WriteLine($"Total sales processed: ${totalSales:N2}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error generating sales report: {ex.Message}");
        }
    }

    private static void GenerateReportFile(string outputFilePath, decimal totalSales, List<SalesDetail> salesDetails)
    {
        using (var writer = new StreamWriter(outputFilePath))
        {
            writer.WriteLine("Sales Summary");
            writer.WriteLine(new string('-', 30));
            writer.WriteLine($" Total Sales: ${totalSales:N2}");
            writer.WriteLine();
            writer.WriteLine(" Details:");
            
            foreach (var detail in salesDetails)
            {
                writer.WriteLine($"  {detail.FileName}: ${detail.Amount:N2}");
            }
            
            writer.WriteLine();
            writer.WriteLine($"Report generated: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
        }
    }
    
    private class SalesDetail
    {
        public string FileName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }
}