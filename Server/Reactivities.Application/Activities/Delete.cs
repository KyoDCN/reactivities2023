using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Reactivities.Domain;
using Reactivities.Persistence;

namespace Reactivities.Application.Activities
{
    public partial class Activities
    {
        public partial class Commands
        {
            public class Delete : Activity, IRequest
            {
            }

            private class DeleteHandler : IRequestHandler<Delete>
            {
                private readonly DataContext _context;
                private readonly IMapper _mapper;
                private readonly ILogger<Delete> _logger;

                public DeleteHandler(DataContext context, IMapper mapper, ILogger<Delete> logger)
                {
                    _context = context;
                    _mapper = mapper;
                    _logger = logger;
                }

                public async Task Handle(Delete request, CancellationToken cancellationToken)
                {
                    var activity = await _context.Activities.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

                    if (activity == null)
                    {
                        return;
                    }

                    _context.Activities.Remove(activity);

                    await _context.SaveChangesAsync(cancellationToken);
                }
            }
        }
    }
}
