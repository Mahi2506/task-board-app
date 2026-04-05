1. Task Board Application

A mini Task Management (Task Board) application built as part of a
coding assignment to demonstrate skills in:

-   .NET 8 Web API
-   Angular
-   Entity Framework Core
-   SQLite Database
-   JWT Authentication

------------------------------------------------------------------------

2. Tech Stack

Backend

-   ASP.NET Core Web API (.NET 8)
-   Entity Framework Core
-   SQLite
-   JWT Authentication
-   Custom Middleware

Frontend

-   Angular
-   TypeScript
-   HTML / CSS

------------------------------------------------------------------------

3. Features Implemented

 Authentication

-   User Registration with validation
-   User Login with JWT Token
-   Secure API endpoints using Bearer Token
-   Password validation (basic + strong password rules)

Task Management

-   Create Task
-   Get All Tasks
-   Update Task
-   Delete Task
-   Mark Task as Complete
-   Task status handling

Architecture & Design

-   Controller / Service / Repository pattern

Backend Enhancements

-   EF Core with SQLite
-   Global Exception Handling (Custom Middleware)
-   CORS enabled
-   DTO usage
-   Validation handling

Frontend Features

-   Login / Register / Task Board pages
-   API integration
-   Loading indicators
-   Error handling
-   Basic responsive UI

------------------------------------------------------------------------

4. Setup Instructions

Backend

``` bash
cd TaskBoard.API
dotnet restore
dotnet ef database update
dotnet run
```

Frontend

``` bash
cd taskboard-ui
npm install
ng serve
```

------------------------------------------------------------------------

5. API Endpoints

Auth

-   POST /api/auth/register\
-   POST /api/auth/login

Tasks

-   GET /api/tasks\
-   POST /api/tasks\
-   PUT /api/tasks/{id}\
-   DELETE /api/tasks/{id}

------------------------------------------------------------------------

6. Authentication

Use JWT token in header:

Authorization: Bearer `<token>`{=html}

------------------------------------------------------------------------

7. Design Decisions

-   SQLite for lightweight DB
-   Repository pattern for maintainability
-   Service layer for business logic
-   DTOs for clean responses
-   Middleware for centralized error handling

------------------------------------------------------------------------

8. Completed

-   JWT Authentication
-   Task CRUD
-   Angular integration
-   Error handling
-   Clean architecture

------------------------------------------------------------------------

9. Not Completed

-   Role-based auth
-   Pagination
-   Email verification

------------------------------------------------------------------------

10. Future Enhancements

-   Email OTP verification (SMTP)
-   Refresh tokens
-   Role-based access
-   Filtering & pagination
-   Notifications

------------------------------------------------------------------------


