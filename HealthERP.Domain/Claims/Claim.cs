using HealthERP.Domain.Expenses;
using HealthERP.Domain.Identity;
using HealthERP.Domain.PolicyHolders;

namespace HealthERP.Domain.Claims
{
    public class Claim : BaseEntity
    {
        public ClaimStatus Status { get;set; }

        public string? PolicyHolderId { get; set; }

        public PolicyHolder? PolicyHolder { get;set; }

        public List<Expense> Expenses { get; set; } = new List<Expense>();

        public decimal TotalExpenseAmount => GetTotal();

        public decimal GetTotal()
        {
            return Expenses.Sum(x => x.Amount);
        }
    }
}
