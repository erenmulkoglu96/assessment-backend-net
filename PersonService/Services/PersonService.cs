using Microsoft.EntityFrameworkCore;
using PersonService.Data;
using Shared.Models;

namespace PersonService.Services;

public class PersonService : IPersonService
{
    private readonly PhoneBookDbContext _context;

    public PersonService(PhoneBookDbContext context)
    {
        _context = context;
    }

    public async Task<List<Person>> GetAllAsync()
    {
        return await _context.Persons.Include(p => p.ContactInfos).ToListAsync();
    }

    public async Task<Person?> GetByIdAsync(Guid id)
    {
        return await _context.Persons.Include(p => p.ContactInfos).FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Person> CreateAsync(Person person)
    {
        _context.Persons.Add(person);
        await _context.SaveChangesAsync();
        return person;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var person = await _context.Persons.FindAsync(id);
        if (person == null) return false;

        _context.Persons.Remove(person);
        await _context.SaveChangesAsync();
        return true;
    }
}
