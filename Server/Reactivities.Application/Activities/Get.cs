using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Reactivities.Core;
using Reactivities.Domain;
using Reactivities.Persistence;

namespace Reactivities.Application.Activities
{
    public partial class Activities
    {
        public partial class Queries
        {
            public class Get : IRequest<Result<ActivityDTO>>
            {
                public Guid Id { get; set; }
            }

            private class GetHandler : IRequestHandler<Get, Result<ActivityDTO>>
            {
                private readonly DataContext _context;
                private readonly IMapper _mapper;

                public GetHandler(DataContext context, IMapper mapper)
                {
                    _context = context;
                    _mapper = mapper;
                }

                public async Task<Result<ActivityDTO>> Handle(Get request, CancellationToken cancellationToken)
                {
                    var activity = await _context.Activities
                        .ProjectTo<ActivityDTO>(_mapper.ConfigurationProvider)
                        .FirstOrDefaultAsync(x => x.Id == request.Id);

                    return Result<ActivityDTO>.Success(activity);
                }
            }
        }
    }
}
