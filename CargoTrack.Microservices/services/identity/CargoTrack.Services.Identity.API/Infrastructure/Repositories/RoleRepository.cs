using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CargoTrack.Services.Identity.API.Domain.Entities;
using CargoTrack.Services.Identity.API.Domain.Interfaces;
using CargoTrack.Services.Identity.API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CargoTrack.Services.Identity.API.Infrastructure.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly IdentityDbContext _context;

        public RoleRepository(IdentityDbContext context)
        {
            _context = context;
        }

        public async Task<Role> GetByIdAsync(Guid id)
        {
            return await _context.Roles
                .Include(r => r.Permissions)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Role> GetByNameAsync(string name)
        {
            return await _context.Roles
                .Include(r => r.Permissions)
                .FirstOrDefaultAsync(r => r.Name == name);
        }

        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            return await _context.Roles
                .Include(r => r.Permissions)
                .ToListAsync();
        }

        public async Task AddAsync(Role role)
        {
            await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Role role)
        {
            _context.Roles.Update(role);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Role role)
        {
            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
        }
    }
} 