using Microsoft.EntityFrameworkCore;
using ReportService.Data;
using ReportService.Data.ReportService.Data;
using ReportService.Kafka;
using ReportService.Models;
using System.Xml;
using Newtonsoft.Json;
using Shared.Models;
using Formatting = Newtonsoft.Json.Formatting;
using System.Text;


namespace ReportService.Services
{
    public class ReportService : IReportService
    {
        private readonly ReportDbContext _context;
        private readonly ReportRequestProducer _producer;

        public ReportService(ReportDbContext context, ReportRequestProducer producer)
        {
            _context = context;
            _producer = producer;
        }

        public async Task<Report> RequestReportAsync()
        {
            var report = new Report();
            _context.Reports.Add(report);
            await _context.SaveChangesAsync();

            await _producer.SendMessageAsync(report.Id);
            return report;
        }


        public async Task<List<Report>> GetAllReportsAsync(int? status = null, DateTime? after = null)
        {
            var query = _context.Reports.AsQueryable();

            if (status != null)
                query = query.Where(r => (int)r.Status == status);

            if (after != null)
                query = query.Where(r => r.RequestedAt >= after);

            return await query.OrderByDescending(r => r.RequestedAt).ToListAsync();
        }

        public async Task<Report?> GetByIdAsync(Guid id)
        {
            return await _context.Reports.FindAsync(id);
        }

        public async Task<string> GenerateReportContentAsync(Guid reportId)
        {
            var reportData = await _context.Persons
                .Include(p => p.ContactInfos)
                .Where(p => p.ContactInfos.Any(ci => ci.Type == ContactType.Location))
                .GroupBy(p => p.ContactInfos.First(ci => ci.Type == ContactType.Location).Content)
                .Select(g => new
                {
                    Location = g.Key,
                    PersonCount = g.Count(),
                    PhoneNumberCount = g.Sum(p => p.ContactInfos.Count(ci => ci.Type == ContactType.Phone))
                })
                .ToListAsync();

            var json = JsonConvert.SerializeObject(reportData, Formatting.Indented);

            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "reports");
            Directory.CreateDirectory(folderPath); // klasör yoksa oluştur

            var jsonFileName = $"report-{Guid.NewGuid()}.json";
            var jsonFilePath = Path.Combine(folderPath, jsonFileName);

            await File.WriteAllTextAsync(jsonFilePath, json);

            // 🎯 CSV dosyası oluştur
            var csvFileName = $"report-{Guid.NewGuid()}.csv";
            var csvFilePath = Path.Combine(folderPath, csvFileName);

            var csvBuilder = new StringBuilder();
            csvBuilder.AppendLine("Location,PersonCount,PhoneNumberCount");

            foreach (var item in reportData)
            {
                csvBuilder.AppendLine($"{item.Location},{item.PersonCount},{item.PhoneNumberCount}");
            }

            var csvContent = csvBuilder.ToString();
            await File.WriteAllTextAsync(csvFilePath, csvContent);

            // 🎯 Report kaydını güncelle
            var report = await _context.Reports.FindAsync(reportId);
            if (report != null)
            {
                report.FilePath = $"/reports/{jsonFileName}";
                report.CsvPath = $"/reports/{csvFileName}";
                report.CsvContent = csvContent; // 📌 CSV içeriğini de kaydediyoruz
                report.Status = ReportStatus.Completed;
                report.CompletedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
            }

            return jsonFilePath; // JSON dosyasının path'ini geri döndürüyoruz
        }


        public async Task<string> GenerateReportCsvContentAsync(Guid reportId)
        {
            var persons = await _context.Persons
                .Include(p => p.ContactInfos)
                .ToListAsync();

            var reportData = persons.Select(p => new
            {
                p.FirstName,
                p.LastName,
                p.Company,
                Location = p.ContactInfos.FirstOrDefault(ci => ci.Type == ContactType.Location)?.Content ?? "",
                PhoneNumber = p.ContactInfos.FirstOrDefault(ci => ci.Type == ContactType.Phone)?.Content ?? ""
            }).ToList();

            var csv = new StringBuilder();
            csv.AppendLine("FirstName,LastName,Company,Location,PhoneNumber");

            foreach (var row in reportData)
            {
                csv.AppendLine($"{row.FirstName},{row.LastName},{row.Company},{row.Location},{row.PhoneNumber}");
            }

            return csv.ToString(); 
        }

        public async Task DeleteReportAsync(Guid id)
        {
            var report = await _context.Reports.FindAsync(id);
            if (report != null)
            {
                _context.Reports.Remove(report);
                await _context.SaveChangesAsync();
            }
        }





        public async Task<List<Report>> GetCompletedReportsAsync()
        {
            return await _context.Reports
                .Where(r => r.Status == ReportStatus.Completed)
                .ToListAsync();
        }

    }
}
