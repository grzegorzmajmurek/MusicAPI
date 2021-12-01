using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicAPI.DTOs.SongDTO
{
    public class SongDto
    {
        public string Title { get; set; }
        public string Duration { get; set; }
        public int ArtistId { get; set; }
        public int AlbumId { get; set; }
        public int GenreId { get; set; }
        public int PlaylistId { get; set; }
    }
}
