using ReportService.Models;
using Xunit;

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
            Assert.Equal(ReportStatus.Preparing, report.Status);
            Assert.NotEqual(Guid.Empty, report.Id);
            Assert.True(report.RequestedAt <= DateTime.UtcNow);
        }

        [Fact]
        public void Report_Status_Should_Be_Preparing_By_Default()
        {
            // Arrange
            var report = new Report();

            // Act
            var status = report.Status;

            // Assert
            Assert.Equal(ReportStatus.Preparing, status);
        }
    }
}
