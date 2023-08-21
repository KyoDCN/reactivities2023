using MediatR;
using Microsoft.EntityFrameworkCore;
using Reactivities.Domain;
using Reactivities.Persistence;

namespace Reactivities.Application.Activities
{
    public partial class Activities
    {
        public partial class Queries
        {
            public class Get : IRequest<Activity>
            {
                public Guid Id { get; set; }
            }

            private class GetHandler : IRequestHandler<Get, Activity>
            {
                private readonly DataContext _context;

                public GetHandler(DataContext context)
                {
                    _context = context;
                }

                public async Task<Activity> Handle(Get request, CancellationToken cancellationToken)
                {
                    return await _context.Activities.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
                }
            }
        }
    }
}
