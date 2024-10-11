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
        public static async Task<string> GetDataAsync(HttpClient httpClient, string endpoint, RichTextBox logRichTextBox)
        {
            LogUtil.Add(logRichTextBox, false, $"Sending GET request to: {endpoint}");
            
            var response = await httpClient.GetAsync(endpoint);            
            LogUtil.Add(logRichTextBox, false, $"Received response. Status: {response.StatusCode}");
            
            response.EnsureSuccessStatusCode();            
            var content = await response.Content.ReadAsStringAsync();            
            LogUtil.Add(logRichTextBox, false, $"Retrieved content. Length: {content.Length} characters");
            
            return content;
        }


        // retrieves secure data and logs actions
        // -------------------------------------------------------------
        public static async Task GetSecureDataAsync(RichTextBox logRichTextBox, WebBrowser htmlViewer)
        {
            var user = AuthUtil.GetCurrentWindowsIdentity();
            LogUtil.LogWindowsUserInfo(logRichTextBox, user);

            LogUtil.Add(logRichTextBox, false,
                "Connecting to the secure endpoint...",
                "Using Windows credentials for authentication..."
            );

            int port = ConfigUtil.GetServicePort();
            string endpoint = $"http://localhost:{port}/BasicHttp";
            LogUtil.Add(logRichTextBox, false, $"Connecting to endpoint {endpoint}");

            try
            {
                string content = await GetDataAsync(HttpClient, endpoint, logRichTextBox);

                if (LogUtil.IsHtmlResponse(content))
                {
                    string validHtml = LogUtil.EnsureValidHtml(content);
                    htmlViewer.NavigateToString(validHtml);
                }

                LogUtil.Add(logRichTextBox, true, "Authentication successful: Data retrieved successfully.");
            }
            catch (Exception ex)
            {
                LogUtil.Add(logRichTextBox, false, $"Error: {ex.Message}");
                LogUtil.AppendExceptionDetails(logRichTextBox, ex);
            }
        }
    }
}