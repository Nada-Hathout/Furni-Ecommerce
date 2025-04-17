
# 🛒 Furni Ecommerce Website

**Furni** is a modular, scalable, and professional eCommerce platform built using **ASP.NET MVC (Framework)**.  
It offers a seamless online shopping experience for furniture with a modern **customer-facing storefront** and a secure **admin dashboard**.  
The application follows **clean architecture principles** for maintainability, scalability, and separation of concerns.

---

## 📦 Solution Architecture

The solution is structured into multiple projects with clear separation between presentation, business logic, and data access.

```plaintext
/FurniEcommerceSolution
│
├── Furni-Ecommerce-Website       → MVC frontend for customers
├── Furni-Ecommerce-DashBoard     → MVC admin dashboard
├── BusinessLogic                 → Application layer (services, managers)
├── DataAccess                    → EF6 DbContext, repositories, migrations
├── Furni_Ecommerce_Shared        → Shared DTOs, enums, interfaces
```

### 🔗 Project Reference Diagram

```plaintext
[Furni-Ecommerce-Website]
   ├── references → BusinessLogic
   └── references → Furni_Ecommerce_Shared

[Furni-Ecommerce-DashBoard]
   ├── references → BusinessLogic
   └── references → Furni_Ecommerce_Shared

[BusinessLogic]
   ├── references → DataAccess
   └── references → Furni_Ecommerce_Shared

[DataAccess]
   └── references → Furni_Ecommerce_Shared

[Furni_Ecommerce_Shared]
   └── No project dependencies (pure shared layer)
```

### ✅ Highlights

- Shared **SQL Server database** for all apps
- Clean separation of business logic, UI, and data
- Role-based access with **user/admin flows**
- Each project can be independently deployed and maintained

---

## 🚀 Features

### 👤 User Website

- Home, product listings, and detail views
- Shopping cart & checkout flow
- User registration and login
- Responsive Razor views using Bootstrap
- Role-based access control

### 🛠 Admin Dashboard

- Secure login for admins
- Admin dashboard with:
  - 🛋 Product Management
  - 🗂 Category Management
  - 📦 Order Management
  - 👥 User & Role Management
- CRUD operations with validation and error handling

### 🧠 Business Logic Layer

- Centralized service classes for all operations
- Clean abstraction of business rules
- Reusable service interfaces
- Handles business validation and orchestration

### 💾 Data Access Layer

- **Entity Framework 6 (EF6)** with Code-First
- Repository & Unit of Work patterns
- SQL Server integration
- Lazy loading enabled
- Centralized migrations

### 🧩 Shared Layer

- DTOs and view models
- Role enums and constants
- Interfaces for services and repositories
- Zero dependencies – usable across all layers

---

## 🔐 Authentication & Authorization

- Powered by **ASP.NET Identity**
- Role-based access (`User`, `Admin`)
- Separate login endpoints for customers and admins
- Route and view protection using `[Authorize]`

---

## ⚙️ Dependency Injection

- Powered by `Microsoft.Extensions.DependencyInjection`
- All services and repositories registered in `Startup.cs`
- Shared `ServiceConfigurator` to wire up dependencies cleanly
- Constructor injection throughout the solution

---

## 🧪 Entity Framework Setup

### 🔧 Configuration Example

```csharp
services.AddDbContext<FurniDbContext>(options =>
{
    options.UseLazyLoadingProxies()
           .UseSqlServer(configuration.GetConnectionString("cs"),
               sql => sql.MigrationsAssembly("DataAccess"));
});
```

### 🗃 EF Migrations

#### Using PMC:

```powershell
Add-Migration InitialCreate -Project DataAccess -StartupProject Furni-Ecommerce-Website
Update-Database -Project DataAccess -StartupProject Furni-Ecommerce-Website
```

#### Using .NET CLI:

```bash
dotnet ef migrations add InitialCreate --project DataAccess --startup-project Furni-Ecommerce-Website
dotnet ef database update --project DataAccess --startup-project Furni-Ecommerce-Website
```

---

## 🌍 Hosting & Deployment

| App               | Example URL                |
|------------------|----------------------------|
| User Website      | `https://www.furni.com`    |
| Admin Dashboard   | `https://admin.furni.com`  |

- Hosted on **IIS**, **Azure**, or compatible .NET hosting
- One SQL Server instance
- Can be deployed independently
- Supports CI/CD and production-ready scaling

---

## 🧰 Tech Stack

| Layer              | Technology                              |
|--------------------|------------------------------------------|
| Frontend           | Razor Views, Bootstrap                   |
| Backend            | ASP.NET MVC (.NET Framework)             |
| ORM                | Entity Framework 6 (EF6)                 |
| Authentication     | ASP.NET Identity                         |
| Dependency Injection | Microsoft.Extensions.DependencyInjection |
| Database           | Microsoft SQL Server                     |

---

## ▶️ Getting Started

### ✅ Prerequisites

- Visual Studio 2019 or later
- .NET Framework 4.7.2 or newer
- SQL Server (Express or Full)
- NuGet CLI / .NET CLI

### ⚙️ Installation

1. **Clone the Repository**

   ```bash
   git clone https://github.com/yourusername/furni-ecommerce-website.git
   ```

2. **Configure Connection Strings**

   Update the following:
   - `Furni-Ecommerce-Website\web.config`
   - `Furni-Ecommerce-DashBoard\web.config`

3. **Run EF Migrations**

   Execute migration commands as outlined above.

4. **Run the Application**

   - Set either frontend or dashboard as the startup project
   - Press `F5` to build and launch

---

## 📷 Screenshots (Optional)

Include:
- Product catalog view
- Product detail page
- Cart and checkout
- Admin dashboard
- Order history
- Login / Register UI

---

## 🙌 Contributing

We welcome contributions!

### How to Contribute

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Push and create a Pull Request

Open issues for bugs, enhancements, or discussions.

---

## 📝 License

This project is licensed under the **MIT License**.  
See the [LICENSE](./LICENSE) file for details.

---

## 💬 Contact

- 📧 Email: [youremail@example.com](mailto:youremail@example.com)
- 🌐 Website: [yourwebsite.com](https://yourwebsite.com)

---
