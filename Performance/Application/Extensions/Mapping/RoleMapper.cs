using Performance.Application.DTOs.Roles;
using Performance.Application.Interface.Security;
using Performance.Domain.Entity;

namespace Performance.Application.Extensions.Mapping
{
    public static class RoleMapper
    {
        extension(Role role)
        {
            public RoleDTO ToDTO(IIdHelper idHelper) => new(
                Id: idHelper.EncryptId(role.Id),
                Name: role.Name
            );
        }
    }
}