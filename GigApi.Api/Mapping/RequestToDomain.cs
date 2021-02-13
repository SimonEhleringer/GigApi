using AutoMapper;
using GigApi.Api.V1.Playlists;
using GigApi.Api.V1.Songs;
using GigApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GigApi.Api.Mapping
{
    public class RequestToDomain : Profile
    {
        public RequestToDomain()
        {
            CreateMap<CreateUpdateSongRequest, Song>();

            CreateMap<CreateUpdatePlaylistRequest, Playlist>()
                .ForMember(x => x.PlaylistSongs, map => map.MapFrom(source => 
                    source.SongIds.ToList().Select((x, index) => 
                        new PlaylistSong
                        {
                            SongId = x,
                            IndexInPlaylist = index
                        })));
        }
    }
}
