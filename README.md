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

---

# Tailscale Take Home Challenge (Windows UI)

This challenge is a basic account management server. Clients can
register an account and login with the server and present the results in 
a simple GUI.

Your task is to implement the client against the provided server.

There is no authentication, session management, or cookies in this challenge to
keep it simple. We don't expect you to implement those.

## Control plane

The `server` directory contains a somewhat unreliable implementation of the
account management server. It implements 2 HTTP endpoints:

* `/register` - registers a new user and returns its ID, e.g. `{"user_id": 123}`
* `/login` - logs in as the user with the provided ID as a URL parameter, e.g. `/login?user_id=123`

The server is intentionally flaky and buggy. Use the server as-is and do not
change it.

### Running the server

You'll need to have [Go](https://go.dev/) installed in order to run the server.
You can run it from the root of the repository like this:

```
go run ./server
```

### Client

Implement a GUI application that does the following:

- call `/register` to get a user ID
- call `/login` to login as the user
    - if server returns a `404 Not Found` error, render the error and allow the user to retry
- render the client states such as "Registered", "Unregistered", "Logged in" in the GUI
- optional: render transient client states such as "Registering" or "Retrying"

The UI should include buttons to trigger the expected actions, and some simple state and error handling and rendering. You may get as creative as you wish but the focus should be on high quality, well structured and bug free code.

### What we're looking for

We expect a working client that eventually succeeds with a login and renders the result. An **ideal** submission will:

- have clean and readable code
- the minimum requirement is for the client to work against the provided server
- a working GUI with register/login buttons that renders client state and errors
- unit tests are a plus, but we don't expect 100% coverage
- have some amount of automated testing
- have no memory safety or concurrency bugs

After you submit your response, you will participate in a 1 hour interview with one or two Tailscale engineers. The first half of the interview will give you an opportunity to present your solution, and if there are any lingering problems, work through fixing them. The second half of the interview will be a pair-programming session with your interviewers to expand on your solution with new requirements.

### Confidential

Tailscale are big believers in Open Source - but good take home interviews take time to create and polish. Please help us in keeping this interview scenario confidential. Do not post this code or any information about this exercise publicly.

### Feedback

If you have any feedback about this challenge, good or bad, feel free to add a
`feedback.md` file with your submission. This helps us iterate on the challenge
and improve it for future candidates.
