using System.Diagnostics.CodeAnalysis;
using Performance.Domain.Entity;

namespace Performance.Application.DTOs.Roles
{
    public class RoleDTO
    {
        public long Id { get; set; }
        public required string Name { get; set; }


        // only for mapping from Role entity to RoleDTO
        [SetsRequiredMembers]
        public RoleDTO(Role role)
        {
            Id = role.Id;
            Name = role.Name;
        }
    }
}