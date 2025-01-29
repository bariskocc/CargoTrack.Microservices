using System.Collections.Generic;
using System.Threading.Tasks;
using CargoTrack.Services.Identity.API.Domain.Entities;

namespace CargoTrack.Services.Identity.API.Domain.Interfaces
{
    public interface IPermissionRepository : IRepository<Permission>
    {
        Task<Permission> GetBySystemNameAsync(string systemName);
        Task<IEnumerable<Permission>> GetByCategoryAsync(string category);
        Task<bool> IsSystemNameUniqueAsync(string systemName);
        Task<IEnumerable<string>> GetAllCategoriesAsync();
    }
} 