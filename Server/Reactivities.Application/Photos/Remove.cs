using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Reactivities.Application.Core;
using Reactivities.Application.Interfaces;
using Reactivities.Domain;
using Reactivities.Persistence;

namespace Reactivities.Application.Photos
{
    public partial class Photos
    {
        public partial class Commands
        {
            public class Remove : IRequest<Result<Unit>>
            {
                public string Id { get; set; }
            }

            public class RemoveHander : IRequestHandler<Remove, Result<Unit>>
            {
                private readonly DataContext _context;
                private readonly IPhotoAccessor _photoAccessor;
                private readonly IUserAccessor _userAccessor;

                public RemoveHander(DataContext context, IPhotoAccessor photoAccessor, IUserAccessor userAccessor)
                {
                    _context = context;
                    _photoAccessor = photoAccessor;
                    _userAccessor = userAccessor;
                }

                public async Task<Result<Unit>> Handle(Remove request, CancellationToken cancellationToken)
                {
                    var user = await _context.Users.Include(x => x.Photos).FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUserName(), cancellationToken: cancellationToken);

                    if (user == null) return null;

                    var photoDeleteResult = await _photoAccessor.DeletePhoto(request.Id);

                    if(photoDeleteResult == null) return Result<Unit>.Failure("Problem deleting photo in Cloudinary");

                    var photoToDelete = user.Photos.FirstOrDefault(x => x.Id == request.Id);
                    user.Photos.Remove(photoToDelete);
                    var result = await _context.SaveChangesAsync(cancellationToken) > 0;

                    if(result) return Result<Unit>.Success(Unit.Value);

                    return Result<Unit>.Failure("Problem deleting photo");
                }
            }
        }
    }
}
