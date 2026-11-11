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

Settings are split by environment (later sources override earlier ones: `appsettings.json` -> `appsettings.{Environment}.json` -> user-secrets (Development) -> environment variables):

| File | Contents |
|---|---|
| `Nexam/appsettings.json` | shared defaults only (`Database:ActiveConnection`, logging); no credentials |
| `Nexam/appsettings.Development.json` | all local keys: connection strings, `Email`, `Twilio`, `Pdf` (contains real secrets, keep the repo private) |
| `Nexam/appsettings.Production.json` | the same keys with empty values; fill them on the server with environment variables (`Section__Key`) |

Details per setting:

| Setting | Where |
|---|---|
| `Database:ActiveConnection` | name of the `ConnectionStrings` entry to use (default `DefaultConnection`, the local DB) |
| `ConnectionStrings:Production_94_130_177_96`, `GoDaddy_secureserver`, `Legacy_148_72_232_168`, `Legacy_43_255_152_25` | full strings with passwords: user-secrets |
| `Email:*` (incl. `Password`) | `appsettings.Development.json` (real password; keep the repo private) |
| `Twilio:*` (incl. `AuthToken`) | `appsettings.Development.json` (real token; keep the repo private) |
| `Pdf:IronPdfLicenseKey` | `appsettings.Development.json` (empty until you have a key; needed for PPSC paper PDF download) |

```
dotnet user-secrets --project Nexam list
dotnet user-secrets --project Nexam set "Pdf:IronPdfLicenseKey" "<key>"
```

On a server use environment variables with `__` instead of `:` (e.g. `Twilio__AuthToken`).

Note: the remote databases still use the old `ahmedshair`/`shaira` schemas; move their objects to `dbo` before pointing `Database:ActiveConnection` at them.

## Authentication

Cookie authentication with two roles: `Admin` (all `*ByAdmin`, create/edit/delete pages) and `User` (quizzes and dashboard).

## Past-paper importer

`tools/Nexam.Importer` is a console tool that pulled the 2021-2026 PPSC past papers from gk360.pk into `PPSCPaper` / `PPSCQuestions` (every imported paper is recorded with its source URL in `dbo.ImportedPaper`).

```
cd tools/Nexam.Importer
dotnet run -- crawl --min-year 2021     # polite crawl (robots.txt, 1.5-3s delay), cached in data/cache
dotnet run -- parse                     # cache -> data/staging/*.json
dotnet run -- report                    # dry-run summary: counts, subject mapping, warnings
dotnet run -- crosscheck                # compare answers with questions already on the site
dotnet run -- import                    # dry run; add --commit --create-subjects to write
```

Re-running is safe: papers already in `ImportedPaper` (same URL or same content) are skipped. Answer keys come from a third party and are unverified; spot-check before relying on them.
