using AutoMapper;
using System;
using System.Linq;
using System.Reflection;

namespace StarterApp.Core.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
        }

        private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            var types = assembly.GetExportedTypes()
                .Where(t => t.GetInterfaces().Any(i =>
                    i.IsGenericType && (i.GetGenericTypeDefinition() == typeof(IMapFrom<>)
                        || i.GetGenericTypeDefinition() == typeof(IMapTo<>)
                        || i.GetGenericTypeDefinition() == typeof(IMap<>))
                    ))
                .ToList();

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);

                var interfaceTypes = type.GetInterfaces();

                foreach (var interfaceType in interfaceTypes)
                {
                    if (!interfaceType.IsGenericType) continue;

                    var interfaceMethod = interfaceType.GetGenericTypeDefinition() == typeof(IMapFrom<>)
                        || interfaceType.GetGenericTypeDefinition() == typeof(IMapTo<>)
                        || interfaceType.GetGenericTypeDefinition() == typeof(IMap<>) ? interfaceType.GetMethod("Mapping") : null;
                    interfaceMethod?.Invoke(instance, new object[] { this });
                }

                var method = type.GetMethod("Mapping");
                method?.Invoke(instance, new object[] { this });
            }
        }
    }
}