using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace WpfClient
{
    public static class AuthUtil
    {
        private static readonly HttpClient httpClient = new HttpClient(new HttpClientHandler
        {
            UseDefaultCredentials = true  // Enables Windows Authentication
        });


        public static async Task GetSecureDataAsync(RichTextBox logRichTextBox, WebBrowser htmlViewer)
        {
            var logs = new List<(string message, bool isSuccess)>();

            // Log user info and endpoint setup
            LogWindowsUserInfo(logs);

            logs.Add(("Connecting to the secure endpoint...", false));
            logs.Add(("Using Windows credentials for authentication...", false));

            // Retrieve endpoint from config
            int port = ConfigUtil.GetServicePort();
            string endpoint = $"http://localhost:{port}/BasicHttp";
            logs.Add(($"Connecting to endpoint {endpoint}", false));

            try
            {
                // Send request to secure endpoint
                var response = await httpClient.GetAsync(endpoint);
                string content = await response.Content.ReadAsStringAsync();

                if (IsHtmlResponse(content))
                {
                    // Wrap content in HTML tags if not already present
                    if (!content.StartsWith("<!DOCTYPE html>", StringComparison.OrdinalIgnoreCase) &&
                        !content.StartsWith("<html", StringComparison.OrdinalIgnoreCase))
                    {
                        content = $"<!DOCTYPE html><html><body>{content}</body></html>";
                    }

                    // Display formatted HTML content in WebBrowser directly
                    htmlViewer.NavigateToString(content);
                }
                
                if (response.IsSuccessStatusCode)
                {
                    logs.Add(("Authentication successful: Data retrieved successfully.", true));
                }
                else
                {
                    logs.Add(($"Error: Server returned status code {response.StatusCode}", false));
                }
            }
            catch (Exception ex)
            {
                logs.Add(($"Error: {ex.Message}", false));
                AppendExceptionDetails(logs, ex);
            }

            // Update logRichTextBox with all logs in color
            DisplayLogs(logRichTextBox, logs);
        }

        private static bool IsHtmlResponse(string content)
        {
            return 
                content.StartsWith("<?xml", StringComparison.OrdinalIgnoreCase) ||
                content.StartsWith("<!DOCTYPE html>", StringComparison.OrdinalIgnoreCase) ||
                content.StartsWith("<html", StringComparison.OrdinalIgnoreCase);
        }

        private static void LogWindowsUserInfo(List<(string message, bool isSuccess)> logs)
        {
            var user = WindowsIdentity.GetCurrent();
            if (user != null)
            {
                logs.Add(("Windows User Information:", false));
                logs.Add(($"Username: {user.Name}", false));
                logs.Add(($"Is Authenticated: {user.IsAuthenticated}", false));
                logs.Add(($"Authentication Type: {user.AuthenticationType ?? "N/A"}", false));
                logs.Add(($"User SID: {user.User?.Value ?? "N/A"}", false));

                var principal = new WindowsPrincipal(user);
                logs.Add(($"Is Admin: {principal.IsInRole(WindowsBuiltInRole.Administrator)}", false));
                logs.Add(($"Is Guest: {principal.IsInRole(WindowsBuiltInRole.Guest)}", false));

                logs.Add(("User Groups:", false));
                foreach (var group in user.Groups)
                {
                    try
                    {
                        var groupName = group.Translate(typeof(NTAccount))?.ToString() ?? "Unknown";
                        logs.Add(($"Group: {groupName} (SID: {group.Value})", false));
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

        private static void DisplayLogs(RichTextBox logRichTextBox, List<(string message, bool isSuccess)> logs)
        {
            logRichTextBox.Document.Blocks.Clear();
            foreach (var log in logs)
            {
                var paragraph = new Paragraph
                {
                    Margin = new System.Windows.Thickness(0)  // Removes extra space between lines
                };

                var run = new Run($"{DateTime.Now:HH:mm:ss}: {log.message}")
                {
                    Foreground = log.isSuccess ? Brushes.Green : (log.message.StartsWith("Error") ? Brushes.Red : Brushes.Black)
                };

                paragraph.Inlines.Add(run);
                logRichTextBox.Document.Blocks.Add(paragraph);
            }
        }
    }
}
