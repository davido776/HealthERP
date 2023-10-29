using AutoFixture;
using HealthERP.Application.Command.Claims;
using HealthERP.Application.Interfaces;
using HealthERP.Domain.Claims;
using HealthERP.Test.Commons;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace HealthERP.Test.Commands.Claims
{
    public class SubmitClaimFacts : IClassFixture<IntegrationTestDatabaseGenerator>
    {
        //private TestingContainer container { get; }
        private readonly IntegrationTestDatabaseGenerator _databaseGenerator;
        private readonly Fixture fixture;
        public SubmitClaimFacts()
        {
            //container = Container;
            _databaseGenerator = new IntegrationTestDatabaseGenerator();
            fixture = new Fixture();
        }

        [Fact]
        public async Task Should_create_claim_with_submitted_status()
        {
            var context = _databaseGenerator.Generate();

            var policyHolder = new Domain.PolicyHolders.PolicyHolder() { };
            context.PolicyHolders.Add(policyHolder);
            await context.SaveChangesAsync();

            var userAccessorMock = new Mock<IUserAccessor>();
            userAccessorMock.Setup(u => u.GetUsername()).Returns(policyHolder.Id);

            var handler = new SubmitClaim.Handler(userAccessorMock.Object, context);

            var expense1 = fixture.Build<SubmitClaim.ExpenseModel>()
                                 .With(x => x.Amount, 100)
                                 .Create();

            var expense2 = fixture.Build<SubmitClaim.ExpenseModel>()
                                 .With(x => x.Amount, 100)
                                 .Create();

            var request = fixture.Build<SubmitClaim.Request>()
                                .With(r => r.Expenses, new List<SubmitClaim.ExpenseModel> { expense1, expense2} )
                                .Create();

            var result = await handler.Handle(request, CancellationToken.None);

            var expenses = await context.Expense.ToListAsync();
            var claim = await context.Claims.FirstOrDefaultAsync();

            Assert.Equal(2, expenses.Count);
            Assert.Equal(expense1.Amount + expense2.Amount, claim.TotalExpenseAmount);
            Assert.Equal(ClaimStatus.Submitted, claim.Status);
        }
    }
}
