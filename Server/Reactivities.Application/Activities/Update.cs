using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Reactivities.Application.Core;
using Reactivities.Domain;
using Reactivities.Persistence;

namespace Reactivities.Application.Activities
{
    public partial class Activities
    {
        public partial class Commands
        {
            public class Update : Activity, IRequest<Result<Unit>>
            {
            }

            private class UpdateHandler : IRequestHandler<Update, Result<Unit>>
            {
                private readonly DataContext _context;
                private readonly IMapper _mapper;

                public UpdateHandler(DataContext context, IMapper mapper)
                {
                    _context = context;
                    _mapper = mapper;
                }

                public async Task<Result<Unit>> Handle(Update request, CancellationToken cancellationToken)
                {
                    var activity = await _context.Activities.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

                    if (activity == null) return null;

                    _mapper.Map(request, activity);

                    _context.Activities.Update(activity);
                    var result = await _context.SaveChangesAsync(cancellationToken) > 0;

                    if (!result) return Result<Unit>.Failure("Failed to update activity");

                    return Result<Unit>.Success(Unit.Value);
                }
            }
        }
    }
}
