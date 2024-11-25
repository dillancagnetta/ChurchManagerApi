using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChurchManager.Api.Controllers.v1;

[ApiVersion("1.0")]
[AllowAnonymous]
public class ReportsController: BaseApiController
{
    [HttpGet()]
    public async Task<IActionResult> GetSampleReport( CancellationToken token)
    {
        /*LocalReport report = new LocalReport();
        report.LoadReportDefinition(reportDefinition);
        report.DataSources.Add(new ReportDataSource("source", dataSource));
        report.SetParameters(new[] { new ReportParameter("Parameter1", "Parameter value") });
        byte[] pdf = report.Render("PDF");*/
    }
}