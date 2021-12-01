using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicAPI.Data;
using MusicAPI.DTOs.AlbumDTO;
using MusicAPI.Models;

namespace MusicAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private ApiDbContext _dbContext;
        public AlbumsController(IMapper mapper, ApiDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AlbumDto album)
        {
            await _dbContext.Albums.AddAsync(_mapper.Map<Album>(album));
            await _dbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);
        }

        //api/albums
        [HttpGet]
        public async Task<IActionResult> GetAlbums()
        {
            var albums = await (from album in _dbContext.Albums
                                select new
                                {
                                    Id = album.Id,
                                    ArtistId = album.ArtistId,
                                    Name = album.Name
                                }).ToListAsync();

            return Ok(albums);
        }

        //api/albums/{id}
        [HttpGet("{albumId}")]
        public async Task<IActionResult> AlbumDetails(int albumId)
        {
            var albumDetails = await (
                from list in _dbContext.Albums
                select new
                {
                    Id = list.Id,
                    Name = list.Name
                }
                ).Where(list => list.Id == albumId).ToListAsync();
            return Ok(albumDetails);
        }

        //api/albums/{albumId}/songs
        [HttpGet("{albumId}/songs")]
        public async Task<IActionResult> AlbumsDetailsSongs(int albumId)
        {
            var albumDetails = await (
                from list in _dbContext.Albums
                select new
                {
                    Id = list.Id,
                    Songs = list.Songs
                }
                ).Where(list => list.Id == albumId).ToListAsync();
            return Ok(albumDetails);
        }

        //api/albums/{albumId}/songs/{id}
        [HttpGet("{albumId}/songs/{id}")]
        public async Task<IActionResult> AlbumDetailsOneSong(int albumId, int id)
        {
            var albumDetails = await (
               from list in _dbContext.Albums
               select new
               {
                   Id = list.Id,
                   Songs = list.Songs
               }).ToListAsync();


            var songs = albumDetails
                .Where(album => album.Id == albumId)
                // sprawdz czy jest puste , jezeli tak to []
                .Select(playlist => new { Songs = playlist.Songs })
                .First()
                .Songs
                .Where(song => song.Id == id);
            return Ok(songs);
        }

        // PUT api/<AlbumController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] AlbumDto albumObj)
        {
            var album = await _dbContext.Albums.FindAsync(id);
            if (album == null)
            {
                return NotFound("No albums found against this Id");
            }
            else
            {
                album.Name = albumObj.Name;
                await _dbContext.SaveChangesAsync();
                return Ok("Album updated sucessfully");
            }
        }

        // DELETE api/<AlbumsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var album = _dbContext.Albums.Find(id);
            _dbContext.Albums.Remove(album);
            await _dbContext.SaveChangesAsync();
            return Ok("Album deleted successfully");
        }
    }
}
