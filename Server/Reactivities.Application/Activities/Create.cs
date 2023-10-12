using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Reactivities.Application.Core;
using Reactivities.Application.Interfaces;
using Reactivities.Domain;
using Reactivities.Persistence;

namespace Reactivities.Application.Activities
{
    public partial class Activities
    {
        public partial class Commands
        {
            public class Create : Activity, IRequest<Result<Unit>>
            { 
            }

            public class CreateValidator : AbstractValidator<Create>
            {
                public CreateValidator()
                {
                    RuleFor(x => x).SetValidator(new ActivityValidator());
                }
            }

            private class CreateHandler : IRequestHandler<Create, Result<Unit>>
            {
                private readonly DataContext _context;
                private readonly IUserAccessor _userAccessor;

                public CreateHandler(DataContext context, IUserAccessor userAccessor)
                {
                    _context = context;
                    _userAccessor = userAccessor;
                }

                public async Task<Result<Unit>> Handle(Create activity, CancellationToken cancellationToken)
                {
                    var user = await _context.Users.FirstAsync(x => x.UserName == _userAccessor.GetUserName(), cancellationToken);

                    var attendee = new ActivityAttendee
                    {
                        ApplicationUser = user,
                        Activity = activity,
                        IsHost = true,
                    };

                    activity.Attendees.Add(attendee);

                    await _context.Activities.AddAsync(activity, cancellationToken);

                    var result = await _context.SaveChangesAsync(cancellationToken) > 0;

                    if (!result) return Result<Unit>.Failure("Failed to create activity");

                    return Result<Unit>.Success(Unit.Value);
                }
            }
        }
    }
}
