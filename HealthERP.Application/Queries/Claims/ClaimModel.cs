using HealthERP.Domain.Claims;
using HealthERP.Domain.Expenses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthERP.Application.Queries.Claims
{
    public class ClaimModel
    {
        public string Id { get; set; }
        public ClaimStatus Status { get; set; }

        public string? PolicyHolderNationalId { get; set; }

        public List<ExpenseModel> Expenses { get; set; } = new List<ExpenseModel>();

        public decimal TotalExpenseAmount { get; set; }
    }

    public class ExpenseModel 
    {
        public string Id { get; set; }
        public ExpenseType Type { get; set; }

        public string? Name { get; set; }

        public decimal Amount { get; set; }

        public DateTime Date { get; set; }
    }
}
