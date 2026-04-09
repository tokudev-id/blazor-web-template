# Blazor Web Template

Enterprise-ready Blazor Web App starter built on `.NET 8`, `MudBlazor`, cookie authentication, and a clean `Web → Client → Shared` dependency flow. Backed by the [Unictive Backend API](https://github.com/tokudev-id/boilerplate-dotnet) (`boilerplate-dotnet`).

## Features

- Cookie auth with JWT token refresh, route protection, and role-aware navigation
- RBAC management — create/delete roles, assign permissions, assign roles to users
- User management — paginated list with search, edit (name/phone), delete with confirmation
- Dashboard — live user stats (total, active, inactive) and role distribution
- Brandable shell with configuration-backed color palette and product identity
- Reusable page primitives for headers, stat cards, tables, confirmation dialogs, and state views
- Correlation IDs, retry handling, and validated options across all HTTP clients
- Health check endpoint at `/health`

## Project Structure

```
BlazorWebTemplate.sln
├── BlazorWebTemplate.Web      → Blazor UI, routes, layouts, feature pages, Web services
├── BlazorWebTemplate.Client   → Feature services, HTTP API clients, auth/session/transport
├── BlazorWebTemplate.Shared   → Stable contracts, query models, auth constants, paged results
└── BlazorWebTemplate.Tests    → xUnit tests (53) — architecture, state, client, shared logic
```

Dependency direction: `Shared ← Client ← Web`

## Prerequisites

The Unictive Backend API must be running before you start the web app.

```bash
# 1. Start the backend API
cd /path/to/boilerplate-dotnet
dotnet run --project src/Unictive.API
# → https://localhost:7001  (Swagger at /swagger)

# 2. Register a user via Swagger or seed the database

# 3. Start the web app
cd /path/to/BlazorWebTemplate
dotnet run --project BlazorWebTemplate.Web
# → https://localhost:7052
```

Login with the email and password you registered in the backend.

## Configuration

All configurable values live in `BlazorWebTemplate.Web/appsettings.json`. Invalid values fail startup immediately (validated with `ValidateOnStart`).

### `UnictiveApi`

HTTP client settings for the backend API.

```json
"UnictiveApi": {
  "BaseUrl": "https://localhost:7001/",
  "RequestTimeoutSeconds": 20,
  "RetryCount": 2,
  "RetryDelayMilliseconds": 250,
  "DefaultPageSize": 20
}
```

### `AppAuth`

Authentication cookie behavior.

```json
"AppAuth": {
  "CookieName": "BlazorWebTemplate.Auth",
  "ExpireTimeSpanHours": 8
}
```

### `AppDashboard`

Dashboard cache freshness window.

```json
"AppDashboard": {
  "FreshnessMinutes": 2
}
```

### `Branding`

White-label the shell without touching component code.

```json
"Branding": {
  "ProductName": "Unictive Control Center",
  "ProductTagline": "Enterprise content operations",
  "CompanyName": "Unictive",
  "SupportEmail": "support@unictive.example",
  "PrimaryColor": "#1F4FD8",
  "SecondaryColor": "#0F172A",
  "AccentColor": "#14B8A6"
}
```

## Reusable UI Primitives

Shared components live in `BlazorWebTemplate.Web/Common/Components/`.

| Component | Purpose |
|---|---|
| `PageHeader` | Page title, subtitle, section label, breadcrumbs |
| `PageSection` | Titled content card |
| `StatCard` | Metric tile (icon, label, value, description) |
| `DataGridShell` | Table wrapper with toolbar and footer slots |
| `StateView` | Error and empty state display |
| `Loading` | Skeleton/spinner placeholder |
| `ConfirmPanel` | Confirmation content block with optional warning |
| `ConfirmDialog` | Reusable confirm dialog wrapping `ConfirmPanel` |

Use these before adding page-specific wrappers.

## Adding a New Module

1. Add contracts to `Shared` if the feature needs cross-layer models.
2. Add API client and service to `Client/Services/BackEnd/<Module>/`.
3. Create the feature folder at `Web/Features/<Module>/` with:
   - `Constants/RouteFor.cs` — route constants
   - `Constants/BreadcrumbFor.cs` — breadcrumb helpers
   - `_Imports.razor` — feature-scoped `@using` directives
   - `Pages/` and `Components/` as needed
4. Register the nav link in `Layouts/NavMenu.razor`.
5. Add a type alias to `Layouts/_Imports.razor` if `RouteFor` name collides with another feature.

## Testing

```bash
dotnet test
```

53 tests covering architecture boundaries, shared contract logic, route builders, pagination, client services, and Web state.
