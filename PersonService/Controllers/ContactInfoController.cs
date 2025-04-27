using Microsoft.AspNetCore.Mvc;
using PersonService.Services;
using Shared.Models;

namespace PersonService.Controllers;

[ApiController]
[Route("api/person/{personId}/contact")]
public class ContactInfoController : ControllerBase
{
    private readonly IContactInfoService _contactInfoService;

    public ContactInfoController(IContactInfoService contactInfoService)
    {
        _contactInfoService = contactInfoService;
    }

    [HttpGet]
    public async Task<IActionResult> GetContactInfos(Guid personId)
    {
        var contacts = await _contactInfoService.GetContactInfosByPersonIdAsync(personId);
        return Ok(contacts);
    }

    [HttpPost]
    public async Task<IActionResult> AddContactInfo(Guid personId, [FromBody] ContactInfo contactInfo)
    {
        var added = await _contactInfoService.AddContactInfoAsync(personId, contactInfo);
        return Ok(added);
    }

    [HttpDelete("{contactId}")]
    public async Task<IActionResult> DeleteContactInfo(Guid contactId)
    {
        var deleted = await _contactInfoService.DeleteContactInfoAsync(contactId);
        return deleted ? NoContent() : NotFound();
    }
}
