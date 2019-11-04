using System;
using System.Collections.Generic;
using System.Linq;
using FI.API.SignTool.Parameters.Interfaces;

namespace FI.API.SignTool.SigningProviders
{
    public class KeyVaultConnectionString : IValidatable
    {
        public const string DefaultAlgorithm = "RS256";
        public const string DefaultKeyVersion = "";

        public string Url { get; set; }
        public string KeyName { get; set; }
        public string KeyVersion { get; set; }
        public string Algorithm { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }

        public bool HasKeyName => !string.IsNullOrWhiteSpace(KeyName);
        public bool HasKeyVersion => !string.IsNullOrWhiteSpace(KeyVersion);

        public KeyVaultConnectionString(string connectionString)
        {
            var dict = (connectionString ?? string.Empty).Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Split("=".ToCharArray()))
                .ToDictionary(x => x.First().ToLower(), x => string.Join("=", x.Skip(1)));

            Url          = GetValue(dict, nameof(Url).ToLower());
            KeyName      = GetValue(dict, nameof(KeyName).ToLower());
            KeyVersion   = GetValue(dict, nameof(KeyVersion).ToLower()) ?? DefaultKeyVersion;
            Algorithm    = GetValue(dict, nameof(Algorithm).ToLower()) ?? DefaultAlgorithm;
            ClientId     = GetValue(dict, nameof(ClientId).ToLower());
            ClientSecret = GetValue(dict, nameof(ClientSecret).ToLower());
        }

        private static string GetValue(IDictionary<string, string> dict, string key)
        {
            return dict.ContainsKey(key)
                ? dict[key]
                : null;
        }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Url))
                throw new ArgumentException($"{nameof(KeyVaultConnectionString)}: Invalid or missing {nameof(Url)}", nameof(Url));
            if (string.IsNullOrWhiteSpace(KeyName))
                throw new ArgumentException($"{nameof(KeyVaultConnectionString)}: Invalid or missing {nameof(KeyName)}", nameof(KeyName));
            if (string.IsNullOrWhiteSpace(ClientId))
                throw new ArgumentException($"{nameof(KeyVaultConnectionString)}: Invalid or missing {nameof(ClientId)}", nameof(ClientId));
            if (string.IsNullOrWhiteSpace(ClientSecret))
                throw new ArgumentException($"{nameof(KeyVaultConnectionString)}: Invalid or missing {nameof(ClientSecret)}", nameof(ClientSecret));
        }
    }
}
