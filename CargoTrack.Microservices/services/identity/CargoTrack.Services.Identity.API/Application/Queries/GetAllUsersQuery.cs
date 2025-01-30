using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CargoTrack.Services.Identity.API.Application.DTOs;
using CargoTrack.Services.Identity.API.Domain.Interfaces;
using MediatR;

namespace CargoTrack.Services.Identity.API.Application.Queries
{
    public class GetAllUsersQuery : IRequest<IEnumerable<UserDto>>
    {
        public bool IncludeInactive { get; }

        public GetAllUsersQuery(bool includeInactive = false)
        {
            IncludeInactive = includeInactive;
        }
    }

    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<UserDto>>
    {
        private readonly IUserRepository _userRepository;

        public GetAllUsersQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAllAsync();
            var userDtos = new List<UserDto>();

            foreach (var user in users)
            {
                if (!request.IncludeInactive && !user.IsActive)
                    continue;

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