using Microsoft.EntityFrameworkCore;
using Shared.Models;
using PersonService.Data;


namespace PersonService.Data;

public class PhoneBookDbContext : DbContext
{
    public PhoneBookDbContext(DbContextOptions<PhoneBookDbContext> options)
        : base(options) { }

    public DbSet<Person> Persons => Set<Person>();
    public DbSet<ContactInfo> ContactInfos => Set<ContactInfo>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Person>().HasMany(p => p.ContactInfos).WithOne(c => c.Person).HasForeignKey(c => c.PersonId);
    }
}
