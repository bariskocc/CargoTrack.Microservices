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
    /// İzin yönetimi için API endpoint'leri
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    [SwaggerTag("İzin yönetimi işlemleri")]
    public class PermissionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PermissionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Tüm izinleri listeler
        /// </summary>
        /// <returns>İzin listesi</returns>
        [HttpGet]
        [SwaggerOperation(Summary = "Tüm izinleri listeler")]
        [SwaggerResponse(200, "İzinler başarıyla getirildi", typeof(IEnumerable<PermissionDto>))]
        public async Task<ActionResult<IEnumerable<PermissionDto>>> GetAll()
        {
            try
            {
                var query = new GetAllPermissionsQuery();
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Belirtilen ID'ye sahip izni getirir
        /// </summary>
        /// <param name="id">İzin ID'si</param>
        /// <returns>İzin detayları</returns>
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "İzin detaylarını getirir")]
        [SwaggerResponse(200, "İzin başarıyla getirildi", typeof(PermissionDto))]
        [SwaggerResponse(404, "İzin bulunamadı")]
        public async Task<ActionResult<PermissionDto>> GetById(Guid id)
        {
            try
            {
                var query = new GetPermissionByIdQuery(id);
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Belirtilen kategorideki izinleri getirir
        /// </summary>
        /// <param name="category">İzin kategorisi</param>
        /// <returns>İzin listesi</returns>
        [HttpGet("category/{category}")]
        [SwaggerOperation(Summary = "Kategoriye göre izinleri getirir")]
        [SwaggerResponse(200, "İzinler başarıyla getirildi", typeof(IEnumerable<PermissionDto>))]
        public async Task<ActionResult<IEnumerable<PermissionDto>>> GetByCategory(string category)
        {
            try
            {
                var query = new GetPermissionsByCategoryQuery(category);
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Tüm izin kategorilerini getirir
        /// </summary>
        /// <returns>Kategori listesi</returns>
        [HttpGet("categories")]
        [SwaggerOperation(Summary = "Tüm izin kategorilerini getirir")]
        [SwaggerResponse(200, "Kategoriler başarıyla getirildi", typeof(IEnumerable<string>))]
        public async Task<ActionResult<IEnumerable<string>>> GetAllCategories()
        {
            try
            {
                var query = new GetAllPermissionCategoriesQuery();
                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Yeni izin oluşturur
        /// </summary>
        /// <param name="createPermissionDto">İzin oluşturma modeli</param>
        /// <returns>Oluşturulan izin</returns>
        [HttpPost]
        [SwaggerOperation(Summary = "Yeni izin oluşturur")]
        [SwaggerResponse(200, "İzin başarıyla oluşturuldu", typeof(PermissionDto))]
        [SwaggerResponse(400, "Geçersiz istek")]
        public async Task<ActionResult<PermissionDto>> Create([FromBody] CreatePermissionDto createPermissionDto)
        {
            try
            {
                var command = new CreatePermissionCommand(createPermissionDto);
                var result = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// İzin bilgilerini günceller
        /// </summary>
        /// <param name="id">İzin ID'si</param>
        /// <param name="updatePermissionDto">Güncellenecek bilgiler</param>
        /// <returns>Güncellenmiş izin</returns>
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "İzin bilgilerini günceller")]
        [SwaggerResponse(200, "İzin başarıyla güncellendi", typeof(PermissionDto))]
        [SwaggerResponse(400, "Geçersiz istek")]
        public async Task<ActionResult<PermissionDto>> Update(Guid id, [FromBody] UpdatePermissionDto updatePermissionDto)
        {
            try
            {
                var command = new UpdatePermissionCommand(id, updatePermissionDto);
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// İzni siler
        /// </summary>
        /// <param name="id">İzin ID'si</param>
        /// <returns>İşlem sonucu</returns>
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "İzni siler")]
        [SwaggerResponse(200, "İzin başarıyla silindi")]
        [SwaggerResponse(400, "Geçersiz istek")]
        public async Task<ActionResult<bool>> Delete(Guid id)
        {
            try
            {
                var command = new DeletePermissionCommand(id);
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