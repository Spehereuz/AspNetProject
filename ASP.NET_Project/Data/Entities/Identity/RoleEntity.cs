using Microsoft.AspNetCore.Identity;

namespace ASP.NET_Project.Data.Entities.Identity
{
    public class RoleEntity : IdentityRole<int>
    {
        public ICollection<UserRoleEntity>? UserRoles { get; set; }
    }
}
