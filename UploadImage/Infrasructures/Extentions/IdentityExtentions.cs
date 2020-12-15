using System.Linq;
using System.Security.Claims;

namespace UploadImage.Infrasructures.Extentions
{
    public static class IdentityExtentions
    {
        public static string GetId(this ClaimsPrincipal user)
           => user
               .Claims
               .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
               ?.Value;
    }
}
