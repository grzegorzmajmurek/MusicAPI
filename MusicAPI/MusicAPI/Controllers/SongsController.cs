using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicAPI.Models;

namespace MusicAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongsController : ControllerBase
    {
        private static List<Song> songs = new List<Song>()
        {
            new Song(){ Id = 0, Title = "Bo jestes Ty", Language = "Polish" },
            new Song(){ Id = 1, Title = "Chcialem Byc", Language = "Polish" }
        };

        [HttpGet]
        public IEnumerable<Song> Get()
        {
            return songs;
        }

        [HttpPost]
        public void Post([FromBody]Song song)
        {
            songs.Add(song);
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody]Song song)
        {
            songs[id] = song;
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            songs.RemoveAt(id);
        }
    }
}
