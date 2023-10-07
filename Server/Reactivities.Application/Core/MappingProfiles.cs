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
            CreateMap<Activity, ActivityDTO>()
                .ForMember(
                    activityDTO => activityDTO.HostUsername, 
                    opt => opt.MapFrom(
                        activity => activity.Attendees.FirstOrDefault(activityAttendee => activityAttendee.IsHost).ApplicationUser.UserName
                    )
                );
            CreateMap<ActivityAttendee, Profiles.Profile>()
                .ForMember(profile => profile.DisplayName, opt => opt.MapFrom(src => src.ApplicationUser.DisplayName))
                .ForMember(profile => profile.Username, opt => opt.MapFrom(src => src.ApplicationUser.UserName))
                .ForMember(profile => profile.Bio, opt => opt.MapFrom(src => src.ApplicationUser.Bio));
        }
    }
}
