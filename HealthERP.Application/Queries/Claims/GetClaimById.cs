using HealthERP.Application.Core;
using HealthERP.Application.Helpers.ClaimHelpers;
using HealthERP.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HealthERP.Application.Queries.Claims
{
    public class GetClaimById
    {
        public class Request : IRequest<Result<ClaimModel>>
        {
            public string? ClaimId { get; set; }
        }

        public class Handler : IRequestHandler<Request, Result<ClaimModel>>
        {
            private readonly AppDbContext context;
            public Handler(AppDbContext Context)
            {
                context = Context;
            }

            public async Task<Result<ClaimModel>> Handle(Request request, CancellationToken cancellationToken)
            {
                var claim = await context.Claims
                                         .Include(x => x.PolicyHolder)
                                         .Include(x => x.Expenses)
                                         .FirstOrDefaultAsync(x => x.Id == request.ClaimId);

                if (claim == null)
                {
                    return Result<ClaimModel>.Failure("claim not found");
                }

                var claimModel = ClaimHelper.GetClaimModel(claim);

                return Result<ClaimModel>.Success(claimModel);

            }
        }
    }
}
