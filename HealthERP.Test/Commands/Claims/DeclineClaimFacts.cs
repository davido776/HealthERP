using AutoFixture;
using HealthERP.Application.Command.Claims;
using HealthERP.Domain.Claims;
using HealthERP.Test.Commons;

namespace HealthERP.Test.Commands.Claims
{
    public class DeclineClaimFacts : IClassFixture<IntegrationTestDatabaseGenerator>
    {
        private readonly IntegrationTestDatabaseGenerator _databaseGenerator;
        private readonly Fixture fixture;
        public DeclineClaimFacts()
        {
            //container = Container;
            _databaseGenerator = new IntegrationTestDatabaseGenerator();
            fixture = new Fixture();
        }

        [Fact]
        public async Task Should_decline_claim()
        {
            var context = _databaseGenerator.Generate();

            var claim = fixture.Build<Claim>()
                                 .With(x => x.Status, ClaimStatus.Submitted)
                                 .Create();

            context.Claims.Add(claim);

            await context.SaveChangesAsync();

            var handler = new DeclineClaim.Handler(context);

            var request = fixture.Build<DeclineClaim.Request>()
                                 .With(x => x.ClaimId, claim.Id)
                                 .Create();

            var result = await handler.Handle(request, CancellationToken.None);

            var newClaim = context.Claims.Find(claim.Id);

            Assert.Equal(ClaimStatus.Declined, newClaim.Status);
        }

        [Fact]
        public async Task Should_return_error_if_claim_not_found()
        {
            var context = _databaseGenerator.Generate();

            var claim = fixture.Build<Claim>()
                                 .With(x => x.Status, ClaimStatus.Submitted)
                                 .Create();

            var handler = new DeclineClaim.Handler(context);

            var request = fixture.Build<DeclineClaim.Request>()
                                 .With(x => x.ClaimId, claim.Id)
                                 .Create();

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal("claim not found", result.Error);
        }
    }
}
