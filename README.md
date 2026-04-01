# 🚀 Blazor Web Template

A modern, high-performance Blazor Web App template built with **.NET 8** and **MudBlazor**. This template is designed with a clean architecture, featuring a modular "Vertical Slice" aesthetic and pre-configured authentication.

---

## ✨ Features

- **.NET 8 Blazor Web App**: Leverages the latest features including Interactive Server components.
- **MudBlazor UI**: Integrated with MudBlazor 9.2.0 for a premium, responsive Material Design interface.
- **Custom Theming**: Includes a built-in `EditorialMudTheme` for a unique, polished look.
- **Vertical Slice Organization**: Features and components are logically organized to improve maintainability.
- **Auth Ready**: Pre-configured Cookie-based authentication with `AuthenticationStateProvider`.
- **API Integration**: Example integration with `DummyJson` for users and authentication.
- **Test Suite**: Includes a dedicated test project with `xUnit` for both project structure and logic validation.

---

## 📂 Project Structure

The solution is divided into four main projects:

- **`BlazorWebTemplate.Web`**: The main entry point. Contains UI components, layouts, features, and theme definitions.
- **`BlazorWebTemplate.Backend`**: Domain logic, services, and external API clients.
- **`BlazorWebTemplate.Shared`**: Shared models, DTOs, and constants used across both Web and Backend.
- **`BlazorWebTemplate.Tests`**: Automated tests (xUnit) ensuring architecture integrity and feature correctness.

---

## 🛠️ Tech Stack

- **Framework**: .NET 8.0
- **UI Framework**: [MudBlazor](https://mudblazor.com/)
- **State Management**: Scoped Services & AuthenticationStateProvider
- **Authentication**: Cookie Authentication
- **Testing**: xUnit, Coverlet
- **External Data**: DummyJSON (Sample)

---

## 🚀 Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

### Running the Project

1. **Clone the repository** (if applicable).
2. **Navigate to the web project**:
   ```bash
   cd BlazorWebTemplate.Web
   ```
3. **Run the application**:
   ```bash
   dotnet run
   ```
4. Open your browser and navigate to `http://localhost:5000` (or the port specified in your console).

---

## 📝 Key Components

- **Theme Engine**: Check `BlazorWebTemplate.Web/Theme/EditorialMudTheme.cs` to customize colors and typography.
- **Auth Logic**: Services are defined in `BlazorWebTemplate.Backend/Auth` and registered in `Program.cs`.
- **Sidebar**: The navigation menu is centrally managed in `Layout/SidebarNav.razor`.

---

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.
