using AutoFixture;
using HealthERP.Application.Queries.Claims;
using HealthERP.Domain.Claims;
using HealthERP.Test.Commons;

namespace HealthERP.Test.Queries.Claims
{
    public class GetClaimByIdFacts : IClassFixture<IntegrationTestDatabaseGenerator>
    {
        private readonly IntegrationTestDatabaseGenerator _databaseGenerator;
        private readonly Fixture fixture;
        public GetClaimByIdFacts() 
        {
            _databaseGenerator = new IntegrationTestDatabaseGenerator();
            fixture = new Fixture();
        }

        [Fact]
        public async Task Should_find_claim()
        {
            var context = _databaseGenerator.Generate();

            var claim = fixture.Build<Claim>()
                                 .With(x => x.Status, ClaimStatus.Submitted)
                                 .Create();

            context.Claims.Add(claim);

            await context.SaveChangesAsync();

            var handler = new GetClaimById.Handler(context);

            var request = fixture.Build<GetClaimByIdCommand>()
                                 .With(x => x.ClaimId, claim.Id)
                                 .Create();

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.Equal(claim.Id, result.Value.Id);
        }

        [Fact]
        public async Task Should_return_error_if_claim_not_found()
        {
            var context = _databaseGenerator.Generate();

            var claim = fixture.Build<Claim>()
                                 .With(x => x.Status, ClaimStatus.Submitted)
                                 .Create();

            var handler = new GetClaimById.Handler(context);

            var request = fixture.Build<GetClaimByIdCommand>()
                                 .With(x => x.ClaimId, claim.Id)
                                 .Create();

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.False(result.IsSuccess);
        }
    }
}
