using AutoMapper;

namespace StarterApp.Core.Common.Mappings
{
    public interface IMapTo<T>
    {
        void Mapping(Profile profile) => profile.CreateMap(GetType(), typeof(T))
             .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
