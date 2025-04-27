using System;
using Xunit;
using ReportService.Models;

namespace PhoneBook.Tests
{
    public class ReportServiceTests
    {
        [Fact]
        public void Report_Should_Set_Default_Values()
        {
            // Arrange
            var report = new Report();

            // Assert
            Assert.NotEqual(Guid.Empty, report.Id);
            Assert.Equal(ReportStatus.Preparing, report.Status);
            Assert.NotEqual(default(DateTime), report.RequestedAt);
            Assert.Null(report.FilePath);
            Assert.Null(report.Content);
        }
    }
}
