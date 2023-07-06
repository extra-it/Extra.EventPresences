using AutoMapper;
using Azure;
using Extra.EventPresences.DTO;
using Extra.EventPresences.DTO.Dto;
using Extra.EventPresences.Model.Entities;
using Extra.EventPresences.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extra.EventPresences.Middleware.Managers
{
    public class MapperManager
    {
        static IMapper mapper = null;
        public static IMapper GetMapper()
        {
            if (mapper == null)
            {
                var config = new MapperConfiguration(cfg =>
                {
                    MapLogs(ref cfg);
                    MapDto(ref cfg);
                });

                mapper = new AutoMapper.Mapper(config);
            }
            return mapper;
        }

        private static void MapLogs(ref IMapperConfigurationExpression cfg)
        {
            //Read
            cfg.CreateMap<WebApiLog, WebApiLogDto>();
            ////Write
            cfg.CreateMap<WebApiLogDto, WebApiLog>();
            cfg.CreateMap<WebApiLogDto, CustomHeaderDto>();
        }

        private static void MapDto(ref IMapperConfigurationExpression cfg)
        {
            //Read

            cfg.CreateMap<User, UserDto>();
            cfg.CreateMap<EventFunctionality, EventFunctionalityDto>().ForMember(dest => dest.FunctionalityID, source => source.MapFrom(src => GetMapper().Map<int,eFunctionality>((int)src.FunctionalityID)));
            cfg.CreateMap<EventFunctionalityUser, EventFunctionalityUserDto>();
            //               .ForMember(dest => dest.SaleStatusID, source => source.MapFrom(src => GetMapper().Map<int, eSaleStatus>((int)src.SaleStatusID)))
            //               .ForMember(src => src.Password, dest => dest.Ignore());
            //cfg.CreateMap<Operator, OperatorDto>();
            //cfg.CreateMap<Statistic, StatisticDto>()
            //               .ForMember(dest => dest.AppActivityID, source => source.MapFrom(src => GetMapper().Map<int, eAppActivity>((int)src.AppActivityID)));

            ////Write
            cfg.CreateMap<UserDto, User>()
                .ForMember(dest=>dest.ID, dest => dest.Ignore());

            cfg.CreateMap<FunctionalityDto, Functionality>();
            cfg.CreateMap<EventFunctionalityUserDto, EventFunctionalityUser>();
            cfg.CreateMap<EventFunctionalityDto, EventFunctionality>().ForMember(dest => dest.FunctionalityID, source => source.MapFrom(src => GetMapper().Map<eFunctionality, int>((eFunctionality)src.FunctionalityID)));
            

            //    .ForMember(dest => dest.SaleStatusID, source => source.MapFrom(src => GetMapper().Map<eSaleStatus, int>((eSaleStatus)src.SaleStatusID)));
            //cfg.CreateMap<OperatorDto, Operator>();
            //cfg.CreateMap<StatisticDto, Statistic>()
            //    .ForMember(dest => dest.AppActivityID, source => source.MapFrom(src => GetMapper().Map<eAppActivity, int>((eAppActivity)src.AppActivityID))); 


        }
    }
}
