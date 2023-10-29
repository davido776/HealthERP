using HealthERP.Application.Queries.Claims;
using HealthERP.Domain.Claims;

namespace HealthERP.Application.Helpers.ClaimHelpers
{
    public static class ClaimHelper
    {
        public static ClaimModel GetClaimModel(Claim claim)
        {
            var claimModel = new ClaimModel
            {
                Id = claim.Id,
                Status = claim.Status,
                PolicyHolderNationalId = claim.PolicyHolder?.NationalId,
                TotalExpenseAmount = claim.TotalExpenseAmount
            };

            var expenses = new List<ExpenseModel>();

            foreach (var expense in claim.Expenses)
            {
                var expenseModel = new ExpenseModel
                {
                    Id = expense.Id,
                    Type = expense.Type,
                    Name = expense.Name,
                    Amount = expense.Amount,
                    Date = expense.Date,
                };

                expenses.Add(expenseModel);
            }

            claimModel.Expenses = expenses;

            return claimModel;
        }

        public static List<ClaimModel> GetClaimModel(List<Claim> claims)
        {
            var claimModels = new List<ClaimModel>();

            foreach (var claim in claims)
            {
                var claimModel = new ClaimModel
                {
                    Id = claim.Id,
                    Status = claim.Status,
                    PolicyHolderNationalId = claim.PolicyHolder?.NationalId,
                    TotalExpenseAmount = claim.TotalExpenseAmount
                };

                var expenses = new List<ExpenseModel>();

                foreach (var expense in claim.Expenses)
                {
                    var expenseModel = new ExpenseModel
                    {
                        Id = expense.Id,
                        Type = expense.Type,
                        Name = expense.Name,
                        Amount = expense.Amount,
                        Date = expense.Date,
                    };

                    expenses.Add(expenseModel);
                }

                claimModel.Expenses = expenses;

                claimModels.Add(claimModel);
            }
            

            return claimModels;
        }
    }
}
