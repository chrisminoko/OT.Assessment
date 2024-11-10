using AutoMapper;
using OT.Assessment.Model.Entities;
using OT.Assessment.Model.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OT.Assessment.Core.Mapper
{
    public class MappingProfile  : Profile
    {
        public MappingProfile()
        {
            CreateMap<GameCreateRequest, Game>();
            CreateMap<PlayerCreateRequest, Player>();
        }
    }
}
