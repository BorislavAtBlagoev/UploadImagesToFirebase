using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace UploadImage.Models
{
    public class User : IdentityUser
    {
        public IEnumerable<Image> Images { get; set; } = new HashSet<Image>();
    }
}
