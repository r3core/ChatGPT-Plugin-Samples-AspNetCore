# ChatGPT Plugin Samples for .NET

This repository contains .NET samples for building ChatGPT plugins using C# and minimal APIs. The examples are based on
OpenAI's [ChatGPT plugin examples](https://platform.openai.com/docs/plugins/examples) and have been adapted for the .NET
platform.

## Solution Structure

The solution consists of a single project, `Gpt.Plugins.TodoList`, which contains a minimal API implementation for a
simple Todo list plugin for ChatGPT, with no authentication.

### Gpt.Plugins.TodoList

This project demonstrates how to build a simple ChatGPT plugin using .NET 7 and C# 11 with minimal APIs, top-level
statements, and records.

## Getting Started

To run the Gpt.Plugin.TodoList project, follow these steps:

1. Clone the repository:

   ```bash
   git clone https://github.com/r3core/chatgpt-plugin-samples-dotnet.git
   ```

2. Navigate to the `Gpt.Plugins.TodoList` directory:

    ```bash
    cd chatgpt-plugin-samples-dotnet/Gpt.Plugins.TodoList
    ```

3. Run the project:

   ```bash
   dotnet run
   ```
   This will launch the minimal API on the default port, usually 5000 for HTTP and 5001 for HTTPS.


4. Test the API endpoints using a tool like [Postman](https://www.postman.com/) or [curl](https://curl.se/). The
   available endpoints are:

- POST `/todos/{username}`
- GET `/todos/{username}`
- DELETE `/todos/{username}`

Please note that this example is intended for educational purposes and should not be used as-is for production
applications.
