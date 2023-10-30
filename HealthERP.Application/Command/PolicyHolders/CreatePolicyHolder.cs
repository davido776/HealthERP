using HealthERP.Application.Constants;
using HealthERP.Application.Core;
using HealthERP.Application.Helpers.PolicyHolder;
using HealthERP.Domain.Identity;
using HealthERP.Domain.PolicyHolders;
using HealthERP.Persistence;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace HealthERP.Application.Command.PolicyHolders
{
    public class CreatePolicyHolder
    {
        public class Request : IRequest<Result<Unit>>
        {
            public string FirstName { get; set; }

            public string LastName { get; set; }

            public string Email { get; set; }

            public string Password { get; set; }
            public string? NationalId { get; set; }

            public string? PolicyNumber { get; set; }

            public DateTime DateofBirth { get; set; }
        }

        public class Handler : IRequestHandler<Request, Result<Unit>>
        {
            private readonly UserManager<ApplicationUser> userManager;
            private readonly AppDbContext context;
            public Handler(UserManager<ApplicationUser> UserManager,
                           AppDbContext Context)
            {
                userManager = UserManager;
                context = Context;
            }

            public async Task<Result<Unit>> Handle(Request request, CancellationToken cancellationToken)
            {
                using (var transaction = await context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var policyHolder = new PolicyHolder
                        {
                            FirstName = request.FirstName,
                            LastName = request.LastName,
                            Email = request.Email,
                            NationalId = request.NationalId,
                            DateofBirth = request.DateofBirth,
                            PolicyNumber = GetPolicyNumber(),
                        };

                        var existingUser = userManager.FindByEmailAsync(request.Email);

                        if (existingUser != null)
                        {
                            return Result<Unit>.Failure("Failed to create user.");
                        }

                        // Add roles and create user within the same transaction
                        var result = await userManager.CreateAsync(policyHolder, request.Password);
                        if (result.Succeeded)
                        {
                            // Add PolicyHolder role
                            await userManager.AddToRoleAsync(policyHolder, RoleConstants.PolicyHolderRole);
                            await transaction.CommitAsync(); // Commit the transaction if everything succeeds
                            return Result<Unit>.Success(Unit.Value);
                        }
                        else
                        {
                            // Handle user creation failure if needed
                            await transaction.RollbackAsync(); // Rollback the transaction in case of failure
                            return Result<Unit>.Failure("Failed to create user.");
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions if necessary
                        await transaction.RollbackAsync(); // Rollback the transaction on exceptions
                        return Result<Unit>.Failure("An error occurred: " + ex.Message);
                    }
                }
            }

            public string GetPolicyNumber()
            {
                bool found = true;
                var policyNumber = "";

                while(found)
                {
                    if (!found)
                    {
                        break;
                    }

                    policyNumber = PolicyHolderHelper.GeneratePolicyNumber();
                    found = context.PolicyHolders.Any(x => x.PolicyNumber == policyNumber);
                }

                return policyNumber;
            }
        }
    }
}
