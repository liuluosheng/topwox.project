using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Data.Dto;

namespace Ew.Api.Core
{
    public static class AutoMapperConfig
    {
        //添加你的实体映射关系.
        public static void InitAutoMapperConfig()
        {
           // Mapper.Initialize(cfg => { cfg.CreateMap<TestUser, User>().ReverseMap(); });
        }
    }
}
