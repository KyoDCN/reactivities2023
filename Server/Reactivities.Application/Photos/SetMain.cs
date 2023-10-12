using MediatR;
using Microsoft.EntityFrameworkCore;
using Reactivities.Application.Core;
using Reactivities.Application.Interfaces;
using Reactivities.Persistence;

namespace Reactivities.Application.Photos
{
    public partial class Photos
    {
        public partial class Commands
        {
            public class SetMain : IRequest<Result<Unit>>
            {
                public string Id { get; set; }
            }

            public class SetMainHandler : IRequestHandler<SetMain, Result<Unit>>
            {
                private readonly DataContext _context;
                private readonly IUserAccessor _userAccessor;

                public SetMainHandler(DataContext context, IUserAccessor userAccessor)
                {
                    _context = context;
                    _userAccessor = userAccessor;
                }

                public async Task<Result<Unit>> Handle(SetMain request, CancellationToken cancellationToken)
                {
                    var user = await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUserName());

                    if (user == null) return null;

                    var photo = user.Photos.FirstOrDefault(x => x.Id == request.Id);

                    if(photo == null) return null;

                    var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);

                    if(currentMain != null) currentMain.IsMain = false;

                    photo.IsMain = true;

                    var success = await _context.SaveChangesAsync(cancellationToken) > 0;

                    if (success) return Result<Unit>.Success(Unit.Value);

                    return Result<Unit>.Failure("Problem setting main photo.");
                }
            }
        }
    }
}
