using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicAPI.Data;
using MusicAPI.DTOs.GenreDTO;
using MusicAPI.Models;

namespace MusicAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IMapper _mapper;
        private ApiDbContext _dbContext;
        public GenresController(IMapper mapper, ApiDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GenreDto genre)
        {
            await _dbContext.Genres.AddAsync(_mapper.Map<Genre>(genre));
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
                    Name = list.Name
                }
                ).Where(list => list.Id == genreId).ToListAsync();
            return Ok(genreDetails);
        }

        //api/genres/{albumId}/songs
        [HttpGet("{genreId}/songs")]
        public async Task<IActionResult> AlbumsDetailsSongs(int genreId)
        {
            var genreDetails = await (
                from list in _dbContext.Genres
                select new
                {
                    Id = list.Id,
                    Songs = list.Songs
                }
                ).Where(list => list.Id == genreId).ToListAsync();
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

        // PUT api/<GenreController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] GenreDto genreObj)
        {
            var genre = await _dbContext.Genres.FindAsync(id);
            if (genre == null)
            {
                return NotFound("No genre found against this Id");
            }
            else
            {
                genre.Name = genreObj.Name;
                await _dbContext.SaveChangesAsync();
                return Ok("Genre updated sucessfully");
            }
        }

        // DELETE api/<GenresController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var genre = _dbContext.Genres.Find(id);
            _dbContext.Genres.Remove(genre);
            await _dbContext.SaveChangesAsync();
            return Ok("Genre deleted successfully");
        }
    }
}
