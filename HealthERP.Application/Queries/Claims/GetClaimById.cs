using HealthERP.Application.Core;
using HealthERP.Application.Helpers.ClaimHelpers;
using HealthERP.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HealthERP.Application.Queries.Claims
{
    public class GetClaimByIdCommand : IRequest<Result<ClaimModel>>
    {
        public string? ClaimId { get; set; }
    }

    public class GetClaimById
    {
        
        public class Handler : IRequestHandler<GetClaimByIdCommand, Result<ClaimModel>>
        {
            private readonly AppDbContext context;
            public Handler(AppDbContext Context)
            {
                context = Context;
            }

            public async Task<Result<ClaimModel>> Handle(GetClaimByIdCommand request, CancellationToken cancellationToken)
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
