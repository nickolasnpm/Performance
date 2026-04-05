# A. Performance

A .NET-based web API to test the performance optimization strategy. Among the strategy tested are:

1. Offset pagination vs Cursor pagination.
2. Batch processing for Create, Update, & Delete.

## B. Architecture

This project follows a monolith architecture for development speed and simplicity whereby the folder is arranged accordingly with the class libraries in Clean Architecture.

- **Central Package Management (CPM)** - To manage common dependencies for many different projects

## C. Design Patterns

- **Dependency Injection**: Used throughout the application for loose coupling and testability.
- **Repository Pattern**: Abstracts data access logic, providing a consistent interface for querying and persisting entities.
- **Unit of Work Pattern**: Manages transactions and ensures data consistency across multiple repositories.
- **Facade Pattern**: Provide simplified and single interface for complex ecosystem, especially repositories via `IUnitOfWork`.

## D. Deployment Management

- **Migration Strategy**: Migrations will be run during app startup for non-production environment. It will be run in CD pipeline for production environment.

## E. Technologies and Tools

- **Framework**: .NET 10 (C#)
- **Web Framework**: ASP.NET Core Web API
- **ORM**: Entity Framework Core with SQL Server
- **Querying**: OData for flexible API queries
- **Containerization**: Docker
- **CI/CD**: Azure DevOps Pipelines (with separate dev and prod pipelines)
- **Testing**:
  - Unit Tests (xUnit)
  - Integration Tests (with database fixtures)
  - Performance Benchmarks (BenchmarkDotNet)
- **Build Tools**: MSBuild, NuGet for package management

## F. Getting Started

1. **Prerequisites**:
   - .NET 10 SDK
   - SQL Server (or compatible database)
   - Docker (for containerized deployment)

2. **Clone the Repository**:

   - `git clone <repository-url>`

3. **Restore Dependencies**:

   - `dotnet restore`

4. **Run Database Migrations**:

   - `dotnet ef database update`

5. **Run the Application**:

   - `dotnet run --project Performance`

6. **Run Tests**:
   - Unit Tests: `dotnet test UnitTest`
   - Integration Tests: `dotnet test IntegrationTest`
   - Benchmarks: `dotnet run --project BenchmarkSuite`

7. **Build and Deploy**:
   - Use Azure DevOps pipelines for automated builds and deployments.
   - For local Docker build: `docker build -t performance .`

## G. API Endpoints

- `GET /api/users` - Retrieve users
- `POST /api/users` - Create a new user
- `PUT /api/users` - Update user
- `DELETE /api/users` - Delete user

Refer to `Performance.http` for sample requests.

## H. Remarks

All the below decision is made for development speed and simplicity, and may not follow the enterprise-level standard practice.

- Database entity's constraints are set by using data annotation instead of configuration class
- Data validation in http request object is checked by using data annotation instead of fluent validation
- Manual mapping from database entity to DTOs instead of using external library such as Automapper
- Using custom in-memory cache implementation instead of external caching tool or library at all

## I. Things to do

- [Infrastructure as code (IaC)](https://learn.microsoft.com/en-us/devops/deliver/what-is-infrastructure-as-code) - To enforce consistency by representing desired environment states via well-documented code
