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
    public class AlbumsController : ControllerBase
    {
        private ApiDbContext _dbContext;
        public AlbumsController(ApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Album album)
        {
            await _dbContext.Albums.AddAsync(album);
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
                    Name = list.Name,
                    Songs = list.Songs
                }
                ).Where(list => list.Id == albumId).ToListAsync();
            return Ok(albumDetails);
        }

        //api/albums/{albumId}/songs
        [HttpGet("{albumId}/songs")]
        public async Task<IActionResult> AlbumsDetailsSongs()
        {
            var albumDetails = await (
                from list in _dbContext.Albums
                select new
                {
                    Songs = list.Songs
                }
                ).ToListAsync();
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
    }
}
