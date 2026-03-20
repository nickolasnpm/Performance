using Performance.Application.DTOs.Roles;
using Performance.Domain.Entity;

namespace Performance.Application.Extensions.Mapping.Roles
{
    public static class RoleMapper
    {
        public static RoleDTO ToDTO (Role role) => new()
        {
            Id = role.Id,
            Name = role.Name
        };
    }
}