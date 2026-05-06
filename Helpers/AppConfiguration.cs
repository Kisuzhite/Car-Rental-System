using System.Text.Json;

namespace Car_Rental_System.Helpers
{
    internal static class AppConfiguration
    {
        private const string AppSettingsFileName = "appsettings.json";
        private const string ConnectionStringEnvironmentVariable = "CAR_RENTAL_CONNECTION_STRING";
        private const string OwnerUserNameEnvironmentVariable = "CAR_RENTAL_OWNER_USERNAME";
        private const string OwnerPasswordEnvironmentVariable = "CAR_RENTAL_OWNER_PASSWORD";

        public static string GetConnectionString()
        {
            string? environmentConnectionString = Environment.GetEnvironmentVariable(ConnectionStringEnvironmentVariable);

            if (!string.IsNullOrWhiteSpace(environmentConnectionString))
                return environmentConnectionString;

            string? configuredConnectionString = GetJsonValue("ConnectionStrings", "CarRentalDb");

            if (!string.IsNullOrWhiteSpace(configuredConnectionString))
                return configuredConnectionString;

            throw new InvalidOperationException(
                $"Database connection string is missing. Set {ConnectionStringEnvironmentVariable} or add ConnectionStrings:CarRentalDb to {AppSettingsFileName}.");
        }

        public static (string UserName, string Password) GetOwnerAccount()
        {
            string? configuredUserName = GetJsonValue("OwnerAccount", "UserName");
            string? configuredPassword = GetJsonValue("OwnerAccount", "Password");

            string userName = Environment.GetEnvironmentVariable(OwnerUserNameEnvironmentVariable)
                ?? configuredUserName
                ?? "";
            string password = Environment.GetEnvironmentVariable(OwnerPasswordEnvironmentVariable)
                ?? configuredPassword
                ?? "";

            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
            {
                throw new InvalidOperationException(
                    $"Owner login is missing. Set {OwnerUserNameEnvironmentVariable} and {OwnerPasswordEnvironmentVariable}, or add OwnerAccount settings to {AppSettingsFileName}.");
            }

            return (userName, password);
        }

        private static string? GetJsonValue(params string[] path)
        {
            foreach (string configPath in GetCandidateConfigPaths())
            {
                if (!File.Exists(configPath))
                    continue;

                using var document = JsonDocument.Parse(File.ReadAllText(configPath));
                JsonElement current = document.RootElement;

                foreach (string section in path)
                {
                    if (current.ValueKind != JsonValueKind.Object ||
                        !current.TryGetProperty(section, out current))
                    {
                        current = default;
                        break;
                    }
                }

                if (current.ValueKind == JsonValueKind.String)
                    return current.GetString();
            }

            return null;
        }

        private static IEnumerable<string> GetCandidateConfigPaths()
        {
            var paths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            AddConfigPaths(paths, AppContext.BaseDirectory);
            AddConfigPaths(paths, Directory.GetCurrentDirectory());

            return paths;
        }

        private static void AddConfigPaths(HashSet<string> paths, string startDirectory)
        {
            DirectoryInfo? directory = new(startDirectory);

            while (directory != null)
            {
                paths.Add(Path.Combine(directory.FullName, AppSettingsFileName));
                directory = directory.Parent;
            }
        }
    }
}
