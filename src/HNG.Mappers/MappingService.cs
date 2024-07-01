using AutoMapper;
using HNG.Abstractions.Services.Infrastructure;

namespace HNG.Mappers
{
    public class MappingService : IMappingService
    {
        readonly IMapper Mapper;

        public MappingService()
        {
            //scan for all mapping profiles in this assembly
            var config = new MapperConfiguration(cfg => {
                cfg.AddMaps(typeof(MappingService));
            });

            //add config to the mapper
            Mapper = new Mapper(config);
        }

        public TDestination Map<TDestination>(object source)
        {
            return Mapper.Map<TDestination>(source);
        }

        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return Mapper.Map<TSource, TDestination>(source);
        }

        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            return Mapper.Map<TSource, TDestination>(source, destination);
        }

        public object Map(object source, Type sourceType, Type destinationType)
        {
            return Mapper.Map(source, sourceType, destinationType);
        }

        public object Map(object source, object destination, Type sourceType, Type destinationType)
        {
            return Mapper.Map(source, destination, sourceType, destinationType);
        }
    }
}
