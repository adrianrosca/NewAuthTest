
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

## Technologies Used

- .NET 8.0
- ASP.NET Core
- WPF
- Windows Authentication
- HTTP Client

## gRPC vs HTTP Client

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

## SID

<style>
.nowrap { white-space: nowrap; }
</style>

| SID                                   | Group Name                                      | Description |
|---------------------------------------|-------------------------------------------------|-------------|
| <span class="nowrap">**S-1-5-32-544**</span> | BUILTIN\Administrators                          | Local Administrators group. Members have administrative privileges on the machine. |
| <span class="nowrap">**S-1-1-0**</span>      | Everyone                                        | Represents all users. All logged-on users are members of this group. |
| <span class="nowrap">**S-1-5-114**</span>    | Local account and member of Administrators group| Identifies accounts that are both local and members of the Administrators group. |
| <span class="nowrap">**S-1-5-32-545**</span> | BUILTIN\Users                                   | Local Users group. Members are standard users with limited privileges. |
| <span class="nowrap">**S-1-5-4**</span>      | NT AUTHORITY\INTERACTIVE                        | Represents users who log on interactively. |
| <span class="nowrap">**S-1-2-1**</span>      | CONSOLE LOGON                                   | Refers to users logged in through the console, such as directly on the physical machine. |
| <span class="nowrap">**S-1-5-11**</span>     | NT AUTHORITY\Authenticated Users                | Includes all users authenticated on the machine. |
| <span class="nowrap">**S-1-5-15**</span>     | NT AUTHORITY\This Organization                  | Used by users authenticated by the organization. |
| <span class="nowrap">**S-1-11-96...**</span> | MicrosoftAccount\<email>                        | Represents a user authenticated with a Microsoft Account. This SID is specific to the user’s Microsoft account. |
| <span class="nowrap">**S-1-5-113**</span>    | NT AUTHORITY\Local account                      | Identifies accounts that are local to the machine (non-domain). |
| <span class="nowrap">**S-1-2-0**</span>      | LOCAL                                           | Represents the machine’s local account (as opposed to network-based users). |
| <span class="nowrap">**S-1-5-64-36**</span>  | NT AUTHORITY\Cloud Account Authentication       | Refers to users authenticated with cloud-based identity, such as Azure AD or Microsoft Accounts. |
