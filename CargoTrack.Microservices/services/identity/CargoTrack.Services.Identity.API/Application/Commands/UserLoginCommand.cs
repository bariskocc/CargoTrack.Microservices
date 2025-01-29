using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CargoTrack.Services.Identity.API.Application.DTOs;
using CargoTrack.Services.Identity.API.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using BC = BCrypt.Net.BCrypt;

namespace CargoTrack.Services.Identity.API.Application.Commands
{
    public class UserLoginCommand : IRequest<string>
    {
        public UserLoginDto LoginDto { get; }

        public UserLoginCommand(UserLoginDto loginDto)
        {
            LoginDto = loginDto;
        }
    }

    public class UserLoginCommandHandler : IRequestHandler<UserLoginCommand, string>
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public UserLoginCommandHandler(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<string> Handle(UserLoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.LoginDto.Email);
            if (user == null)
                throw new Exception("Geçersiz email veya şifre.");

            if (!user.IsActive)
                throw new Exception("Hesabınız aktif değil.");

            if (user.LockoutEnd.HasValue && user.LockoutEnd.Value > DateTime.UtcNow)
                throw new Exception($"Hesabınız {user.LockoutEnd.Value.ToLocalTime()} tarihine kadar kilitli.");

            if (!BC.Verify(request.LoginDto.Password, user.PasswordHash))
            {
                user.RecordLoginFailure();
                await _userRepository.UpdateAsync(user);
                throw new Exception("Geçersiz email veya şifre.");
            }

            user.RecordLoginSuccess();
            await _userRepository.UpdateAsync(user);

            var permissions = await _userRepository.GetUserPermissionsAsync(user.Id);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim("CompanyName", user.CompanyName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["Jwt:ExpireDays"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
} 