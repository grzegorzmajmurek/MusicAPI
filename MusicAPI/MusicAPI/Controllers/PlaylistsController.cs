using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicAPI.Data;
using MusicAPI.Models;

namespace MusicAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlaylistsController : ControllerBase
    {
        private ApiDbContext _dbContext;
        public PlaylistsController(ApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Playlist playlist)
        {
            await _dbContext.Playlists.AddAsync(playlist);
            await _dbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);
        }

        //api/playlists
        [HttpGet]
        public async Task<IActionResult> GetPlaylists()
        {
            var playlists = await (from playlist in _dbContext.Playlists
                                   select new
                                   {
                                       Id = playlist.Id,
                                       Name = playlist.Name,
                                       Songs = playlist.Songs
                                   }).ToListAsync();

            return Ok(playlists);
        }

        //api/playlists/{playlistId}
        [HttpGet("{playlistId}")]
        public async Task<IActionResult> PlaylistDetails(int playlistId)
        {
            var playlistDetails = await (
                from list in _dbContext.Playlists
                select new
                {
                    Id = list.Id,
                    Name = list.Name,
                    Songs = list.Songs
                }
                ).Where(list => list.Id == playlistId).ToListAsync();
            return Ok(playlistDetails);
        }

        //api/playlists/{playlistId}/songs
        [HttpGet("{playlistId}/songs")]
        public async Task<IActionResult> PlaylistDetailsSongs()
        {
            var playlistDetails = await (
                from list in _dbContext.Playlists
                select new
                {
                    Songs = list.Songs
                }
                ).ToListAsync();
            return Ok(playlistDetails);
        }

        //api/playlists/{playlistId}/songs/{id}
        [HttpGet("{playlistId}/songs/{id}")]
        public async Task<IActionResult> PlaylistDetailsOneSong(int playlistId, int id)
        {
            var playlistDetails = await (
               from list in _dbContext.Playlists
               select new
               {
                   Id = list.Id,
                   Songs = list.Songs
               }).ToListAsync();


            var songs = playlistDetails
                .Where(playlist => playlist.Id == playlistId)
                // sprawdz czy jest puste , jezeli tak to []
                .Select(playlist => new { Songs = playlist.Songs })
                .First()
                .Songs
                .Where(song => song.Id == id);
                return Ok(songs);
        }
    }
}
