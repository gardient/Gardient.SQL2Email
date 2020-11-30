using Serilog;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Gardient.SQL2Email.Helpers
{
    static class TemplateHelper
    {
        private static Regex tokenRegex = new Regex("#(?<key>\\w+)#");

        public static string ReplaceTokens(string template, Dictionary<string, string> kvps)
        {
            return tokenRegex.Replace(template, match =>
            {
                string key = match.Groups["key"].Value;
                if (kvps.TryGetValue(key.ToLower(), out string value))
                    return value;

                Log.Error("Could not find {TokenKey} in query response", key);
                return match.Value;
            });
        }
    }
}
