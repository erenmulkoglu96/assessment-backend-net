using Shared.Models;

namespace PersonService.Services;

public interface IContactInfoService
{
    Task<List<ContactInfo>> GetContactInfosByPersonIdAsync(Guid personId);
    Task<ContactInfo> AddContactInfoAsync(Guid personId, ContactInfo contactInfo);
    Task<bool> DeleteContactInfoAsync(Guid contactInfoId);
}
