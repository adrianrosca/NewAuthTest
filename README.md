
# Project Setup Guide

## Prerequisites

- **.NET 8 SDK**
- **Visual Studio Code** with the following extensions:
  - C# for Visual Studio Code
  - NuGet Package Manager

## Setup Instructions

### 1. Trust the Development SSL Certificate

Run this command to avoid SSL issues:

```bash
dotnet dev-certs https --trust
```

### 2. Build and Run

1. Open the **Run and Debug** sidebar (`Ctrl+Shift+D`).
2. Select **Start Both Projects**.
3. Press **F5**.

This will build and launch both the ASP.NET Core backend and the WPF client simultaneously.

## Troubleshooting

- **SSL/TLS Errors**: Ensure the development certificate is trusted. Alternatively, to bypass SSL validation (for development only), modify `AuthUtil.cs` in the WPF client:
  ```csharp
  ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => true
  ```

- **Windows Authentication Issues**: Confirm Windows Authentication is set up correctly for the ASP.NET Core backend.

## Notes

- The backend listens on `https://localhost:7042`. Ensure the WPF client points to this URL.
- Logs in the WPF client’s `TextBox` show real-time status and error messages.
