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
            public class Get : IRequest<Result<Activity>>
            {
                public Guid Id { get; set; }
            }

            private class GetHandler : IRequestHandler<Get, Result<Activity>>
            {
                private readonly DataContext _context;

                public GetHandler(DataContext context)
                {
                    _context = context;
                }

                public async Task<Result<Activity>> Handle(Get request, CancellationToken cancellationToken)
                {
                    Activity activity = await _context.Activities.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
                    return Result<Activity>.Success(activity);
                }
            }
        }
    }
}
