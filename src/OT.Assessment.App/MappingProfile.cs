using AutoMapper;
using OT.Assessment.Model.Dto;
using OT.Assessment.Model.Entities;
using OT.Assessment.Model.Request;

namespace OT.Assessment.App
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<GameCreateRequest, Game>();
            CreateMap<PlayerCreateRequest, Player>();

            CreateMap<Provider, ProviderDto>();
            CreateMap<Game, GameDto>();
        }
    }
}
