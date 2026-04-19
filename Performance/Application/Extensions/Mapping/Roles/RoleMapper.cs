using Performance.Application.Common.Prefix;
using Performance.Application.DTOs.Roles;
using Performance.Application.Interface.Hashing;
using Performance.Domain.Entity;

namespace Performance.Application.Extensions.Mapping.Roles
{
    public static class RoleMapper
    {
        public static RoleDTO ToDTO (Role role, IIdHelper idHelper) => new()
        {
            Id = idHelper.EncodeId(role.Id, IdPrefix.Role),
            Name = role.Name
        };
    }
}