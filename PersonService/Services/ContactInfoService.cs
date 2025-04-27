using Microsoft.EntityFrameworkCore;
using PersonService.Data;
using Shared.Models;

namespace PersonService.Services;

public class ContactInfoService : IContactInfoService
{
    private readonly PhoneBookDbContext _context;

    public ContactInfoService(PhoneBookDbContext context)
    {
        _context = context;
    }

    public async Task<List<ContactInfo>> GetContactInfosByPersonIdAsync(Guid personId)
    {
        return await _context.ContactInfos
            .Where(c => c.PersonId == personId)
            .ToListAsync();
    }

    public async Task<ContactInfo> AddContactInfoAsync(Guid personId, ContactInfo contactInfo)
    {
        contactInfo.PersonId = personId;
        _context.ContactInfos.Add(contactInfo);
        await _context.SaveChangesAsync();
        return contactInfo;
    }

    public async Task<bool> DeleteContactInfoAsync(Guid contactInfoId)
    {
        var contact = await _context.ContactInfos.FindAsync(contactInfoId);
        if (contact == null) return false;

        _context.ContactInfos.Remove(contact);
        await _context.SaveChangesAsync();
        return true;
    }
}
