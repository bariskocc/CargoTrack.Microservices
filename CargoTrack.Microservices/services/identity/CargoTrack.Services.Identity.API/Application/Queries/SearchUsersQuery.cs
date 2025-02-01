using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CargoTrack.Services.Identity.API.Application.DTOs;
using CargoTrack.Services.Identity.API.Domain.Interfaces;
using MediatR;

namespace CargoTrack.Services.Identity.API.Application.Queries
{
    public class SearchUsersQuery : IRequest<IEnumerable<UserDto>>
    {
        public string SearchTerm { get; }

        public SearchUsersQuery(string searchTerm)
        {
            SearchTerm = searchTerm;
        }
    }

    public class SearchUsersQueryHandler : IRequestHandler<SearchUsersQuery, IEnumerable<UserDto>>
    {
        private readonly IUserRepository _userRepository;

        public SearchUsersQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<UserDto>> Handle(SearchUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.FindAsync(u => 
                u.Email.Contains(request.SearchTerm) ||
                u.Username.Contains(request.SearchTerm) ||
                u.FirstName.Contains(request.SearchTerm) ||
                u.LastName.Contains(request.SearchTerm) ||
                u.CompanyName.Contains(request.SearchTerm)
            );

            var userDtos = new List<UserDto>();

            foreach (var user in users)
            {
                var permissions = await _userRepository.GetUserPermissionsAsync(user.Id);
                var roles = await _userRepository.GetUsersInRoleAsync(user.Id.ToString());

                userDtos.Add(new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    Username = user.Username,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    CompanyName = user.CompanyName,
                    PhoneNumber = user.PhoneNumber,
                    IsActive = user.IsActive,
                    EmailConfirmed = user.EmailConfirmed,
                    PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                    CreatedDate = user.CreatedDate,
                    LastModifiedDate = user.LastModifiedDate,
                    LastLoginDate = user.LastLoginDate,
                    Roles = roles.Select(r => r.ToString()).ToList(),
                    Permissions = permissions.ToList()
                });
            }

            return userDtos;
        }
    }
} 