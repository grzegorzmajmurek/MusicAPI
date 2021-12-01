using System;
using System.Collections.Generic;
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
    [Route("api/[controller]")]
    [ApiController]
    public class SongsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private ApiDbContext _dbContext;
        public SongsController(IMapper mapper, ApiDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SongDto song)
        {
            Song songObj = new Song();
            songObj.UploadedDate = DateTime.Now;
            await _dbContext.Songs.AddAsync(_mapper.Map<Song>(song));
            await _dbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);
        }

        //api/songs
        [HttpGet]
        public async Task<IActionResult> GetSongs()
        {
            var songs = await (from song in _dbContext.Songs
                               select new
                               {
                                   Id = song.Id,
                                   Title = song.Title,
                                   Duration = song.Duration,
                                   AlbumId = song.AlbumId,
                                   ArtistId = song.ArtistId,
                                   GenreId = song.GenreId,
                                   PlaylistId = song.PlaylistId
                               }).ToListAsync();

            return Ok(songs);
        }

        // PUT api/<SongsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] SongDto songObj)
        {
            var song = await _dbContext.Songs.FindAsync(id);
            if (song == null)
            {
                return NotFound("No record found against this Id");
            }
            else
            {
                song.Title = songObj.Title;
                song.Duration = songObj.Duration;
                song.ArtistId = songObj.ArtistId;
                song.AlbumId = songObj.AlbumId;
                song.GenreId = songObj.GenreId;
                song.PlaylistId = songObj.PlaylistId;
                await _dbContext.SaveChangesAsync();
                return Ok("Record updated sucessfully");
            }
        }

        // DELETE api/<SongsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var song = _dbContext.Songs.Find(id);
            _dbContext.Songs.Remove(song);
            await _dbContext.SaveChangesAsync();
            return Ok("Record deleted successfully");
        }
    }
}
