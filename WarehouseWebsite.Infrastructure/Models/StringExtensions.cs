using System.Text;
using System.Text.RegularExpressions;

namespace WarehouseWebsite.Infrastructure.Models
{
    public static class StringExtensions
    {
        public static string? InjectEnvironmentVariables(this string? confString)
        {
            if (confString != null)
            {
                Regex regex = new(@"\$\{(?<env>[A-Za-z_][A-Za-z0-9_]*)\}");
                MatchCollection matches = regex.Matches(confString);
                StringBuilder builder = new(confString);
                foreach (Match match in matches)
                {
                    string? envVar = Environment.GetEnvironmentVariable(match.Groups["env"].Value);
                    if (envVar != null)
                    {
                        builder.Replace(match.Value, envVar);
                    }
                }
                return builder.ToString();
            }
            return null;
        }
    }
}
