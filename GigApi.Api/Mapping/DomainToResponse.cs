using AutoMapper;
using GigApi.Api.V1;
using GigApi.Api.V1.Authentication;
using GigApi.Api.V1.Playlists;
using GigApi.Api.V1.Songs;
using GigApi.Application.Services.Authentication;
using GigApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GigApi.Api.Mapping
{
    public class DomainToResponse : Profile
    {
        public DomainToResponse()
        {
            CreateMap<Song, SongResponse>();

            CreateMap<Playlist, PlaylistResponse>()
                .ForMember(dest => dest.Songs, map => map.MapFrom(src => 
                    src.PlaylistSongs.Select(x => 
                        x.Song)));

            CreateMap<PlaylistSong, SongResponse>();

            CreateMap<AuthenticationResult, AuthenticationResponse>();

            CreateMap<AuthenticationResult, ErrorResponse>();
        }
    }
}
