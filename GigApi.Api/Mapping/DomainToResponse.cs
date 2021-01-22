using AutoMapper;
using GigApi.Api.V1.Songs;
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
        }
    }
}
