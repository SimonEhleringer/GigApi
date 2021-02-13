using AutoMapper;
using GigApi.Application.Services.Playlists;
using GigApi.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GigApi.Api.V1.Playlists
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class PlaylistsController : ControllerBase
    {
        private readonly PlaylistService _service;
        private readonly IMapper _mapper;

        public PlaylistsController(PlaylistService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var playlists = await _service.GetAllAsync(HttpContext.GetUserId());

            var responses = _mapper.Map<List<PlaylistResponse>>(playlists);

            return Ok(responses);
        }

        [HttpGet("{playlistId}")]
        public async Task<IActionResult> Get([FromRoute] Guid playlistId)
        {
            var playlist = await _service.GetByIdAsync(playlistId, HttpContext.GetUserId());

            if (playlist == null)
            {
                return NotFound();
            }

            var response = _mapper.Map<PlaylistResponse>(playlist);

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUpdatePlaylistRequest request)
        {
            var playlistToCreate = _mapper.Map<Playlist>(request);

            var createdPlaylist = await _service.CreateAsync(playlistToCreate, HttpContext.GetUserId());

            var response = _mapper.Map<PlaylistResponse>(createdPlaylist);

            return Created(HttpContext.GetLocationHeader(Url, response.PlaylistId), response);
        }

        [HttpPut("{playlistId}")]
        public async Task<IActionResult> Update([FromRoute] Guid playlistId, [FromBody] CreateUpdatePlaylistRequest request)
        {
            var playlistToUpdate = _mapper.Map<Playlist>(request);
            playlistToUpdate.PlaylistId = playlistId;

            var updatedPlaylist = await _service.UpdateAsync(playlistToUpdate, HttpContext.GetUserId());

            if (updatedPlaylist == null)
            {
                return NotFound();
            }

            var response = _mapper.Map<PlaylistResponse>(updatedPlaylist);

            return Ok(response);
        }

        [HttpDelete("{playlistId}")]
        public async Task<IActionResult> Delete([FromRoute] Guid playlistId)
        {
            var isDeleted = await _service.DeleteAsync(playlistId, HttpContext.GetUserId());

            if (!isDeleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
