using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Reactivities.Application.Core;
using Reactivities.Persistence;

namespace Reactivities.Application.Profiles
{
    public partial class Profiles
    {
        public partial class Queries
        {
            public class Details : IRequest<Result<Profile>>
            {
                public string UserName { get; set; }
            }

            public class DetailsHandler : IRequestHandler<Details, Result<Profile>>
            {
                private readonly DataContext _context;
                private readonly IMapper _mapper;

                public DetailsHandler(DataContext context, IMapper mapper)
                {
                    _context = context;
                    _mapper = mapper;
                }

                public async Task<Result<Profile>> Handle(Details request, CancellationToken cancellationToken)
                {
                    var user = await _context.Users.ProjectTo<Profile>(_mapper.ConfigurationProvider).SingleOrDefaultAsync(x => x.Username == request.UserName);

                    if (user == null) return null;

                    return Result<Profile>.Success(user);
                }
            }
        }
    }
}
