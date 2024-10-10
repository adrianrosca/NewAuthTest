using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Windows.Controls;

namespace WpfClient
{
    // handles authentication operations
    // ------------------------------------------------------------------------------------------
    public static class AuthUtil
    {
        private static readonly HttpClient httpClient = new HttpClient(new HttpClientHandler { UseDefaultCredentials = true });

        // retrieves windows identity information
        // -------------------------------------------------------------
        public static WindowsIdentity GetCurrentWindowsIdentity() => WindowsIdentity.GetCurrent();

        // checks if user is in a specific role
        // -------------------------------------------------------------
        public static bool IsUserInRole(WindowsIdentity identity, WindowsBuiltInRole role) =>
            new WindowsPrincipal(identity).IsInRole(role);

        // http client instance
        // -------------------------------------------------------------
        public static HttpClient HttpClient => httpClient;

        // retrieves data from the specified endpoint
        // -------------------------------------------------------------
        private static async Task<string> GetDataAsync(HttpClient httpClient, string endpoint)
        {
            var response = await httpClient.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }


        // retrieves secure data and logs actions
        // -------------------------------------------------------------
        public static async Task GetSecureDataAsync(RichTextBox logRichTextBox, WebBrowser htmlViewer)
        {
            var user = AuthUtil.GetCurrentWindowsIdentity();
            LogUtil.LogWindowsUserInfo(logRichTextBox, user);

            LogUtil.AddLogs(logRichTextBox, false,
                "Connecting to the secure endpoint...",
                "Using Windows credentials for authentication..."
            );

            int port = ConfigUtil.GetServicePort();
            string endpoint = $"http://localhost:{port}/BasicHttp";
            LogUtil.AddLogs(logRichTextBox, false, $"Connecting to endpoint {endpoint}");

            try
            {
                string content = await GetDataAsync(AuthUtil.HttpClient, endpoint);

                if (LogUtil.IsHtmlResponse(content))
                {
                    string validHtml = LogUtil.EnsureValidHtml(content);
                    htmlViewer.NavigateToString(validHtml);
                }

                LogUtil.AddLogs(logRichTextBox, true, "Authentication successful: Data retrieved successfully.");
            }
            catch (Exception ex)
            {
                LogUtil.AddLogs(logRichTextBox, false, $"Error: {ex.Message}");
                LogUtil.AppendExceptionDetails(logRichTextBox, ex);
            }
        }
    }
}