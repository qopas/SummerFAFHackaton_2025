# Ediki - Team Management System
## Summer FAF Hackathon 2025

![.NET](https://img.shields.io/badge/.NET-8.0-purple)
![C#](https://img.shields.io/badge/C%23-12.0-blue)
![Entity Framework](https://img.shields.io/badge/Entity%20Framework-Core-orange)
![SignalR](https://img.shields.io/badge/SignalR-Real--time-green)
![Docker](https://img.shields.io/badge/Docker-Containerized-blue)

**Ediki** is a comprehensive team management and collaboration platform built with .NET 8, featuring real-time communication, task management, project tracking, and media recording capabilities.

## 🏗️ Architecture

The project follows **Clean Architecture** principles with clear separation of concerns:

```
├── Ediki.Api/              # Presentation Layer (REST API + SignalR)
├── Ediki.Application/      # Application Layer (Business Logic)
├── Ediki.Domain/          # Domain Layer (Entities & Business Rules)
├── Ediki.Infrastructure/  # Infrastructure Layer (Data Access)
├── Ediki.Tests.Unit/      # Unit Tests
├── Ediki.Tests.Integration/ # Integration Tests
```

## 🚀 Features

### Core Functionality
- **User Authentication & Authorization** - JWT-based secure access
- **Team Management** - Create, manage, and organize teams
- **Project Management** - Full project lifecycle management
- **Task Management** - Create, assign, and track tasks
- **Sprint Management** - Agile sprint planning and tracking
- **Real-time Communication** - SignalR-powered live updates
- **Media Recording** - Screen and audio recording capabilities
- **Notification System** - Real-time notifications and alerts

### API Endpoints

#### Authentication (`/api/v1/auth`)
- `POST /login` - User authentication
- `POST /register` - User registration
- `POST /refresh` - Token refresh
- `POST /logout` - User logout

#### Users (`/api/v1/users`)
- `GET /` - Get all users
- `GET /{id}` - Get user by ID
- `PUT /{id}` - Update user
- `DELETE /{id}` - Delete user

#### Teams (`/api/v1/teams`)
- `GET /` - Get all teams
- `POST /` - Create team
- `GET /{id}` - Get team by ID
- `PUT /{id}` - Update team
- `DELETE /{id}` - Delete team
- `POST /{id}/members` - Add team member
- `DELETE /{id}/members/{userId}` - Remove team member

#### Projects (`/api/v1/projects`)
- `GET /` - Get all projects
- `POST /` - Create project
- `GET /{id}` - Get project by ID
- `PUT /{id}` - Update project
- `DELETE /{id}` - Delete project

#### Tasks (`/api/v1/tasks`)
- `GET /` - Get all tasks
- `POST /` - Create task
- `GET /{id}` - Get task by ID
- `PUT /{id}` - Update task
- `DELETE /{id}` - Delete task
- `PUT /{id}/status` - Update task status
- `PUT /{id}/assign` - Assign task

#### Sprints (`/api/v1/sprints`)
- `GET /` - Get all sprints
- `POST /` - Create sprint
- `GET /{id}` - Get sprint by ID
- `PUT /{id}` - Update sprint
- `DELETE /{id}` - Delete sprint

#### Recordings (`/api/v1/recordings`)
- `GET /` - Get all recordings
- `POST /start` - Start recording
- `POST /stop` - Stop recording
- `GET /{id}` - Get recording by ID
- `DELETE /{id}` - Delete recording

#### Notifications (`/api/v1/notifications`)
- `GET /` - Get user notifications
- `PUT /{id}/read` - Mark notification as read
- `DELETE /{id}` - Delete notification

### Real-time Features (SignalR Hub: `/demohub`)
- Live task updates
- Real-time notifications
- Team collaboration
- Project status updates
- Recording session management

## 🛠️ Technology Stack

### Backend
- **.NET 8** - Latest .NET framework
- **ASP.NET Core** - Web API framework
- **Entity Framework Core** - ORM for database operations
- **SignalR** - Real-time web functionality
- **MediatR** - CQRS and Mediator pattern implementation
- **FluentValidation** - Input validation
- **AutoMapper** - Object-to-object mapping
- **JWT Authentication** - Secure token-based authentication

### Database
- **PostgreSQL** - Primary database
- **Entity Framework Migrations** - Database schema management

### Infrastructure
- **Docker** - Containerization
- **Railway** - Cloud deployment platform
- **Swagger/OpenAPI** - API documentation

### Testing
- **xUnit** - Unit testing framework
- **Integration Tests** - End-to-end API testing

## 🚀 Getting Started

### Prerequisites
- .NET 8 SDK
- PostgreSQL
- Docker (optional)

### Local Development

1. **Clone the repository**
   ```bash
   git clone https://github.com/qopas/SummerFAFHackaton_2025.git
   cd SummerFAFHackaton_2025
   ```

2. **Set up the database**
   ```bash
   # Update connection string in appsettings.Development.json
   # Run migrations
   dotnet ef database update --project Ediki.Infrastructure --startup-project Ediki.Api
   ```

3. **Run the application**
   ```bash
   dotnet run --project Ediki.Api
   ```

4. **Access the API**
   - API: `http://localhost:5000`
   - Swagger UI: `http://localhost:5000/swagger`
   - SignalR Hub: `http://localhost:5000/demohub`

### Docker Deployment

1. **Build and run with Docker Compose**
   ```bash
   docker-compose up --build
   ```

2. **Production deployment**
   ```bash
   docker-compose -f docker-compose.prod.yml up --build
   ```

### Railway Deployment

The application is configured for Railway deployment with:
- Automatic database migrations
- Environment-based configuration
- Health check endpoints
- Railway-optimized Docker configuration

## 🧹 Code Maintenance Tools

### Comment Removal Script (`remove-comments.ps1`)

A sophisticated PowerShell script for removing comments from all C# files while preserving important code elements.

#### Features
- **Intelligent Comment Detection** - Distinguishes between actual comments and legitimate code
- **URL Preservation** - Keeps URLs like `https://api.example.com` intact
- **String Literal Protection** - Preserves content within string literals
- **Context-Aware Processing** - Understands C# syntax context

#### What Gets Removed
- Single-line comments (`// This is a comment`)
- Multi-line comments (`/* This is a comment */`)
- XML documentation comments (`/// <summary>`)
- Comment-only lines

#### What Gets Preserved
- URLs (`https://localhost:5000`, `http://api.example.com`)
- File paths and endpoints (`"/api/v1/users"`, `"/swagger"`)
- String literals containing `//` (`"Visit https://example.com"`)
- Verbatim strings (`@"C:\Path\With\Slashes"`)
- Character literals (`'/'`)
- Regex patterns
- Any `//` or `/* */` within string contexts

#### Usage
```powershell
# Run the script from the project root
powershell -ExecutionPolicy Bypass -File remove-comments.ps1
```

#### Script Output
```
Finding all C# files in the solution...
Found 285 C# files
Processing: C:\Path\To\File1.cs
Processing: C:\Path\To\File2.cs
...
Completed! Processed 285 out of 285 files.
Comments have been removed while preserving URLs, string literals, and other legitimate code content.
```

#### Technical Implementation
The script uses:
- **Character-by-character parsing** for accurate context detection
- **State machine approach** to track string/comment contexts
- **Regex pattern matching** for URL detection
- **Smart whitespace handling** to maintain code formatting

## 📁 Project Structure

### Ediki.Api
- **Controllers/** - REST API endpoints
- **Hubs/** - SignalR hubs for real-time communication
- **DTOs/** - Data transfer objects
- **Services/** - Application services
- **Program.cs** - Application entry point and configuration

### Ediki.Application
- **Features/** - Feature-based organization (CQRS)
- **DTOs/** - Application data transfer objects
- **Interfaces/** - Application contracts
- **Behaviors/** - Cross-cutting concerns (validation, logging)
- **Mappings/** - AutoMapper profiles
- **Validators/** - FluentValidation rules

### Ediki.Domain
- **Entities/** - Domain entities (User, Team, Project, Task, Sprint)
- **Common/** - Shared domain concepts (Result pattern)

### Ediki.Infrastructure
- **Data/** - Entity Framework configuration
- **Repositories/** - Data access implementations
- **Services/** - Infrastructure services

## 🔧 Configuration

### Environment Variables
- `DATABASE_URL` - PostgreSQL connection string
- `JWT_SECRET` - JWT signing key
- `PORT` - Application port (Railway)
- `RAILWAY_GIT_COMMIT_SHA` - Git commit hash (Railway)

### Application Settings
```json
{
  "ApiSettings": {
    "EnableSwagger": true
  },
  "ConnectionStrings": {
    "DefaultConnection": "..."
  },
  "JwtSettings": {
    "Secret": "...",
    "ExpirationInMinutes": 60
  }
}
```

## 🧪 Testing

### Run Unit Tests
```bash
dotnet test Ediki.Tests.Unit
```

### Run Integration Tests
```bash
dotnet test Ediki.Tests.Integration
```

### Run All Tests
```bash
dotnet test
```

## 📊 Health Monitoring

The application includes several health check endpoints:

- `GET /health` - Application health status
- `GET /` - Service information and available endpoints
- `GET /version` - Version and build information

## 🤝 Contributing

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📝 License

This project was developed for the Summer FAF Hackathon 2025.

## 👥 Team

Developed by the Ediki Team for Summer FAF Hackathon 2025.

---

**Note**: This project demonstrates modern .NET development practices including Clean Architecture, CQRS, real-time communication, and cloud deployment strategies.
