# Game Aim & Sensitivity Tracker

## Intended Purpose

The Game Aim & Sensitivity Tracker is a web application I made to help people mouse sensitivity settings across different games. This tool provides a centralized place to track settings.

## Tech Stack

- **Backend**: ASP.NET Core 10.0 (C#) - REST API
- **Frontend**: Vanilla JavaScript, HTML, CSS
- **Testing**: xUnit with WebApplicationFactory

## How to Build and Run

This project consists of a C# .NET Web API backend and a Vanilla HTML/CSS/JavaScript frontend.

### Prerequisites

- .NET 10.0 SDK
- A code editor like Visual Studio Code
- A local web server extension (like Live Server for VS Code)

### Running the Backend API

1. Open your terminal and navigate to the API directory:
   ```bash
   cd TrackerApi
   ```

2. Build and run the project:
   ```bash
   dotnet run
   ```

   The API will start (typically on `http://localhost:5000` or `https://localhost:5001`). Keep this terminal open.

### Running the Frontend

1. Open a new terminal or file explorer and navigate to the Frontend folder:
   ```bash
   cd Frontend
   ```

2. Open the `index.html` file using a local development server (e.g., right-click the file in VS Code and select "Open with Live Server").

3. Ensure the `app.js` file is pointing to the correct localhost port where your .NET API is running.

### Running the Tests

To run the automated xUnit integration tests:

1. Navigate to the tests directory:
   ```bash
   cd TrackerTests
   ```

2. Execute the tests:
   ```bash
   dotnet test
   ```

   The integration tests use WebApplicationFactory to spin up the application in-memory, ensuring the entire routing and model-binding pipeline works correctly.

## What I Learned

Reflecting on this project and the Software Development with C# course, I solidified my understanding of full-stack development and API architecture. Key takeaways include:

- **Backend Engineering**: Designing and implementing a robust RESTful API using C# and .NET, understanding the request/response pipeline and HTTP status codes.
- **Data Manipulation**: Effectively utilizing System.Linq to query and manage data structures within the application logic.
- **Integration Testing**: Writing reliable integration tests using the xUnit framework to ensure API endpoints behave as expected through the entire pipeline.
- **Client-Server Communication**: Successfully connecting a decoupled, vanilla JavaScript frontend to a C# backend using asynchronous fetch calls, handling CORS, and managing JSON data transfer.
- **CORS Configuration**: Understanding cross-origin requests and how to configure CORS policies to allow frontend-backend communication.
- **RESTful API Design**: Implementing CRUD operations following REST conventions and returning appropriate HTTP responses.

## Future Implementations

If I had more time to expand on this project, I would focus on the following enhancements:

- **Persistent Database Integration**: Transitioning from in-memory data storage to a relational database using Entity Framework Core (e.g., SQL Server or SQLite) to ensure user data is permanently saved across sessions.
- **User Authentication**: Adding a login system (such as ASP.NET Core Identity or JWTs) so multiple users can create accounts and securely track their own individual profiles and sensitivities.
- **Data Visualization**: Implementing a charting library (like Chart.js) on the frontend to visually map out aim scores against sensitivity changes over time, helping users identify their statistical "sweet spot."
- **Frontend Framework**: Migrating the vanilla HTML/JS frontend to a modern component-based framework like React or Vue.js for better state management and a more dynamic user interface.
- **Sensitivity Calculator**: A tool to calculate equivalent sensitivity conversions between games (e.g., "What sensitivity should I use in Valorant if my Apex setting is X?").
- **Mobile Responsiveness**: Improve the UI/UX for mobile and tablet devices.
- **Advanced Filtering & Search**: Filter profiles by game name, DPI range, or sensitivity range.
- **Data Export/Import**: Allow users to export their profiles as CSV or JSON and import them on other devices.
- **API Documentation**: Generate interactive API documentation using Swagger/OpenAPI.