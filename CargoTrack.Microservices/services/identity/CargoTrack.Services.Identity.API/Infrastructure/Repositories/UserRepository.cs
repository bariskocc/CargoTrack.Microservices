using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CargoTrack.Services.Identity.API.Domain.Entities;
using CargoTrack.Services.Identity.API.Domain.Interfaces;
using CargoTrack.Services.Identity.API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CargoTrack.Services.Identity.API.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IdentityDbContext _context;

        public UserRepository(IdentityDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            return await _context.Users
                .Include(u => u.Roles)
                .ThenInclude(r => r.Permissions)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.Roles)
                .ThenInclude(r => r.Permissions)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _context.Users
                .Include(u => u.Roles)
                .ThenInclude(r => r.Permissions)
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users
                .Include(u => u.Roles)
                .ThenInclude(r => r.Permissions)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetByCompanyNameAsync(string companyName)
        {
            return await _context.Users
                .Include(u => u.Roles)
                .ThenInclude(r => r.Permissions)
                .Where(u => u.CompanyName == companyName)
                .ToListAsync();
        }

        public async Task<bool> IsEmailUniqueAsync(string email)
        {
            return !await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<bool> IsUsernameUniqueAsync(string username)
        {
            return !await _context.Users.AnyAsync(u => u.Username == username);
        }

        public async Task<IEnumerable<string>> GetUserPermissionsAsync(Guid userId)
        {
            var user = await _context.Users
                .Include(u => u.Roles)
                .ThenInclude(r => r.Permissions)
                .FirstOrDefaultAsync(u => u.Id == userId);

            return user?.Roles
                .SelectMany(r => r.Permissions)
                .Select(p => p.Name)
                .Distinct()
                .ToList() ?? new List<string>();
        }

        public async Task<IEnumerable<Role>> GetUsersInRoleAsync(string userId)
        {
            var user = await _context.Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Id == Guid.Parse(userId));

            return user?.Roles ?? new List<Role>();
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
} 