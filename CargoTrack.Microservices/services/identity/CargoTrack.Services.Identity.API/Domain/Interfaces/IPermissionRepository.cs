using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CargoTrack.Services.Identity.API.Domain.Entities;

namespace CargoTrack.Services.Identity.API.Domain.Interfaces
{
    public interface IPermissionRepository
    {
        Task<IEnumerable<Permission>> GetAllAsync();
        Task<Permission> GetByIdAsync(Guid id);
        Task<IEnumerable<Permission>> GetByCategoryAsync(string category);
        Task<IEnumerable<string>> GetAllCategoriesAsync();
        Task<bool> IsSystemNameUniqueAsync(string systemName);
        Task<Permission> AddAsync(Permission permission);
        Task UpdateAsync(Permission permission);
        Task DeleteAsync(Permission permission);
        Task<Permission> GetBySystemNameAsync(string systemName);
    }
} 