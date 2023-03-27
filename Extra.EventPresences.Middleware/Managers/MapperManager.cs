using AutoMapper;
using Azure;
using Extra.EventPresences.DTO.Dto;
using Extra.EventPresences.Model.Entities;
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
            //               .ForMember(dest => dest.SaleStatusID, source => source.MapFrom(src => GetMapper().Map<int, eSaleStatus>((int)src.SaleStatusID)))
            //               .ForMember(src => src.Password, dest => dest.Ignore());
            //cfg.CreateMap<Operator, OperatorDto>();
            //cfg.CreateMap<Statistic, StatisticDto>()
            //               .ForMember(dest => dest.AppActivityID, source => source.MapFrom(src => GetMapper().Map<int, eAppActivity>((int)src.AppActivityID)));

            ////Write
            cfg.CreateMap<UserDto, User>()
                .ForMember(dest=>dest.ID, dest => dest.Ignore());
            //    .ForMember(dest => dest.SaleStatusID, source => source.MapFrom(src => GetMapper().Map<eSaleStatus, int>((eSaleStatus)src.SaleStatusID)));
            //cfg.CreateMap<OperatorDto, Operator>();
            //cfg.CreateMap<StatisticDto, Statistic>()
            //    .ForMember(dest => dest.AppActivityID, source => source.MapFrom(src => GetMapper().Map<eAppActivity, int>((eAppActivity)src.AppActivityID))); ;


        }
    }
}
