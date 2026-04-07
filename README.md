# Blazor Web Template

Enterprise-ready Blazor Web App starter built on `.NET 8`, `MudBlazor`, cookie authentication, and a clean `Web -> Client -> Shared` dependency flow.

## What Changed

- Brandable corporate shell with configuration-backed theme tokens under `Branding`.
- Reusable page primitives for headers, cards, state handling, tables, stat cards, toolbars, and confirmation panels.
- Enterprise client defaults with validated options, correlation IDs, and retry-aware outbound HTTP clients.
- URL-driven post list filters for search, tag, sorting, direction, and page size.
- Safer content workflows with unsaved-change protection and stronger destructive confirmation.

## Project Structure

- `BlazorWebTemplate.Web`
  Corporate shell, pages, reusable UI primitives, theme/config, and Blazor composition.
- `BlazorWebTemplate.Client`
  Service orchestration, API clients, auth/session handling, retry handlers, and mapping.
- `BlazorWebTemplate.Shared`
  Contracts, auth constants, paging/query models, and shared enums.
- `BlazorWebTemplate.Tests`
  Unit and architecture tests for contracts, mapping, and enterprise defaults.

## Run

```bash
cd BlazorWebTemplate.Web
dotnet run
```

Default app URL: `http://localhost:5000`

Demo credentials:

- Username: `emilys`
- Password: `emilyspass`

## Configuration

`BlazorWebTemplate.Web/appsettings*.json` contains two important sections:

### `Branding`

Use this to white-label the shell without changing component code.

```json
"Branding": {
  "ProductName": "Unictive Control Center",
  "ProductTagline": "Enterprise content operations",
  "ProductDescription": "A reusable admin shell for content, workflow, and team operations.",
  "CompanyName": "Unictive",
  "SupportEmail": "support@unictive.example",
  "PrimaryColor": "#1F4FD8",
  "SecondaryColor": "#0F172A",
  "AccentColor": "#14B8A6"
}
```

### `DummyJson`

Controls the demo backend endpoint and retry/timeout behavior.

```json
"DummyJson": {
  "BaseUrl": "https://dummyjson.com/",
  "RequestTimeoutSeconds": 20,
  "RetryCount": 2,
  "RetryDelayMilliseconds": 250
}
```

## Reusable UI Primitives

The main enterprise UI building blocks live under `BlazorWebTemplate.Web/Components/Primitives/`.

- `AppPageHeader`
- `AppSectionCard`
- `AppToolbar`
- `AppEmptyState`
- `AppStateView`
- `AppStatCard`
- `AppConfirmPanel`
- `AppDataGridShell<T>`

Use these first before adding page-specific wrappers.

## Adding A New Module

1. Add contracts to `Shared` if the feature needs cross-layer models.
2. Add orchestration and API access to `Client`.
3. Build the page in `Web/Features/<Module>/` using the shared primitives.
4. Register route links in `Layout/SidebarNav.razor`.
5. Prefer query-driven list state for filters, sorting, and pagination.
6. Keep HTTP, retries, and auth token handling out of Razor components.

## Enterprise Defaults

- `AddClient(...)` wires options validation, correlation IDs, retry handling, token handling, and backend services.
- `AddEnterpriseWebServices(...)` wires auth policies, branding validation, and the shell state service.
- `AppShellState` centralizes drawer state, page context, and notification hooks.

## Testing

```bash
dotnet test
```

Current test coverage focuses on:

- architecture boundaries
- shared query logic
- role mapping
- enterprise defaults and shell behavior
