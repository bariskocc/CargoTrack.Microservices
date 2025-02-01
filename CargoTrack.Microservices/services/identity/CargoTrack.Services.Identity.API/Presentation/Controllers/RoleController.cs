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
    /// Rol yönetimi için API endpoint'leri
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    [SwaggerTag("Rol yönetimi işlemleri")]
    public class RoleController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RoleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Tüm rolleri listeler
        /// </summary>
        /// <param name="includeDeleted">Silinmiş rolleri de getir</param>
        /// <returns>Rol listesi</returns>
        [HttpGet]
        [SwaggerOperation(Summary = "Tüm rolleri listeler")]
        [SwaggerResponse(200, "Roller başarıyla getirildi", typeof(IEnumerable<RoleDto>))]
        public async Task<ActionResult<IEnumerable<RoleDto>>> GetAll([FromQuery] bool includeDeleted = false)
        {
            try
            {
                var query = new GetAllRolesQuery(includeDeleted);
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Belirtilen ID'ye sahip rolü getirir
        /// </summary>
        /// <param name="id">Rol ID'si</param>
        /// <returns>Rol detayları</returns>
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Rol detaylarını getirir")]
        [SwaggerResponse(200, "Rol başarıyla getirildi", typeof(RoleDto))]
        [SwaggerResponse(404, "Rol bulunamadı")]
        public async Task<ActionResult<RoleDto>> GetById(Guid id)
        {
            try
            {
                var query = new GetRoleByIdQuery(id);
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Yeni rol oluşturur
        /// </summary>
        /// <param name="createRoleDto">Rol oluşturma modeli</param>
        /// <returns>Oluşturulan rol</returns>
        [HttpPost]
        [SwaggerOperation(Summary = "Yeni rol oluşturur")]
        [SwaggerResponse(200, "Rol başarıyla oluşturuldu", typeof(RoleDto))]
        [SwaggerResponse(400, "Geçersiz istek")]
        public async Task<ActionResult<RoleDto>> Create([FromBody] CreateRoleDto createRoleDto)
        {
            try
            {
                var command = new CreateRoleCommand(createRoleDto);
                var result = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Rol bilgilerini günceller
        /// </summary>
        /// <param name="id">Rol ID'si</param>
        /// <param name="updateRoleDto">Güncellenecek bilgiler</param>
        /// <returns>Güncellenmiş rol</returns>
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Rol bilgilerini günceller")]
        [SwaggerResponse(200, "Rol başarıyla güncellendi", typeof(RoleDto))]
        [SwaggerResponse(400, "Geçersiz istek")]
        public async Task<ActionResult<RoleDto>> Update(Guid id, [FromBody] UpdateRoleDto updateRoleDto)
        {
            try
            {
                var command = new UpdateRoleCommand(id, updateRoleDto);
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Rolü siler
        /// </summary>
        /// <param name="id">Rol ID'si</param>
        /// <returns>İşlem sonucu</returns>
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Rolü siler")]
        [SwaggerResponse(200, "Rol başarıyla silindi")]
        [SwaggerResponse(400, "Geçersiz istek")]
        public async Task<ActionResult<bool>> Delete(Guid id)
        {
            try
            {
                var command = new DeleteRoleCommand(id);
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Rolün izinlerini günceller
        /// </summary>
        /// <param name="id">Rol ID'si</param>
        /// <param name="permissions">Yeni izinler</param>
        /// <returns>Güncellenmiş rol</returns>
        [HttpPut("{id}/permissions")]
        [SwaggerOperation(Summary = "Rolün izinlerini günceller")]
        [SwaggerResponse(200, "İzinler başarıyla güncellendi", typeof(RoleDto))]
        [SwaggerResponse(400, "Geçersiz istek")]
        public async Task<ActionResult<RoleDto>> UpdatePermissions(Guid id, [FromBody] List<string> permissions)
        {
            try
            {
                var updateDto = new RolePermissionsUpdateDto
                {
                    RoleId = id,
                    Permissions = permissions
                };
                var command = new UpdateRolePermissionsCommand(updateDto);
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