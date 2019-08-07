using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;

namespace Dictum.Business
{
    public class Mapper
    {
        private static readonly Lazy<Mapper> Instance = new Lazy<Mapper>(() => new Mapper());
        private readonly IMapper _mapper;

        private Mapper()
        {
            var currentAssemblyName = GetType().Assembly.FullName;
            var assemblyPrefix =
                currentAssemblyName.Substring(0, currentAssemblyName.IndexOf(".", StringComparison.Ordinal));
            var assembliesToScan = AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(x => x.FullName.StartsWith(assemblyPrefix));

            var config = new MapperConfiguration(cfg => cfg.AddMaps(assembliesToScan));
            config.CompileMappings();

            _mapper = config.CreateMapper();
        }


        public static TDestination Map<TSource, TDestination>(TSource source)
        {
            return Instance.Value._mapper.Map<TSource, TDestination>(source);
        }

        public static IEnumerable<TDestination> MapCollection<TSource, TDestination>(IEnumerable<TSource> source)
        {
            return Instance.Value._mapper.Map<IEnumerable<TSource>, IEnumerable<TDestination>>(source);
        }
    }
}