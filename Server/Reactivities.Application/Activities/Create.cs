using FluentValidation;
using MediatR;
using Reactivities.Core;
using Reactivities.Domain;
using Reactivities.Persistence;

namespace Reactivities.Application.Activities
{
    public partial class Activities
    {
        public partial class Commands
        {
            public class Create : Activity, IRequest<Result<Unit>>
            { 
            }

            public class CreateValidator : AbstractValidator<Create>
            {
                public CreateValidator()
                {
                    RuleFor(x => x).SetValidator(new ActivityValidator());
                }
            }

            private class CreateHandler : IRequestHandler<Create, Result<Unit>>
            {
                private readonly DataContext _context;

                public CreateHandler(DataContext context)
                {
                    _context = context;
                }

                public async Task<Result<Unit>> Handle(Create request, CancellationToken cancellationToken)
                {
                    await _context.Activities.AddAsync(request, cancellationToken);
                    var result = await _context.SaveChangesAsync(cancellationToken) > 0;

                    if (!result) return Result<Unit>.Failure("Failed to create activity");

                    return Result<Unit>.Success(Unit.Value);
                }
            }
        }
    }
}
