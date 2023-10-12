using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Reactivities.Application.Core;
using Reactivities.Domain;
using Reactivities.Persistence;

namespace Reactivities.Application.Activities
{
    public partial class Activities
    {
        public partial class Queries
        {
            public class GetAll : IRequest<Result<List<ActivityDTO>>>
            {
            }

            private class GetAllHandler : IRequestHandler<GetAll, Result<List<ActivityDTO>>>
            {
                private readonly DataContext _context;
                private readonly IMapper _mapper;

                public GetAllHandler(DataContext context, IMapper mapper)
                {
                    _context = context;
                    _mapper = mapper;
                }

                public async Task<Result<List<ActivityDTO>>> Handle(GetAll request, CancellationToken cancellationToken)
                {
                    var activities = await _context.Activities.ProjectTo<ActivityDTO>(_mapper.ConfigurationProvider).ToListAsync();

                    // var result = _mapper.Map<List<ActivityDTO>>(activities);
                    

                    return Result<List<ActivityDTO>>.Success(activities);
                }
            }
        }
    }
}
