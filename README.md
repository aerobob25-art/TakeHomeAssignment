# Running the Application

## Prerequisites

- Windows 10 or later
- Visual Studio 2022 with the .NET desktop development and WinUI application development tooling installed
- .NET 8 SDK
- Go 1.23 or later

## Start the Server

From the repository root, run:

```powershell
go run ./server
```

Leave this terminal running. The server listens at `http://localhost:8080`.

## Start the Client

1. Open `TakeHomeAssignment.sln` in Visual Studio 2022.
2. Set `TakeHomeAssignment` as the startup project if it is not already selected.
3. Select an appropriate target architecture, such as `x64`.
4. Build and run the application with **F5**, or use **Ctrl+F5** to run without the debugger.

Use the **Register** button to request a user ID, then use **Log In** to attempt a login. The supplied server intentionally fails some requests, so retry the appropriate action when the client displays an error.
