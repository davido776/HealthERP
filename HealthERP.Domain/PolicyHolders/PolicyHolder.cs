using HealthERP.Domain.Identity;
using Microsoft.AspNetCore.Identity;

namespace HealthERP.Domain.PolicyHolders
{
    public class PolicyHolder : ApplicationUser
    {
        public string? NationalId { get;set; }

        public string? PolicyNumber { get;set; }

        public DateTime DateofBirth { get;set;}
    }
}
