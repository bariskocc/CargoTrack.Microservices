using System;
using System.Threading.Tasks;
using CargoTrack.Services.Identity.API.Application.Commands;
using CargoTrack.Services.Identity.API.Application.DTOs;
using CargoTrack.Services.Identity.API.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CargoTrack.Services.Identity.API.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register([FromBody] CreateUserDto createUserDto)
        {
            try
            {
                var command = new CreateUserCommand(createUserDto);
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromBody] UserLoginDto loginDto)
        {
            try
            {
                var command = new UserLoginCommand(loginDto);
                var token = await _mediator.Send(command);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetById(Guid id)
        {
            try
            {
                var query = new GetUserByIdQuery(id);
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<UserDto>> Update(Guid id, [FromBody] UpdateUserDto updateUserDto)
        {
            try
            {
                var command = new UpdateUserCommand(id, updateUserDto);
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(Guid id)
        {
            try
            {
                var command = new DeleteUserCommand(id);
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("{id}/change-password")]
        public async Task<ActionResult<bool>> ChangePassword(Guid id, [FromBody] UserChangePasswordDto changePasswordDto)
        {
            try
            {
                var command = new ChangePasswordCommand(id, changePasswordDto);
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
} 