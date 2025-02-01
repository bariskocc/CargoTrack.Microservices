using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CargoTrack.Services.Identity.API.Application.Commands;
using CargoTrack.Services.Identity.API.Application.DTOs;
using CargoTrack.Services.Identity.API.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CargoTrack.Services.Identity.API.Presentation.Controllers
{
    /// <summary>
    /// Kullanıcı yönetimi için API endpoint'leri
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [SwaggerTag("Kullanıcı yönetimi işlemleri")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Yeni kullanıcı kaydı oluşturur
        /// </summary>
        /// <param name="createUserDto">Kullanıcı oluşturma modeli</param>
        /// <returns>Oluşturulan kullanıcı bilgileri</returns>
        [HttpPost("register")]
        [SwaggerOperation(Summary = "Yeni kullanıcı kaydı oluşturur")]
        [SwaggerResponse(200, "Kullanıcı başarıyla oluşturuldu", typeof(UserDto))]
        [SwaggerResponse(400, "Geçersiz istek")]
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

        /// <summary>
        /// Kullanıcı girişi yapar ve JWT token döner
        /// </summary>
        /// <param name="loginDto">Giriş bilgileri</param>
        /// <returns>JWT token</returns>
        [HttpPost("login")]
        [SwaggerOperation(Summary = "Kullanıcı girişi yapar")]
        [SwaggerResponse(200, "Giriş başarılı", typeof(string))]
        [SwaggerResponse(400, "Geçersiz giriş bilgileri")]
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

        /// <summary>
        /// Tüm kullanıcıları listeler
        /// </summary>
        /// <param name="includeInactive">Pasif kullanıcıları da getir</param>
        /// <returns>Kullanıcı listesi</returns>
        [Authorize]
        [HttpGet]
        [SwaggerOperation(Summary = "Tüm kullanıcıları listeler")]
        [SwaggerResponse(200, "Kullanıcılar başarıyla getirildi", typeof(IEnumerable<UserDto>))]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAll([FromQuery] bool includeInactive = false)
        {
            try
            {
                var query = new GetAllUsersQuery(includeInactive);
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Kullanıcıları arar
        /// </summary>
        /// <param name="searchTerm">Arama terimi</param>
        /// <returns>Bulunan kullanıcılar</returns>
        [Authorize]
        [HttpGet("search")]
        [SwaggerOperation(Summary = "Kullanıcıları arar")]
        [SwaggerResponse(200, "Arama sonuçları başarıyla getirildi", typeof(IEnumerable<UserDto>))]
        public async Task<ActionResult<IEnumerable<UserDto>>> Search([FromQuery] string searchTerm)
        {
            try
            {
                var query = new SearchUsersQuery(searchTerm);
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Belirtilen ID'ye sahip kullanıcıyı getirir
        /// </summary>
        /// <param name="id">Kullanıcı ID'si</param>
        /// <returns>Kullanıcı detayları</returns>
        [Authorize]
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Kullanıcı detaylarını getirir")]
        [SwaggerResponse(200, "Kullanıcı başarıyla getirildi", typeof(UserDto))]
        [SwaggerResponse(404, "Kullanıcı bulunamadı")]
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

        /// <summary>
        /// Kullanıcı bilgilerini günceller
        /// </summary>
        /// <param name="id">Kullanıcı ID'si</param>
        /// <param name="updateUserDto">Güncellenecek bilgiler</param>
        /// <returns>Güncellenmiş kullanıcı bilgileri</returns>
        [Authorize]
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Kullanıcı bilgilerini günceller")]
        [SwaggerResponse(200, "Kullanıcı başarıyla güncellendi", typeof(UserDto))]
        [SwaggerResponse(400, "Geçersiz istek")]
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

        /// <summary>
        /// Kullanıcının rollerini günceller
        /// </summary>
        /// <param name="id">Kullanıcı ID'si</param>
        /// <param name="updateRolesDto">Yeni roller</param>
        /// <returns>Güncellenmiş kullanıcı bilgileri</returns>
        [Authorize]
        [HttpPut("{id}/roles")]
        [SwaggerOperation(Summary = "Kullanıcının rollerini günceller")]
        [SwaggerResponse(200, "Roller başarıyla güncellendi", typeof(UserDto))]
        [SwaggerResponse(400, "Geçersiz istek")]
        public async Task<ActionResult<UserDto>> UpdateRoles(Guid id, [FromBody] UpdateUserRolesDto updateRolesDto)
        {
            try
            {
                var command = new UpdateUserRolesCommand(id, updateRolesDto);
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Kullanıcının aktiflik durumunu günceller
        /// </summary>
        /// <param name="id">Kullanıcı ID'si</param>
        /// <param name="updateStatusDto">Yeni durum</param>
        /// <returns>Güncellenmiş kullanıcı bilgileri</returns>
        [Authorize]
        [HttpPut("{id}/status")]
        [SwaggerOperation(Summary = "Kullanıcının aktiflik durumunu günceller")]
        [SwaggerResponse(200, "Durum başarıyla güncellendi", typeof(UserDto))]
        [SwaggerResponse(400, "Geçersiz istek")]
        public async Task<ActionResult<UserDto>> UpdateStatus(Guid id, [FromBody] UpdateUserStatusDto updateStatusDto)
        {
            try
            {
                var command = new UpdateUserStatusCommand(id, updateStatusDto);
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Kullanıcıyı siler
        /// </summary>
        /// <param name="id">Kullanıcı ID'si</param>
        /// <returns>İşlem sonucu</returns>
        [Authorize]
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Kullanıcıyı siler")]
        [SwaggerResponse(200, "Kullanıcı başarıyla silindi")]
        [SwaggerResponse(400, "Geçersiz istek")]
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

        /// <summary>
        /// Kullanıcının şifresini değiştirir
        /// </summary>
        /// <param name="id">Kullanıcı ID'si</param>
        /// <param name="changePasswordDto">Şifre değiştirme bilgileri</param>
        /// <returns>İşlem sonucu</returns>
        [Authorize]
        [HttpPost("{id}/change-password")]
        [SwaggerOperation(Summary = "Kullanıcının şifresini değiştirir")]
        [SwaggerResponse(200, "Şifre başarıyla değiştirildi")]
        [SwaggerResponse(400, "Geçersiz istek")]
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