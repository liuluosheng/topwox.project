using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace WebService.Core
{
    public static class AutoMapperConfig
    {
        //添加你的实体映射关系.
        public static void MapperConfig()
        {
           // Mapper.Initialize(cfg => { cfg.CreateMap<TestUser, User>().ReverseMap(); });
        }
    }
}
