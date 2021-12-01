using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicAPI.Data;
using MusicAPI.DTOs.ArtistDTO;
using MusicAPI.Models;

namespace MusicAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private ApiDbContext _dbContext;
        public ArtistsController(IMapper mapper, ApiDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ArtistDto artist)
        {
            await _dbContext.Artists.AddAsync(_mapper.Map<Artist>(artist));
            await _dbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);
        }

        //api/artists
        [HttpGet]
        public async Task<IActionResult> GetArtists()
        {
            var artists = await (from artist in _dbContext.Artists
                                 select new
                                 {
                                     Id = artist.Id,
                                     Name = artist.Name,
                                     Surname = artist.Surname
                                 }).ToListAsync();

            return Ok(artists);
        }

        // PUT api/<ArtistController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ArtistDto artistObj)
        {
            var artist = await _dbContext.Artists.FindAsync(id);
            if (artist == null)
            {
                return NotFound("No artist found against this Id");
            }
            else
            {
                artist.Name = artistObj.Name;
                artist.Surname = artistObj.Surname;
                await _dbContext.SaveChangesAsync();
                return Ok("Artist updated sucessfully");
            }
        }

        // DELETE api/<ArtistsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var artist = _dbContext.Artists.Find(id);
            _dbContext.Artists.Remove(artist);
            await _dbContext.SaveChangesAsync();
            return Ok("Artist deleted successfully");
        }
    }
}
