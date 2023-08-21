using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Reactivities.Domain;
using Reactivities.Persistence;

namespace Reactivities.Application.Activities
{
    public partial class Activities
    {
        public partial class Commands
        {
            public class Update : Activity, IRequest
            {
            }

            private class UpdateHandler : IRequestHandler<Update>
            {
                private readonly DataContext _context;
                private readonly IMapper _mapper;

                public UpdateHandler(DataContext context, IMapper mapper)
                {
                    _context = context;
                    _mapper = mapper;
                }

                public async Task Handle(Update request, CancellationToken cancellationToken)
                {
                    var activity = await _context.Activities.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

                    if (activity == null)
                    {
                        return;
                    }

                    _mapper.Map(request, activity);

                    _context.Activities.Update(activity);
                    await _context.SaveChangesAsync(cancellationToken);
                }
            }
        }
    }
}
