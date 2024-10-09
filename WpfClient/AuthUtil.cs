using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Principal;
using System.Threading.Tasks;

namespace WpfClient
{
    public static class AuthUtil
    {
        private static readonly HttpClient httpClient = new HttpClient(new HttpClientHandler
        {
            UseDefaultCredentials = true  // Enables Windows Authentication
        });

        public static async Task<(string result, List<(string message, bool isSuccess)> logs)> GetSecureDataAsync()
        {
            var logs = new List<(string, bool)>();
            
            LogWindowsUserInfo(logs);  // Log user info at the start

            logs.Add(("Connecting to the secure endpoint...", false));
            logs.Add(("Using Windows credentials for authentication...", false));

            // Add headers information for diagnostics
            logs.Add(("Request Headers:", false));
            foreach (var header in httpClient.DefaultRequestHeaders)
            {
                logs.Add(($"{header.Key}: {string.Join(", ", header.Value)}", false));
            }

            try
            {
                // Send request to secure endpoint
                var response = await httpClient.GetAsync("https://localhost:7042/secure-data");

                if (response.IsSuccessStatusCode)
                {
                    logs.Add(("Authentication successful: Data retrieved successfully.", true));
                    return (await response.Content.ReadAsStringAsync(), logs);
                }
                else
                {
                    logs.Add(($"Error: Server returned status code {response.StatusCode}", false));
                    return ($"Error: {response.ReasonPhrase}", logs);
                }
            }
            catch (Exception ex)
            {
                logs.Add(($"Error: {ex.Message}", false));
                AppendExceptionDetails(logs, ex);
                return ($"Exception: {ex.Message}", logs);
            }
        }

        private static void LogWindowsUserInfo(List<(string message, bool isSuccess)> logs)
        {
            var user = WindowsIdentity.GetCurrent();
            if (user != null)
            {
                logs.Add(("Windows User Information:", false));
                logs.Add(($"Username: {user.Name}", false));
                logs.Add(($"Is Authenticated: {user.IsAuthenticated}", false));
                logs.Add(($"Authentication Type: {user.AuthenticationType}", false));
                logs.Add(($"User SID: {user.User?.Value}", false));

                var principal = new WindowsPrincipal(user);
                logs.Add(($"Is Admin: {principal.IsInRole(WindowsBuiltInRole.Administrator)}", false));
                logs.Add(($"Is Guest: {principal.IsInRole(WindowsBuiltInRole.Guest)}", false));

                logs.Add(("User Groups:", false));
                foreach (var group in user.Groups)
                {
                    try
                    {
                        logs.Add(($"Group: {group.Translate(typeof(NTAccount))} (SID: {group.Value})", false));
                    }
                    catch
                    {
                        logs.Add(($"Group SID: {group.Value} (Translation failed)", false));
                    }
                }
            }
            else
            {
                logs.Add(("No Windows user information available.", false));
            }
        }

        private static void AppendExceptionDetails(List<(string message, bool isSuccess)> logs, Exception ex)
        {
            while (ex != null)
            {
                logs.Add(($"Exception: {ex.Message}", false));
                ex = ex.InnerException;
            }
        }
    }
}
