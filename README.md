# Nexam

Exam-preparation site (MCQs, CSS / PPSC / NTS / FPSC past papers, quizzes, forum) built with **ASP.NET Core MVC on .NET 10**, **Dapper** and SQL Server.

## Structure

One project (`Nexam/`), layered by folder. Dependencies only point downward:

```
Controllers  ->  Business (services)  ->  DataAccess (Dapper repositories)  ->  SQL Server
     |                 |                          |
     +---------------- Models (entities + view models) ----------------+
Infrastructure: DI registration, routes, auth helpers, PDF + email
```

| Folder | Responsibility |
|---|---|
| `Controllers/` | HTTP only: bind input, call a service, return a view/JSON |
| `Business/` | Use-case logic (quiz scoring, option parsing, PDF building). `Interfaces/` holds the contracts |
| `DataAccess/` | Dapper repositories. Stored procedures through `*ProcAsync`, raw SQL through `*SqlAsync` (`DapperRepository`) |
| `Models/` | `Entities/` (database shapes) and `ViewModels/` (what views bind to) |
| `Infrastructure/` | `AddNexam()` DI wiring, `MapNexamRoutes()`, cookie-auth helpers, IronPdf and SMTP adapters |
| `Views/`, `wwwroot/` | Razor views and static files |

## Run

```
dotnet run --project Nexam
```

Needs the `Nexam` database. The default connection string targets `(localdb)\MSSQLLocalDB`; change `ConnectionStrings:DefaultConnection` in `Nexam/appsettings.json` (or override it with user-secrets / the `ConnectionStrings__DefaultConnection` environment variable).

## Configuration (keep secrets out of git)

```
dotnet user-secrets --project Nexam set "Pdf:IronPdfLicenseKey" "<key>"   # PPSC paper PDF download
dotnet user-secrets --project Nexam set "Email:UserName" "<smtp user>"
dotnet user-secrets --project Nexam set "Email:Password" "<smtp password>"
dotnet user-secrets --project Nexam set "Email:AdminAddress" "<admin email>"
```

## Authentication

Cookie authentication with two roles: `Admin` (all `*ByAdmin`, create/edit/delete pages) and `User` (quizzes and dashboard).
