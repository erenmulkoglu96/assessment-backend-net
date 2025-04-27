using Shared.Models;

namespace PersonService.Services;

public interface IPersonService
{
    Task<List<Person>> GetAllAsync();
    Task<Person?> GetByIdAsync(Guid id);
    Task<Person> CreateAsync(Person person);
    Task<bool> DeleteAsync(Guid id);
}
