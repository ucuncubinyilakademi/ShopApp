using Microsoft.AspNetCore.Identity;

namespace ETICARET.WEBUI.Identity
{
    public class ApplicationUser:IdentityUser
    {
        public string FullName { get; set; }
    }
}
