using HealthERP.Application.Core;
using HealthERP.Domain.Claims;
using HealthERP.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace HealthERP.Application.Command.Claims
{
    public class DeclineClaim
    {
        public class Request : IRequest<Result<string>>
        {
            [Required]
            public string? ClaimId { get; set; }
        }

        public class Handler : IRequestHandler<Request, Result<string>>
        {
            private readonly AppDbContext context;
            public Handler(AppDbContext Context)
            {
                context = Context;
            }
            public async Task<Result<string>> Handle(Request request, CancellationToken cancellationToken)
            {
                var claim = await context.Claims.FirstOrDefaultAsync(x => x.Id == request.ClaimId);

                if (claim == null)
                {
                    return Result<string>.Failure("claim not found");
                }

                claim.Status = ClaimStatus.Declined;

                if (await context.SaveChangesAsync() > 0)
                {
                    return Result<string>.Success("Claim declined");
                }

                return Result<string>.Failure("Failed to decline claim");
            }
        }
    }
}
