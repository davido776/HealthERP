using HealthERP.Application.Core;
using HealthERP.Application.Interfaces;
using HealthERP.Domain.Claims;
using HealthERP.Domain.Expenses;
using HealthERP.Persistence;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace HealthERP.Application.Command.Claims
{

    public class SubmitClaimCommand : IRequest<Result<Unit>>
    {
        public List<SubmitClaim.ExpenseModel> Expenses { get; set; } = new List<SubmitClaim.ExpenseModel>();
    }


    public class SubmitClaim
    {
        public class ExpenseModel
        {
            public ExpenseType Type { get; set; }

            public string? Name { get; set; }

            public decimal Amount { get; set; }

            public DateTime Date { get; set; }
        }

        public class Handler : IRequestHandler<SubmitClaimCommand, Result<Unit>>
        {
            private readonly IUserAccessor userAccessor;
            private readonly AppDbContext context;
            public Handler(IUserAccessor UserAccessor, AppDbContext Context)
            {
                userAccessor = UserAccessor;
                context = Context;
            }
            public async Task<Result<Unit>> Handle(SubmitClaimCommand request, CancellationToken cancellationToken)
            {
                var policyHolderId = userAccessor.GetUsername();
                
                var claim = new Claim
                {
                    Status = ClaimStatus.Submitted,
                    PolicyHolderId = policyHolderId          
                };

                context.Claims.Add(claim);

                var expenses = new List<Expense>();
                
                foreach (var expense in request.Expenses)
                {
                    var newExpense = new Expense
                    {
                        Amount = expense.Amount,
                        Name   = expense.Name,
                        Type = expense.Type,
                        ClaimId = claim.Id,
                        Date = expense.Date,
                    };

                    expenses.Add(newExpense);

                }

                context.AddRange(expenses);

                if(await context.SaveChangesAsync() > 0) 
                {
                    return Result<Unit>.Success(Unit.Value);
                }

                return Result<Unit>.Failure("Failed to create claim");

            }
        }
    }
}
