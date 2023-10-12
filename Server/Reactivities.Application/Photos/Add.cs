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
            public class Add : IRequest<Result<Photo>>
            {
                public IFormFile File { get; set; }
            }

            public class AddHander : IRequestHandler<Add, Result<Photo>>
            {
                private readonly DataContext _context;
                private readonly IPhotoAccessor _photoAccessor;
                private readonly IUserAccessor _userAccessor;

                public AddHander(DataContext context, IPhotoAccessor photoAccessor, IUserAccessor userAccessor)
                {
                    _context = context;
                    _photoAccessor = photoAccessor;
                    _userAccessor = userAccessor;
                }

                public async Task<Result<Photo>> Handle(Add request, CancellationToken cancellationToken)
                {
                    var user = await _context.Users
                        .Include(x => x.Photos)
                        .FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUserName(), cancellationToken);

                    if (user == null) return null;

                    var photoUploadResult = await _photoAccessor.AddPhoto(request.File);

                    var photoToAdd = new Photo
                    {
                        Url = photoUploadResult.Url.ToString(),
                        Id = photoUploadResult.PublicId
                    };

                    // Set the photo as Main if there are currently zero photos in the user's collection
                    if(!user.Photos.Any(x => x.IsMain)) photoToAdd.IsMain = true;

                    user.Photos.Add(photoToAdd);

                    var result = await _context.SaveChangesAsync(cancellationToken) > 0;

                    if(result) return Result<Photo>.Success(photoToAdd);

                    return Result<Photo>.Failure("Problem adding photo");
                }
            }
        }
    }
}
