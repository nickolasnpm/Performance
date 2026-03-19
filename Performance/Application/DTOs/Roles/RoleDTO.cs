using System.Diagnostics.CodeAnalysis;
using Performance.Domain.Entity;

namespace Performance.Application.DTOs.Roles
{
    public class RoleDTO
    {
        public long Id { get; set; }
        public required string Name { get; set; }
    }
}