using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CargoTrack.Services.Identity.API.Domain.Entities;

namespace CargoTrack.Services.Identity.API.Domain.Interfaces
{
    public interface IRoleRepository : IRepository<Role>
    {
        Task<Role> GetByNameAsync(string name);
        Task<bool> IsNameUniqueAsync(string name);
        Task<IEnumerable<Role>> GetRolesWithPermissionsAsync();
        Task<IEnumerable<Permission>> GetRolePermissionsAsync(Guid roleId);
        Task<bool> HasPermissionAsync(Guid roleId, string permissionSystemName);
    }
} 