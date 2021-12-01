using AutoMapper;
using MusicAPI.DTOs.AlbumDTO;
using MusicAPI.DTOs.ArtistDTO;
using MusicAPI.DTOs.GenreDTO;
using MusicAPI.DTOs.SongDTO;
using MusicAPI.Models;

namespace MusicAPI.Mapping
{
    public static class AutoMapperConfig
    {
        public static IMapper Initialize()
            => new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PlaylistDto, Playlist>();
                cfg.CreateMap<SongDto, Song>();
                cfg.CreateMap<GenreDto, Genre>();
                cfg.CreateMap<AlbumDto, Album>();
                cfg.CreateMap<ArtistDto, Artist>();
            })
            .CreateMapper();
    }
}
