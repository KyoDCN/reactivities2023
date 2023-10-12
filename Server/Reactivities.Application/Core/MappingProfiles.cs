using AutoMapper;
using Reactivities.Application.Activities;
using Reactivities.Domain;

namespace Reactivities.Application.Core
{
    internal class MappingProfiles : Profile
    {
        public MappingProfiles() 
        { 
            CreateMap<Activity, Activity>();

            CreateMap<ActivityAttendee, AttendeeDTO>()
                .ForMember(profile => profile.DisplayName, opt => opt.MapFrom(src => src.ApplicationUser.DisplayName))
                .ForMember(profile => profile.Username, opt => opt.MapFrom(src => src.ApplicationUser.UserName))
                .ForMember(profile => profile.Bio, opt => opt.MapFrom(src => src.ApplicationUser.Bio))
                .ForMember(profile => profile.Image, o => o.MapFrom(src => src.ApplicationUser.Photos.FirstOrDefault(x => x.IsMain).Url));

            CreateMap<Activity, ActivityDTO>()
                .ForMember(d => d.HostUsername, o => o.MapFrom(s => s.Attendees.FirstOrDefault(x => x.IsHost).ApplicationUser.UserName));

            CreateMap<ApplicationUser, Profiles.Profile>()
                .ForMember(d => d.Image, o => o.MapFrom(s => s.Photos.FirstOrDefault(x => x.IsMain).Url));
        }
    }
}
