using System.ComponentModel.DataAnnotations.Schema;

namespace ReportService.Models
{
    public class Report
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
        public ReportStatus Status { get; set; } = ReportStatus.Preparing;
        public string? FilePath { get; set; }
        public string? Content { get; set; }
        public DateTime? CompletedAt { get; set; }

        public string? CsvPath { get; set; }

        public string? CsvContent { get; set; }

        [NotMapped]
        public string? CsvUrl
        {
            get
            {
                if (string.IsNullOrWhiteSpace(CsvPath))
                    return null;

                return $"https://localhost:44393{CsvPath}";
            }
        }


        [NotMapped]
        public string? FileUrl
        {
            get
            {
                if (string.IsNullOrWhiteSpace(FilePath))
                    return null;

                // Configurable base URL için burası genişletilebilir
                return $"https://localhost:44393{FilePath}";
            }
        }





    }


}
