using System.Data;
using System.Data.Odbc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Reporting.NETCore;

namespace ChurchManager.Api.Controllers.v1;

[ApiVersion("1.0")]
[AllowAnonymous]
public class ReportsController: BaseApiController
{
    [HttpGet()]
    public async Task<IActionResult> GetSampleReport( CancellationToken token)
    {
        string reportPath = Path.Combine(Directory.GetCurrentDirectory(), $"TestReport.rdl");
        if (!System.IO.File.Exists(reportPath))
        {
            return NotFound($"Report '{reportPath}' not found.");
        }
// Create the LocalReport instance
        LocalReport report = new LocalReport
        {
            ReportPath = reportPath
        };        
        //report.LoadReportDefinition(reportPath);
        try
        {
            var dataSource = new ReportDataSource("DataSet1", GetDataFromPostgres());
            report.DataSources.Add(dataSource);
            //report.SetParameters(new[] { new ReportParameter("Parameter1", "Parameter value") });
            byte[] pdf = report.Render("EXCELOPENXML");
            return File(pdf, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"TestReport.xlsx");
            
            // Render the report to HTML
            /*string mimeType, encoding, fileNameExtension;
            Warning[] warnings;
            string[] streams;

            var reportBytes = report.Render(
                "HTML5", // HTML5 for modern HTML rendering
                null,    // No device info needed
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings
            );

            // Convert byte array to a string (HTML content)
            string htmlContent = Encoding.UTF8.GetString(reportBytes);

            return Ok(new { htmlContent });*/
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
      
    }
    
    private DataTable GetDataFromPostgres()
    {
        // ODBC connection string for PostgreSQL
        string connectionString = "Driver={PostgreSQL UNICODE};Server=localhost;Port=5432;Database=churchmanager_db;Schema=public;Uid=postgres;Pwd=ABCD;";

        // SQL query
        string query = "SELECT * FROM \"Person\" LIMIT 10"; // Replace with your query

        // Create and populate the DataTable
        try
        {
            using (OdbcConnection connection = new OdbcConnection(connectionString))
            using (OdbcCommand command = new OdbcCommand(query, connection))
            using (OdbcDataAdapter adapter = new OdbcDataAdapter(command))
            {
                DataTable dataTable = new DataTable();
                connection.Open();
                adapter.Fill(dataTable);
          
                // Log data for debugging
                Console.WriteLine("Data Retrieved:");
                /*foreach (DataRow row in dataTable.Rows)
                {
                    foreach (var item in row.ItemArray)
                    {
                        Console.Write(item + "\t");
                    }
                    Console.WriteLine();
                }*/

            
                return dataTable;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}