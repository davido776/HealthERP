using AutoFixture;
using HealthERP.Application.Queries.Claims;
using HealthERP.Domain.Claims;
using HealthERP.Domain.PolicyHolders;
using HealthERP.Test.Commons;

namespace HealthERP.Test.Queries.Claims
{
    public class GetClaimByPolicyHolderFacts
    {
        private readonly IntegrationTestDatabaseGenerator _databaseGenerator;
        private readonly Fixture fixture;
        public GetClaimByPolicyHolderFacts()
        {
            _databaseGenerator = new IntegrationTestDatabaseGenerator();
            fixture = new Fixture();
        }

        [Fact]
        public async Task Should_find_claim()
        {
            
            
            var context = _databaseGenerator.Generate();

            var policyHolder = fixture.Build<PolicyHolder>()
                                 .Create();

            context.PolicyHolders.Add(policyHolder);

            var claims = fixture.Build<Claim>()
                                 .With(x => x.Status, ClaimStatus.Submitted)
                                 .With(x => x.PolicyHolderId, policyHolder.Id)
                                 .Without(x => x.PolicyHolder)
                                 .CreateMany(2);

            context.Claims.AddRange(claims);

            await context.SaveChangesAsync();

            var handler = new GetClaimsByPolicyHolder.Handler(context);

            var request = fixture.Build<GetClaimsByPolicyHolder.Request>()
                                 .With(x => x.PolicyHolderId, policyHolder.Id)
                                 .Create();

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.Equal(2, result.Value.Count);
            Assert.All(result.Value, x => Assert.Equal(policyHolder.NationalId, x.PolicyHolderNationalId));
        }

        [Fact]
        public async Task Should_return_error_if_claim_not_found()
        {
            var context = _databaseGenerator.Generate();

            var claim = fixture.Build<Claim>()
                                 .With(x => x.Status, ClaimStatus.Submitted)
                                 .Create();

            var handler = new GetClaimsByPolicyHolder.Handler(context);

            var request = fixture.Build<GetClaimsByPolicyHolder.Request>()
                                 .Create();

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.Empty(result.Value);
        }
    }
}
