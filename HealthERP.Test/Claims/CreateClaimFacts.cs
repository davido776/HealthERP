using HealthERP.Application.Command.Claims;
using HealthERP.Application.Interfaces;
using HealthERP.Domain.Claims;
using HealthERP.Domain.Expenses;
using HealthERP.Persistence;
using HealthERP.Test.Commons;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthERP.Test.Claims
{
    public class CreateClaimFacts : IClassFixture<IntegrationTestDatabaseGenerator>
    {
        //private TestingContainer container { get; }
        private readonly IntegrationTestDatabaseGenerator _databaseGenerator;
        public CreateClaimFacts()
        {
            //container = Container;
            _databaseGenerator = new IntegrationTestDatabaseGenerator();
        }

        [Fact]
        public async Task GetProducts_ReturnsAllProducts()
        {
            var context = _databaseGenerator.Generate();

            var policyHolder = new Domain.PolicyHolders.PolicyHolder() { };
            context.PolicyHolders.Add(policyHolder);
            await context.SaveChangesAsync();
        
            var userAccessorMock = new Mock<IUserAccessor>();
            userAccessorMock.Setup(u => u.GetUsername()).Returns(policyHolder.Id);

            var handler = new SubmitClaim.Handler(userAccessorMock.Object, context);

            var request = new SubmitClaim.Request
            {
                Status = ClaimStatus.Submitted,
                Expenses = new List<SubmitClaim.ExpenseModel>
                {
                    new SubmitClaim.ExpenseModel { Amount = 100, Name = "Expense 1", Type = ExpenseType.Procedure, Date = DateTime.Now },
                    new SubmitClaim.ExpenseModel { Amount = 200, Name = "Expense 2", Type = ExpenseType.Prescription,Date = DateTime.Now }
                }
            };

            var result = await handler.Handle(request, CancellationToken.None);

            var expenses = await context.Expenses.ToListAsync();
            var claim = await context.Claims.FirstOrDefaultAsync();

            Assert.Equal(2, expenses.Count);
            Assert.Equal(300, claim.TotalExpenseAmount);
        }
    }
}
