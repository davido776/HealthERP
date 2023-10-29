using HealthERP.Application.Core;
using HealthERP.Application.Helpers.ClaimHelpers;
using HealthERP.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HealthERP.Application.Queries.Claims
{
    public class GetClaimsByPolicyHolder
    {
        public class Request : IRequest<Result<List<ClaimModel>>>
        {
            public string? PolicyHolderId { get; set; }
        }

        public class Handler : IRequestHandler<Request, Result<List<ClaimModel>>>
        {
            private readonly AppDbContext context;
            public Handler(AppDbContext Context)
            {
                context = Context;
            }

            public async Task<Result<List<ClaimModel>>> Handle(Request request, CancellationToken cancellationToken)
            {
                var claim = await context.Claims.Include(x => x.PolicyHolder)
                                                .Include(x => x.Expenses)
                                                .Where(x => x.PolicyHolderId == request.PolicyHolderId)
                                                .ToListAsync();

                var claimModels = ClaimHelper.GetClaimModel(claim);

                return Result<List<ClaimModel>>.Success(claimModels);

            }
        }
    }
}
