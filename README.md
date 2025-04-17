# 🛒 Furni Ecommerce Website

**Furni** is a modular, scalable, and professional eCommerce platform built using **ASP.NET MVC (Framework)**.  
It offers a seamless online shopping experience for furniture with a modern **customer-facing storefront** and a secure **admin dashboard**.  
The application follows clean architecture principles for maximum maintainability and scalability.

---

## 📦 Solution Architecture

The solution uses a **multi-project structure** to promote separation of concerns and clean code organization:

```
/FurniEcommerceSolution
│
├── /UserWebsite       → Frontend MVC app for customers
├── /AdminDashboard    → Backend MVC panel for administrators
├── /CoreLibrary       → Shared services, models, DTOs
├── /DataAccess        → Entity Framework Core DbContext, repositories, migrations
```

- ✅ **Single SQL Server database** shared between user and admin apps
- 🔒 **Separate authentication flows** for users and administrators
- 🧱 Built on **clean architecture** with a layered approach

---

## 🚀 Features

### 👤 User Website

- Home, product listing, and product detail pages
- Fully functional shopping cart and checkout flow
- User registration and secure login
- Razor Views with responsive Bootstrap-based UI
- Role-based access control for users

### 🛠 Admin Dashboard

- Secure admin login with role-based access
- Dashboard for managing:
  - 🛋 Products
  - 🗂 Categories
  - 📦 Orders
  - 👥 Users & roles
- Full CRUD operations with form validation
- Admin-only route protection

### 🧠 Core Library

- Centralized business logic
- Shared interfaces and services
- DTOs for structured data flow between layers

### 💾 Data Access Layer

- **Entity Framework 6 (EF6)** with Code-First approach
- Repository pattern for abstraction
- Lazy loading support enabled
- Migrations and centralized DbContext
- SQL Server database integration

---

## 🔐 Authentication & Authorization

- Built with **ASP.NET Identity**
- Role-based access control for `User` and `Admin`
- Separate login views and routes
- Views and actions protected via `[Authorize(Roles = "...")]`

---

## ⚙️ Dependency Injection

- Implemented using `Microsoft.Extensions.DependencyInjection`
- All services and repositories registered in `Startup.cs`
- Organized via a shared `ServiceConfigurator` for clean initialization
- Constructor injection across controllers and services

---

## 🧪 Entity Framework Setup

Furni uses **Entity Framework 6** with Code-First and migration support.

### 🔧 Configuration Example

```csharp
services.AddDbContext<FurniDbContext>(options =>
{
    options.UseLazyLoadingProxies()
           .UseSqlServer(configuration.GetConnectionString("cs"),
               sql => sql.MigrationsAssembly("DataAccess"));
});
```

### 🗃 Running Migrations

Using **Package Manager Console (PMC)**:

```powershell
Add-Migration InitialCreate -Project DataAccess -StartupProject UserWebsite
Update-Database -Project DataAccess -StartupProject UserWebsite
```

Using **.NET CLI**:

```bash
dotnet ef migrations add InitialCreate --project DataAccess --startup-project UserWebsite
dotnet ef database update --project DataAccess --startup-project UserWebsite
```

---

## 🌍 Hosting & Deployment

| App              | Example URL               |
|------------------|---------------------------|
| User Website     | `https://www.furni.com`   |
| Admin Dashboard  | `https://admin.furni.com` |

- Can be hosted via **IIS**, **Azure App Services**, or any .NET-compatible hosting
- Single database instance for both portals
- Projects are deployable independently

---

## 🧰 Tech Stack

| Layer             | Technology                            |
|-------------------|----------------------------------------|
| Frontend          | Razor Views, Bootstrap                |
| Backend           | ASP.NET MVC (.NET Framework)          |
| ORM               | Entity Framework 6 (EF6)              |
| Authentication    | ASP.NET Identity                      |
| Dependency Injection | Microsoft.Extensions.DependencyInjection |
| Database          | Microsoft SQL Server                  |

---

## ▶️ Getting Started

### ✅ Prerequisites

- Visual Studio 2019 or later
- .NET Framework 4.7.2 or higher
- SQL Server (Express or Full)
- NuGet Package Manager / .NET CLI

### ⚙️ Installation Steps

1. **Clone the Repository**

   ```bash
   git clone https://github.com/yourusername/furni-ecommerce-website.git
   ```

2. **Configure Connection Strings**

   Update SQL connection strings in the following files:
   - `UserWebsite\web.config`
   - `AdminDashboard\web.config`

3. **Apply Migrations**

   Run EF migration commands from the **DataAccess** project with **UserWebsite** as the startup project.

4. **Run the Application**

   - Set `UserWebsite` or `AdminDashboard` as the startup project in Visual Studio
   - Press `F5` or `Ctrl + F5` to build and run

---

## 📷 Screenshots (Optional)

Include screenshots of:
- User storefront
- Product details
- Admin panel
- Order management
- Login/register forms

---

## 🙌 Contributing

We welcome community contributions to improve Furni!

### How to Contribute:

1. Fork this repository
2. Create a new feature/bugfix branch
3. Commit your changes
4. Push to your forked repo
5. Submit a Pull Request

Please open issues for bugs, feature requests, or enhancement ideas.

---

## 📝 License

This project is licensed under the **MIT License**.  
See the [LICENSE](./LICENSE) file for full license information.

---

## 💬 Contact

- 📧 Email: [youremail@example.com](mailto:youremail@example.com)
- 🌐 Website: [yourwebsite.com](https://yourwebsite.com)

---
