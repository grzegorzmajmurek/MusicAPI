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
    public class GenresController : ControllerBase
    {
        private ApiDbContext _dbContext;
        public GenresController(ApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Genre genre)
        {
            await _dbContext.Genres.AddAsync(genre);
            await _dbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);
        }

        //api/genres
        [HttpGet]
        public async Task<IActionResult> GetGenres()
        {
            var genres = await (from genre in _dbContext.Genres
                                select new
                                {
                                    Id = genre.Id,
                                    Name = genre.Name
                                }).ToListAsync();

            return Ok(genres);
        }
        //api/genres/{id}
        [HttpGet("{genreId}")]
        public async Task<IActionResult> AlbumDetails(int genreId)
        {
            var genreDetails = await (
                from list in _dbContext.Genres
                select new
                {
                    Id = list.Id,
                    Name = list.Name,
                    Songs = list.Songs
                }
                ).Where(list => list.Id == genreId).ToListAsync();
            return Ok(genreDetails);
        }

        //api/genres/{albumId}/songs
        [HttpGet("{genreId}/songs")]
        public async Task<IActionResult> AlbumsDetailsSongs()
        {
            var genreDetails = await (
                from list in _dbContext.Genres
                select new
                {
                    Songs = list.Songs
                }
                ).ToListAsync();
            return Ok(genreDetails);
        }

        //api/genres/{genreId}/songs/{id}
        [HttpGet("{genreId}/songs/{id}")]
        public async Task<IActionResult> AlbumDetailsOneSong(int genreId, int id)
        {
            var genreDetails = await (
               from list in _dbContext.Genres
               select new
               {
                   Id = list.Id,
                   Songs = list.Songs
               }).ToListAsync();


            var songs = genreDetails
                .Where(genre => genre.Id == genreId)
                // sprawdz czy jest puste , jezeli tak to []
                .Select(genre => new { Songs = genre.Songs })
                .First()
                .Songs
                .Where(song => song.Id == id);
            return Ok(songs);
        }
    }
}
