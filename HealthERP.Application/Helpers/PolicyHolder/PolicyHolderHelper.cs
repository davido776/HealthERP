using System.Text;

namespace HealthERP.Application.Helpers.PolicyHolder
{
    public static class PolicyHolderHelper
    {
        private const int PolicyNumberLength = 6;
        public static string GeneratePolicyNumber()
        {
            Random random = new Random();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var stringBuilder = new StringBuilder();

            for (int i = 0; i < PolicyNumberLength; i++)
            {
                stringBuilder.Append(chars[random.Next(chars.Length)]);
            }

            return stringBuilder.ToString();
        }
    }
}
