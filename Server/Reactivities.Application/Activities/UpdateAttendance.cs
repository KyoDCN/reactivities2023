using MediatR;
using Microsoft.EntityFrameworkCore;
using Reactivities.Application.Interfaces;
using Reactivities.Core;
using Reactivities.Domain;
using Reactivities.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reactivities.Application.Activities
{
    public partial class Activities
    {
        public partial class Commands
        {
            public class UpdateAttendance : IRequest<Result<Unit>>
            {
                public Guid Id { get; set; }
            }

            public class UpdateAttendanceHandler : IRequestHandler<UpdateAttendance, Result<Unit>>
            {
                private readonly DataContext _context;
                private readonly IUserAccessor _userAccessor;

                public UpdateAttendanceHandler(DataContext context, IUserAccessor userAccessor)
                {
                    _context = context;
                    _userAccessor = userAccessor;
                }

                public async Task<Result<Unit>> Handle(UpdateAttendance request, CancellationToken cancellationToken)
                {
                    var activity = await _context.Activities
                        .Include(a => a.Attendees)
                        .ThenInclude(u => u.ApplicationUser)
                        .FirstOrDefaultAsync(x => x.Id == request.Id);

                    if (activity == null) return null;

                    var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUserName());

                    if(user == null) return null;

                    var hostUsername = activity.Attendees.FirstOrDefault(x => x.IsHost).ApplicationUser.UserName;

                    var attendance = activity.Attendees.FirstOrDefault(x => x.ApplicationUser.UserName == user.UserName);

                    if (attendance != null)
                    {
                        if(hostUsername == user.UserName)
                            activity.IsCancelled = !activity.IsCancelled;
                        else if(hostUsername != user.UserName)
                            activity.Attendees.Remove(attendance);
                    }

                    if (attendance == null)
                    {
                        attendance = new ActivityAttendee
                        {
                            ApplicationUser = user,
                            Activity = activity,
                            IsHost = false
                        };

                        activity.Attendees.Add(attendance);
                    }

                    var result = await _context.SaveChangesAsync() > 0;

                    return result ? Result<Unit>.Success(Unit.Value) : Result<Unit>.Failure("Problem updating attendance");
                }
            }
        }
    }
}
