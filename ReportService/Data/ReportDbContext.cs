using global::ReportService.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Models;
using System.Collections.Generic;



namespace ReportService.Data
{
  

    namespace ReportService.Data
    {
        public class ReportDbContext : DbContext
        {
            public ReportDbContext(DbContextOptions<ReportDbContext> options)
                : base(options)
            {
            }

            public DbSet<Report> Reports => Set<Report>();
            public DbSet<Person> Persons { get; set; }
            public DbSet<ContactInfo> ContactInfos => Set<ContactInfo>();

        }
    }

}
