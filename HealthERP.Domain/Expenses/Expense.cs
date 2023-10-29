using HealthERP.Domain.Identity;

namespace HealthERP.Domain.Expenses
{
    public class Expense : BaseEntity
    {
        public ExpenseType Type { get; set; }

        public string? Name { get; set; }

        public decimal Amount { get; set; }

        public DateTime Date { get; set; }

        public string? ClaimId { get; set; }
    }
}
