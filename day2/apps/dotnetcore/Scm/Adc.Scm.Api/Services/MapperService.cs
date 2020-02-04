using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Adc.Scm.Api.Services
{
    public class MapperService
    {
        private static readonly Lazy<IMapper>  Mapper = new Lazy<IMapper>(CreateMapper);

        private static IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DomainObjects.Contact, Models.Contact>();
                cfg.CreateMap<Models.Contact, DomainObjects.Contact>();
            });

            return config.CreateMapper();
        }

        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return Mapper.Value.Map<TSource, TDestination>(source);
        }

        public List<TDestination> Map<TSource, TDestination>(List<TSource> source)
        {
            // LINQ looks more sexy
            return source.Select(s => Mapper.Value.Map<TDestination>(s)).ToList();
        }
    }
}
