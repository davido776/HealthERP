using HealthERP.Application.Command.Claims;
using HealthERP.Application.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthERP.Presentation.Controllers
{
    public class ClaimController : BaseController
    {
        
        [HttpPost]
        [Authorize(Policy = PolicyConstants.PolicyHolderOnly)]
        public async Task<IActionResult> CreateClaim([FromBody] CreateClaim.Request request)
        {
            return HandleResult(await Mediator.Send(request));
        }
    }
}
