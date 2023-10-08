using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reactivities.Domain;

namespace Reactivities.Persistence.EntityTypeConfigurations
{
    public class ActivityAttendeeEntityTypeConfiguration : IEntityTypeConfiguration<ActivityAttendee>
    {
        public void Configure(EntityTypeBuilder<ActivityAttendee> builder)
        {
            builder
                .HasKey(a => new { a.ApplicationUserId, a.ActivityId });

            builder
                .HasOne(x => x.ApplicationUser)
                .WithMany(x => x.Activities)
                .HasForeignKey(x => x.ApplicationUserId);

            builder
                .HasOne(x => x.Activity)
                .WithMany(x => x.Attendees)
                .HasForeignKey(x => x.ActivityId);
        }
    }
}
