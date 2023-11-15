using Microsoft.AspNetCore.Identity;

namespace DataAccess.Common.IdentityModels
{
    public class AppRole : IdentityRole<Guid>
    {
        public AppRole() : base() { }
        public AppRole(string roleName) : base(roleName) { }
    }
}
