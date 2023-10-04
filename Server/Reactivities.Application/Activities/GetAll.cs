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
            public class GetAll : IRequest<Result<List<Activity>>>
            {
            }

            private class GetAllHandler : IRequestHandler<GetAll, Result<List<Activity>>>
            {
                private readonly DataContext _context;

                public GetAllHandler(DataContext context)
                {
                    _context = context;
                }

                async Task<Result<List<Activity>>> IRequestHandler<GetAll, Result<List<Activity>>>.Handle(GetAll request, CancellationToken cancellationToken)
                {
                    return Result<List<Activity>>.Success(await _context.Activities.ToListAsync(cancellationToken));
                }
            }
        }
    }
}
