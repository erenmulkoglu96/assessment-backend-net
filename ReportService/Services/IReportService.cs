using ReportService.Models;

namespace ReportService.Services
{
    public interface IReportService
    {
        Task<Report> RequestReportAsync();
        Task<List<Report>> GetAllReportsAsync(int? status = null, DateTime? after = null); // 🔥 GÜNCELLENDİ
        Task<Report?> GetByIdAsync(Guid id);
        Task<string?> GenerateReportContentAsync(Guid reportId);

        Task<List<Report>> GetCompletedReportsAsync();

        Task<string> GenerateReportCsvContentAsync(Guid reportId);

        Task DeleteReportAsync(Guid id);


    }
}
