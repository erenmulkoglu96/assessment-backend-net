using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReportService.Services;

namespace ReportService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportController : ControllerBase
{
    private readonly IReportService _reportService;
    private readonly ILogger<ReportController> _logger;


    public ReportController(IReportService reportService, ILogger<ReportController> logger)
    {
        _reportService = reportService;
        _logger = logger;

    }

    [HttpPost]
    public async Task<IActionResult> RequestReport()
    {
        var report = await _reportService.RequestReportAsync();
        return Ok(report);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllReports([FromQuery] int? status, [FromQuery] DateTime? after)
    {
        var reports = await _reportService.GetAllReportsAsync(status, after);
        return Ok(reports);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetReportById(Guid id)
    {
        var report = await _reportService.GetByIdAsync(id);
        if (report == null)
            return NotFound();

        var baseUrl = $"{Request.Scheme}://{Request.Host}";
        return Ok(new
        {
            report.Id,
            report.Status,
            report.RequestedAt,
            report.CompletedAt,
            report.Content,
            report.FilePath,
            report.CsvPath,
            CsvUrl = string.IsNullOrEmpty(report.CsvPath) ? null : $"{baseUrl}{report.CsvPath}"
        });
    }


    [HttpGet("{id}/download")]
    public async Task<IActionResult> DownloadReportFile(Guid id)
    {
        var report = await _reportService.GetByIdAsync(id);
        if (report == null || string.IsNullOrWhiteSpace(report.FilePath))
            return NotFound("Rapor veya dosya bulunamadı.");

        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", report.FilePath.TrimStart('/'));

        if (!System.IO.File.Exists(filePath))
            return NotFound("Dosya fiziksel olarak bulunamadı.");

        try
        {
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/json", $"report-{id}.json");
        }
        catch (IOException)
        {
            return StatusCode(500, "Rapor dosyasına erişilirken bir hata oluştu.");
        }
    }


    [HttpGet("completed")]
    public async Task<IActionResult> GetCompletedReports()
    {
        var reports = await _reportService.GetCompletedReportsAsync();
        return Ok(reports);
    }

    [HttpGet("{id}/download/csv")]
    public async Task<IActionResult> DownloadReportFileCsv(Guid id)
    {
        var report = await _reportService.GetByIdAsync(id);
        if (report == null || string.IsNullOrWhiteSpace(report.CsvPath))
            return NotFound("CSV dosyası oluşturulmamış.");

        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", report.CsvPath.TrimStart('/'));

        if (!System.IO.File.Exists(path))
            return NotFound("CSV dosyası fiziksel olarak bulunamadı.");

        var bytes = await System.IO.File.ReadAllBytesAsync(path);
        return File(bytes, "text/csv", Path.GetFileName(path));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReport(Guid id)
    {
        var report = await _reportService.GetByIdAsync(id);
        if (report == null)
            return NotFound("Rapor bulunamadı.");

        // JSON dosyasını sil
        if (!string.IsNullOrWhiteSpace(report.FilePath))
        {
            var jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", report.FilePath.TrimStart('/'));
            if (System.IO.File.Exists(jsonFilePath))
                System.IO.File.Delete(jsonFilePath);
        }

        // CSV dosyasını sil
        if (!string.IsNullOrWhiteSpace(report.CsvPath))
        {
            var csvFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", report.CsvPath.TrimStart('/'));
            if (System.IO.File.Exists(csvFilePath))
                System.IO.File.Delete(csvFilePath);
        }

        // Veritabanından raporu sil
        await _reportService.DeleteReportAsync(id);

        return Ok("Rapor ve dosyaları başarıyla silindi.");
    }





}
