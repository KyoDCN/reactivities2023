using MediatR;
using Reactivities.Domain;
using Reactivities.Persistence;

namespace Reactivities.Application.Activities
{
    public partial class Activities
    {
        public partial class Commands
        {
            public class Create : Activity, IRequest
            { 
            }

            private class CreateHandler : IRequestHandler<Create>
            {
                private readonly DataContext _context;

                public CreateHandler(DataContext context)
                {
                    _context = context;
                }

                public async Task Handle(Create request, CancellationToken cancellationToken)
                {
                    await _context.Activities.AddAsync(request, cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);
                }
            }
        }
    }
}
