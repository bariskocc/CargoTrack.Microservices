using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CargoTrack.Services.Identity.API.Application.DTOs;
using CargoTrack.Services.Identity.API.Domain.Interfaces;
using MediatR;

namespace CargoTrack.Services.Identity.API.Application.Queries
{
    public class GetUsersByCompanyQuery : IRequest<IEnumerable<UserDto>>
    {
        public string CompanyName { get; }
        public bool IncludeInactive { get; }

        public GetUsersByCompanyQuery(string companyName, bool includeInactive = false)
        {
            CompanyName = companyName;
            IncludeInactive = includeInactive;
        }
    }

    public class GetUsersByCompanyQueryHandler : IRequestHandler<GetUsersByCompanyQuery, IEnumerable<UserDto>>
    {
        private readonly IUserRepository _userRepository;

        public GetUsersByCompanyQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<UserDto>> Handle(GetUsersByCompanyQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetByCompanyNameAsync(request.CompanyName);
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