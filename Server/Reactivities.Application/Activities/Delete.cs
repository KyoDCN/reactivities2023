using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Reactivities.Application.Core;
using Reactivities.Domain;
using Reactivities.Persistence;

namespace Reactivities.Application.Activities
{
    public partial class Activities
    {
        public partial class Commands
        {
            public class Delete : Activity, IRequest<Result<Unit>>
            {
            }

            private class DeleteHandler : IRequestHandler<Delete, Result<Unit>>
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

                public async Task<Result<Unit>> Handle(Delete request, CancellationToken cancellationToken)
                {
                    var activity = await _context.Activities.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

                    if (activity == null) return null;

                    _context.Activities.Remove(activity);

                    var result = await _context.SaveChangesAsync(cancellationToken) > 0;

                    if (!result) return Result<Unit>.Failure("Failed to delete the activity");

                    return Result<Unit>.Success(Unit.Value);
                }
            }
        }
    }
}
