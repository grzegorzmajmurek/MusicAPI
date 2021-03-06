using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicAPI.Data;
using MusicAPI.DTOs.SongDTO;
using MusicAPI.Models;

namespace MusicAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlaylistsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private ApiDbContext _dbContext;

        public PlaylistsController(IMapper mapper, ApiDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PlaylistDto playlist)
        {
            await _dbContext.Playlists.AddAsync(_mapper.Map<Playlist>(playlist));
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
                }
                ).Where(list => list.Id == playlistId).ToListAsync();
            return Ok(playlistDetails);
        }

        //api/playlists/{playlistId}/songs
        [HttpGet("{playlistId}/songs")]
        public async Task<IActionResult> PlaylistDetailsSongs(int playlistId, SongDto songDto)
        {

            var playlistDetails = await (
                from list in _dbContext.Playlists
                select new
                {
                    Id = list.Id,
                    Songs = list.Songs
                }
                ).Where(list => list.Id == playlistId).ToListAsync();
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

        // PUT api/<PlaylistController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] PlaylistDto playlistObj)
        {
            var playlist = await _dbContext.Playlists.FindAsync(id);
            if (playlist == null)
            {
                return NotFound("No playlist found against this Id");
            }
            else
            {
                playlist.Name = playlistObj.Name;
                await _dbContext.SaveChangesAsync();
                return Ok("Playlist updated sucessfully");
            }
        }

        // DELETE api/<PlaylistsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var playlist = _dbContext.Playlists.Find(id);
            _dbContext.Playlists.Remove(playlist);
            await _dbContext.SaveChangesAsync();
            return Ok("Playlist deleted successfully");
        }
    }
}
