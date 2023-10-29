using AutoFixture;
using HealthERP.Application.Command.Claims;
using HealthERP.Domain.Claims;
using HealthERP.Test.Commons;

namespace HealthERP.Test.Commands.Claims
{
    public class ApproveClaimFacts : IClassFixture<IntegrationTestDatabaseGenerator>
    {
        private readonly IntegrationTestDatabaseGenerator _databaseGenerator;
        private readonly Fixture fixture;
        public ApproveClaimFacts()
        {
            //container = Container;
            _databaseGenerator = new IntegrationTestDatabaseGenerator();
            fixture = new Fixture();
        }

        [Fact]
        public async Task Should_approve_claim()
        {
            var context = _databaseGenerator.Generate();

            var claim = fixture.Build<Claim>()
                                 .With(x => x.Status, ClaimStatus.Submitted)
                                 .Create();

            context.Claims.Add(claim);

            await context.SaveChangesAsync();

            var handler = new ApproveClaim.Handler(context);

            var request = fixture.Build<ApproveClaim.Request>()
                                 .With(x => x.ClaimId, claim.Id)
                                 .Create();

            var result = await handler.Handle(request, CancellationToken.None);

            var newClaim = context.Claims.Find(claim.Id);

            Assert.Equal(ClaimStatus.Approved, newClaim.Status);
        }

        [Fact]
        public async Task Should_not_approve_if_claim_not_found()
        {
            var context = _databaseGenerator.Generate();

            var claim = fixture.Build<Claim>()
                                 .With(x => x.Status, ClaimStatus.Submitted)
                                 .Create();

            var handler = new ApproveClaim.Handler(context);

            var request = fixture.Build<ApproveClaim.Request>()
                                 .With(x => x.ClaimId, claim.Id)
                                 .Create();

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal("claim not found", result.Error);
        }
    }
}
