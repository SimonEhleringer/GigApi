using AutoMapper;
using GigApi.Application.Services.Songs;
using GigApi.Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GigApi.Api.V1.Songs
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize] //(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)
    public class SongsController : ControllerBase
    {
        private readonly SongService _service;
        private readonly IMapper _mapper;

        public SongsController(SongService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var songs = await _service.GetAllAsync(HttpContext.GetUserId());

            var responses = _mapper.Map<List<SongResponse>>(songs);

            return Ok(responses);
        }

        [HttpGet("{songId}")]
        public async Task<IActionResult> Get([FromRoute] Guid songId)
        {
            var song = await _service.GetByIdAsync(songId, HttpContext.GetUserId());
            
            if (song == null)
            {
                return NotFound();
            }

            var response = _mapper.Map<SongResponse>(song);

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUpdateSongRequest request)
        {
            var songToCreate = _mapper.Map<Song>(request);

            var createdSong = await _service.CreateAsync(songToCreate, HttpContext.GetUserId());

            var response = _mapper.Map<SongResponse>(createdSong);

            return Created(HttpContext.GetLocationHeader(Url, response.SongId), response);
        }

        [HttpPut("{songId}")]
        public async Task<IActionResult> Update([FromRoute] Guid songId, [FromBody] CreateUpdateSongRequest request)
        {
            var songToUpdate = _mapper.Map<Song>(request);
            songToUpdate.SongId = songId;

            var updatedSong = await _service.UpdateAsync(songToUpdate, HttpContext.GetUserId());

            if (updatedSong == null)
            {
                return NotFound();
            }

            var response = _mapper.Map<SongResponse>(updatedSong);

            return Ok(response);
        }

        [HttpDelete("{songId}")]
        public async Task<IActionResult> Delete([FromRoute] Guid songId)
        {
            var isDeleted = await _service.DeleteAsync(songId, HttpContext.GetUserId());

            if (!isDeleted)
            {
                return NotFound();
            }

            return NoContent();
        }

    }
}
