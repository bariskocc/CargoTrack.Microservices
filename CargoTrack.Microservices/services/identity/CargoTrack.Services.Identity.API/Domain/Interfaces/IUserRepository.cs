using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CargoTrack.Services.Identity.API.Domain.Entities;

namespace CargoTrack.Services.Identity.API.Domain.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByUsernameAsync(string username);
        Task<IEnumerable<User>> GetByCompanyNameAsync(string companyName);
        Task<IEnumerable<User>> GetUsersInRoleAsync(string roleName);
        Task<bool> IsEmailUniqueAsync(string email);
        Task<bool> IsUsernameUniqueAsync(string username);
        Task<IEnumerable<string>> GetUserPermissionsAsync(Guid userId);
        Task<bool> HasPermissionAsync(Guid userId, string permissionSystemName);
    }
} 