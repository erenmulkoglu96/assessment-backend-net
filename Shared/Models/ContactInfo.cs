using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models
{
    public enum ContactType
    {
        Phone,
        Email,
        Location
    }

    public class ContactInfo
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public ContactType Type { get; set; }
        public string Content { get; set; } = null!;

        public Guid PersonId { get; set; }
        public Person Person { get; set; } = null!;
    }
}
