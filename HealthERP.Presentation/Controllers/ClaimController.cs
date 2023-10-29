using HealthERP.Application.Command.Claims;
using HealthERP.Application.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthERP.Presentation.Controllers
{
    public class ClaimController : BaseController
    {
        
        [HttpPost]
        [Authorize(Policy = PolicyConstants.SubmitClaimPolicy)]
        public async Task<IActionResult> CreateClaim([FromBody] SubmitClaim.Request request)
        {
            return HandleResult(await Mediator.Send(request));
        }

        [HttpPut("{id}/approve")]
        [Authorize(Policy = PolicyConstants.UpdateClaimPolicy)]
        public async Task<IActionResult> ApproveClaim([FromRoute] string Id)
        {
            return HandleResult(await Mediator.Send(new ApproveClaim.Request { ClaimId = Id}));
        }

        [HttpPut("{id}/decline")]
        [Authorize(Policy = PolicyConstants.UpdateClaimPolicy)]
        public async Task<IActionResult> DeclineClaim([FromRoute] string Id)
        {
            return HandleResult(await Mediator.Send(new DeclineClaim.Request { ClaimId = Id }));
        }
    }
}
