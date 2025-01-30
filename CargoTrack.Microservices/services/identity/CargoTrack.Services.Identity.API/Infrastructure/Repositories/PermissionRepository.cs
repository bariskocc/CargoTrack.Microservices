using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CargoTrack.Services.Identity.API.Domain.Entities;
using CargoTrack.Services.Identity.API.Domain.Interfaces;
using CargoTrack.Services.Identity.API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CargoTrack.Services.Identity.API.Infrastructure.Repositories
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly IdentityDbContext _context;

        public PermissionRepository(IdentityDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Permission> GetByIdAsync(Guid id)
        {
            if (_context.Permissions == null)
                throw new InvalidOperationException("Permissions DbSet is null");

            return await _context.Permissions
                .Include(p => p.Roles)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Permission> GetBySystemNameAsync(string systemName)
        {
            if (_context.Permissions == null)
                throw new InvalidOperationException("Permissions DbSet is null");

            return await _context.Permissions
                .Include(p => p.Roles)
                .FirstOrDefaultAsync(p => p.SystemName == systemName);
        }

        public async Task<IEnumerable<Permission>> GetByCategoryAsync(string category)
        {
            if (_context.Permissions == null)
                throw new InvalidOperationException("Permissions DbSet is null");

            return await _context.Permissions
                .Include(p => p.Roles)
                .Where(p => p.Category == category)
                .ToListAsync();
        }

        public async Task<IEnumerable<Permission>> GetAllAsync()
        {
            if (_context.Permissions == null)
                throw new InvalidOperationException("Permissions DbSet is null");

            return await _context.Permissions
                .Include(p => p.Roles)
                .ToListAsync();
        }

        public async Task<Permission> AddAsync(Permission permission)
        {
            if (_context.Permissions == null)
                throw new InvalidOperationException("Permissions DbSet is null");

            await _context.Permissions.AddAsync(permission);
            await _context.SaveChangesAsync();
            return permission;
        }

        public async Task UpdateAsync(Permission permission)
        {
            if (_context.Permissions == null)
                throw new InvalidOperationException("Permissions DbSet is null");

            _context.Permissions.Update(permission);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Permission permission)
        {
            if (_context.Permissions == null)
                throw new InvalidOperationException("Permissions DbSet is null");

            permission.Delete();
            _context.Permissions.Update(permission);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsSystemNameUniqueAsync(string systemName)
        {
            if (_context.Permissions == null)
                throw new InvalidOperationException("Permissions DbSet is null");

            return !await _context.Permissions.AnyAsync(p => p.SystemName == systemName);
        }

        public async Task<IEnumerable<string>> GetAllCategoriesAsync()
        {
            if (_context.Permissions == null)
                throw new InvalidOperationException("Permissions DbSet is null");

            return await _context.Permissions
                .Select(p => p.Category)
                .Distinct()
                .Where(c => c != null)
                .ToListAsync();
        }

        public async Task<IEnumerable<Permission>> FindAsync(Expression<Func<Permission, bool>> predicate)
        {
            if (_context.Permissions == null)
                throw new InvalidOperationException("Permissions DbSet is null");

            return await _context.Permissions
                .Include(p => p.Roles)
                .Where(predicate)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(Expression<Func<Permission, bool>> predicate)
        {
            if (_context.Permissions == null)
                throw new InvalidOperationException("Permissions DbSet is null");

            return await _context.Permissions.AnyAsync(predicate);
        }

        public async Task<int> CountAsync(Expression<Func<Permission, bool>> predicate)
        {
            if (_context.Permissions == null)
                throw new InvalidOperationException("Permissions DbSet is null");

            return await _context.Permissions.CountAsync(predicate);
        }
    }
} 