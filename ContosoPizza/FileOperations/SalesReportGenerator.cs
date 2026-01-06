using Microsoft.AspNetCore.Mvc;

namespace ContosoPizza.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalesReportController : ControllerBase
{
    private readonly IWebHostEnvironment _environment;
    
    public SalesReportController(IWebHostEnvironment environment)
    {
        _environment = environment;
    }
    
    [HttpPost("generate")]
    public IActionResult GenerateSalesReport()
    {
        try
        {
            var salesDirectory = Path.Combine(_environment.ContentRootPath, "SalesData");
            var outputPath = Path.Combine(_environment.ContentRootPath, "Reports", $"sales_summary_{DateTime.Now:yyyyMMdd_HHmmss}.txt");
            
            // Ensure Reports directory exists
            var reportsDirectory = Path.GetDirectoryName(outputPath);
            if (!Directory.Exists(reportsDirectory))
            {
                Directory.CreateDirectory(reportsDirectory!);
            }
            
            // Generate the report
            FileOperations.SalesReportGenerator.GenerateSalesSummaryReport(salesDirectory, outputPath);
            
            // Read and return the report content
            if (System.IO.File.Exists(outputPath))
            {
                var reportContent = System.IO.File.ReadAllText(outputPath);
                return Ok(new 
                { 
                    message = "Sales report generated successfully",
                    reportPath = outputPath,
                    content = reportContent
                });
            }
            
            return NotFound("Report file was not created.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error generating report: {ex.Message}");
        }
    }
    
    [HttpGet]
    public IActionResult GetLatestReport()
    {
        try
        {
            var reportsDirectory = Path.Combine(_environment.ContentRootPath, "Reports");
            
            if (!Directory.Exists(reportsDirectory))
            {
                return NotFound("No reports directory found.");
            }
            
            var reports = Directory.GetFiles(reportsDirectory, "sales_summary_*.txt")
                                  .OrderByDescending(f => f)
                                  .ToList();
            
            if (!reports.Any())
            {
                return NotFound("No sales reports found.");
            }
            
            var latestReport = reports.First();
            var content = System.IO.File.ReadAllText(latestReport);
            
            return Ok(new 
            {
                filename = Path.GetFileName(latestReport),
                content = content
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error retrieving report: {ex.Message}");
        }
    }
    
    [HttpGet("files")]
    public IActionResult GetSalesFiles()
    {
        try
        {
            var salesDirectory = Path.Combine(_environment.ContentRootPath, "SalesData");
            
            if (!Directory.Exists(salesDirectory))
            {
                return NotFound("Sales data directory not found.");
            }
            
            var files = Directory.GetFiles(salesDirectory, "*.txt")
                                .Select(f => new 
                                {
                                    filename = Path.GetFileName(f),
                                    content = System.IO.File.ReadAllText(f),
                                    lastModified = System.IO.File.GetLastWriteTime(f)
                                })
                                .ToList();
            
            return Ok(files);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error reading sales files: {ex.Message}");
        }
    }
}