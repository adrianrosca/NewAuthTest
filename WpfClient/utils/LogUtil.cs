using System;
using System.Windows;
using System.Security.Principal;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace WpfClient
{
    // handles logging operations
    // ------------------------------------------------------------------------------------------
    public static class LogUtil
    {
        // adds multiple log entries to the richtextbox
        // -------------------------------------------------------------
        public static void AddLogs(RichTextBox logRichTextBox, bool isSuccess, params string[] messages)
        {
            foreach (var message in messages)
            {
                var paragraph = new Paragraph(new Run(message))
                {
                    Margin = new Thickness(0),
                    Padding = new Thickness(0),
                    Foreground = isSuccess ? Brushes.Green : Brushes.Black
                };

                logRichTextBox.Document.Blocks.Add(paragraph);
            }

            logRichTextBox.ScrollToEnd();
        }

        // logs windows user information
        // -------------------------------------------------------------
        public static void LogWindowsUserInfo(RichTextBox logRichTextBox, WindowsIdentity user)
        {
            if (user == null)
            {
                AddLogs(logRichTextBox, false, "No Windows user information available.");
                return;
            }

            AddLogs(logRichTextBox, false,
                "## Windows User Information",
                $"Username: {user.Name}",
                $"Is Authenticated: {user.IsAuthenticated}",
                $"Authentication Type: {user.AuthenticationType ?? "N/A"}",
                $"User SID: {user.User?.Value ?? "N/A"}",
                $"Is Admin: {AuthUtil.IsUserInRole(user, WindowsBuiltInRole.Administrator)}",
                $"Is Guest: {AuthUtil.IsUserInRole(user, WindowsBuiltInRole.Guest)}",
                "----------",
                "## User Groups"
            );

            foreach (var group in user.Groups)
            {
                try
                {
                    var groupName = group.Translate(typeof(NTAccount))?.ToString() ?? "Unknown";
                    AddLogs(logRichTextBox, false, $"Group: {groupName} (SID: {group.Value})");
                }
                catch
                {
                    AddLogs(logRichTextBox, false, $"Group SID: {group.Value} (Translation failed)");
                }
            }
            AddLogs(logRichTextBox, false, "----------");
        }

        // appends exception details to the log
        // -------------------------------------------------------------
        public static void AppendExceptionDetails(RichTextBox logRichTextBox, Exception ex)
        {
            AddLogs(logRichTextBox, false,
                "Exception Details:",
                $"Type: {ex.GetType().FullName}",
                $"Message: {ex.Message}",
                $"StackTrace:",
                ex.StackTrace ?? "No stack trace available"
            );

            if (ex.InnerException != null)
            {
                AddLogs(logRichTextBox, false, "Inner Exception:");
                AppendExceptionDetails(logRichTextBox, ex.InnerException);
            }
        }

        // checks if content is html
        // -------------------------------------------------------------
        public static bool IsHtmlResponse(string content)
        {
            return content.StartsWith("<?xml", StringComparison.OrdinalIgnoreCase) ||
                   content.StartsWith("<!DOCTYPE html>", StringComparison.OrdinalIgnoreCase) ||
                   content.StartsWith("<html", StringComparison.OrdinalIgnoreCase);
        }

        // ensures content is valid html
        // -------------------------------------------------------------
        public static string EnsureValidHtml(string content)
        {
            return content.StartsWith("<!DOCTYPE html>", StringComparison.OrdinalIgnoreCase) ||
                   content.StartsWith("<html", StringComparison.OrdinalIgnoreCase)
                ? content
                : $"<!DOCTYPE html><html><body>{content}</body></html>";
        }
    }
}