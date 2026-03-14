# TaskSphere

TaskSphere is a lightweight, team-focused ticket manager built on .NET 8.

## Features
- Create, assign, and track tasks
- Projects, priorities, and statuses
- Simple REST API for integration
- Role Based Functionalities (Admin/User)
- Swagger Documentation


## Quick start
1. Clone repo
2. `dotnet run` (from project folder)
3. Open http://localhost:5000

### Project Structure

TaskSphere.sln
│
├── TS.Contract/
│   ├── DTOs (Auth, Comment, Task)
│   ├── Enums
│   └── TS.Contract.csproj
│
├── TS.Model/
│   ├── Data (AppDbContext.cs)
│   ├── Entities (Auth, Base, Comment, Task)
│   └── TS.Model.csproj
│
├── TS.ServiceLogic/
│   ├── Common (Utility.cs)
│   ├── Interfaces (IAuth, IComment, ITask)
│   ├── Services (Auth, Comment, Task)
│   └── TS.ServiceLogic.csproj
│
└── TS.WebAPI/
    ├── Controllers (Auth, Comment, Task)
    ├── Program.cs
    └── TS.WebAPI.csproj
