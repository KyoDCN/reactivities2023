using AutoMapper;
using Reactivities.Domain;

namespace Reactivities.Application.Core
{
    internal class MappingProfiles : Profile
    {
        public MappingProfiles() 
        { 
            CreateMap<Activity, Activity>(); 
        }
    }
}
