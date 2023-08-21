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
            public class GetAll : IRequest<List<Activity>>
            {
            }

            private class GetAllHandler : IRequestHandler<GetAll, List<Activity>>
            {
                private readonly DataContext _context;

                public GetAllHandler(DataContext context)
                {
                    _context = context;
                }

                async Task<List<Activity>> IRequestHandler<GetAll, List<Activity>>.Handle(GetAll request, CancellationToken cancellationToken)
                {
                    return await _context.Activities.ToListAsync(cancellationToken);
                }
            }
        }
    }
}
