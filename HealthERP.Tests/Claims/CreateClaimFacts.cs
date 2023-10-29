using HealthERP.Application.Command.Claims;
using HealthERP.Application.Interfaces;
using HealthERP.Domain.Claims;
using HealthERP.Domain.Expenses;
using HealthERP.Tests.Commons;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace HealthERP.Tests.Claims
{
    public class CreateClaimFacts : IClassFixture<TestingContainer>
    {
        private TestingContainer container { get; }
        public CreateClaimFacts(TestingContainer Container)
        {
            container = Container;
        }

        [Fact]
        public void GetProducts_ReturnsAllProducts()
        {
            //using (var context = container.CreateContext())
            //{
            //    var userAccessorMock = new Mock<IUserAccessor>();
            //    userAccessorMock.Setup(u => u.GetUsername()).Returns("testuser");

            //    var handler = new CreateClaim.Handler(userAccessorMock.Object, context);

            //    var request = new CreateClaim.Request
            //    {
            //        Status = ClaimStatus.Submitted,
            //        Expenses = new List<CreateClaim.ExpenseModel>
            //        {
            //            new CreateClaim.ExpenseModel { Amount = 100, Name = "Expense 1", Type = ExpenseType.Procedure, Date = DateTime.Now },
            //            new CreateClaim.ExpenseModel { Amount = 200, Name = "Expense 2", Type = ExpenseType.Prescription,Date = DateTime.Now }
            //        }
            //    };


            //    var result = handler.Handle(request, CancellationToken.None);
                

            //    Assert.Equal(10, products.Count);
            //}
        }

    }
}
