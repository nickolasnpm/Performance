# Performance Project

This project is a .NET-based web API to test the performance optimization strategy. Among the strategy tested are:

1. Offset pagination vs Cursor pagination.
2. Batch processing for Create, Update, & Delete.

## A. Architecture

This project follows a monolith architecture for development speed and simplicity whereby the folder is arranged accordingly with the class libraries in Clean Architecture.

## B. Design Patterns

- **Central Package Management (CPM)** - To manage common dependencies for many different projects
- **Dependency Injection**: Used throughout the application for loose coupling and testability.
- **Repository Pattern**: Abstracts data access logic, providing a consistent interface for querying and persisting entities.
- **Unit of Work Pattern**: Manages transactions and ensures data consistency across multiple repositories.
- **Facade Pattern**: Provide simplified and single interface for complex ecosystem, especially repositories via `IUnitOfWork`.
- **Result Pattern**: To handle *expected business logic* outcomes explicitly by returning an object that encapsulates success, failure, and any returned data
- **Error Controller**: To handle *unexpected exceptions* (500, and mapped exception types) with `Problem Details`

## C. Deployment Management

- **Migration Strategy**: Migrations will be run during app startup for non-production environment. It will be run in CD pipeline for production environment.

## D. Technologies and Tools

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
- **Security**:
  - Pipeline security scanning using Snyk
- **Build Tools**: MSBuild, NuGet for package management

## E. Getting Started

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

## F. API Endpoints

- `GET /api/Users` - Retrieve list of users
- `GET /api/User/{id}` - Retrieve single user
- `POST /api/Users` - Create a new user
- `PUT /api/Users` - Update user
- `DELETE /api/Users` - Delete user

Refer to `Performance.http` for sample requests.

## G. Remarks

All the below decision is made for development speed and simplicity, and may not follow the enterprise-level standard practice.

- Database entity's constraints are set by using data annotation instead of configuration class
- Data validation in http request object is checked by using data annotation instead of fluent validation
- Manual mapping from database entity to DTOs instead of using external library such as Automapper
- Using custom in-memory cache implementation instead of external caching tool or library at all

## H. Things to do

- [Infrastructure as code (IaC)](https://learn.microsoft.com/en-us/devops/deliver/what-is-infrastructure-as-code) - To enforce consistency by representing desired environment states via well-documented code
