
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

## Technologies Used

- .NET 8.0
- ASP.NET Core
- WPF
- Windows Authentication
- HTTP Client

## Comparison gRPC and HTTP Client

This project uses HTTP Client for communication between the WPF client and the ASP.NET Core backend.
gRPC could be used instead, it has good performance and Protobuf serialization.

| Feature               | gRPC                                           | HTTP Client                                      |
|-----------------------|------------------------------------------------|--------------------------------------------------|
| **Protocol**          | HTTP/2 with Protocol Buffers                   | HTTP/1.1 (typically) with JSON/XML               |
| **Serialization**     | Protocol Buffers (binary, compact)             | JSON/XML (human-readable, less compact)          |
| **Performance**       | High performance, low latency                  | Moderate performance, dependent on JSON/XML      |
| **Setup Complexity**  | Requires `.proto` files and gRPC service setup | Simple setup with REST API endpoints             |
| **Compatibility**     | Primarily backend-to-backend, limited in browsers | Broad compatibility across platforms and browsers |
| **Error Handling**    | Richer error handling with status codes        | Standard HTTP status codes                       |
| **Streaming Support** | Supports bi-directional streaming              | Limited, typically unidirectional (server to client) |
| **Use Cases**         | Real-time, low-latency applications            | Standard web APIs, general-purpose communication |


