using HealthERP.Application.Core;
using MediatR;

namespace HealthERP.Application.Command.Claim
{
    public class CreateClaim
    {
        public class Request : IRequest<Result<Unit>>
        {
            public string FirstName { get; set; }

            public string LastName { get; set; }

            public string Email { get; set; }

            public string Password { get; set; }
            public string? NationalId { get; set; }

            public string? PolicyNumber { get; set; }

            public DateTime DateofBirth { get; set; }
        }

    }
}
