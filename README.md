# 🛒 Furni Ecommerce Website

**Furni** is a modular, scalable, and professional eCommerce platform built using **ASP.NET MVC (Framework)**.  
It provides a modern **customer-facing storefront** and a secure **admin dashboard** for furniture eCommerce, following **clean architecture principles** for maintainability and scalability.

---

## 📦 Solution Architecture

The solution is structured with clear separation between presentation, business logic, and data access:

```
/FurniEcommerceSolution
│
├── Furni-Ecommerce-Website       → MVC frontend for customers
├── Furni-Ecommerce-DashBoard     → MVC admin dashboard
├── BusinessLogic                 → Application layer (services, managers)
├── DataAccess                    → EF6 DbContext, repositories, migrations
├── Furni_Ecommerce_Shared        → Shared DTOs, enums, interfaces
```

### 🔗 Project References

```
[Furni-Ecommerce-Website]       → BusinessLogic, Furni_Ecommerce_Shared
[Furni-Ecommerce-DashBoard]     → BusinessLogic, Furni_Ecommerce_Shared
[BusinessLogic]                 → DataAccess, Furni_Ecommerce_Shared
[DataAccess]                    → Furni_Ecommerce_Shared
[Furni_Ecommerce_Shared]        → No dependencies
```

---

## 🚀 Features

### 👤 User Website

- Home, product listings, and detail views  
- Shopping cart & checkout flow  
- User registration and login  
- **Stripe Payments Integration**  
- **Email confirmation**  
- **Password reset via email**  
- **Order tracking system**  
- Responsive Razor views using Bootstrap  
- Role-based access control  

### 🛠 Admin Dashboard

- Secure login for admins  
- Admin dashboard with:
  - 🛋 Product Management
  - 🗂 Category Management
  - 📦 Order Management (with delivery status)
  - 👥 User & Role Management
  - 📊 **Sales & Order Analytics (Charts)**
- View customer order history and transaction status  

### 🧠 Business Logic Layer

- Centralized service classes  
- Clean abstraction of business rules  
- Interfaces for easy testing  
- Handles validation, Stripe, and email orchestration  

### 💾 Data Access Layer

- Entity Framework 6 (Code-First)  
- Repository & Unit of Work patterns  
- Lazy loading enabled  
- Centralized migrations  

### 🧩 Shared Layer

- DTOs, Enums, Interfaces  
- Role constants  
- Zero dependencies  

---

## 💳 Stripe Payment Integration

- Stripe Checkout for secure transactions  
- Webhook support for post-payment confirmation  
- Stores payment intent, transaction data  
- Handles success/failure redirects  
- Configured via `web.config`

---

## 📧 Account Email Features

- **Email confirmation** after registration  
- **Password reset** with secure token and email link  
- Uses ASP.NET Identity token providers  
- SMTP email service configured in `web.config`

---

## 🚚 Order Tracking

- Track order by status: `Pending`, `Processing`, `Shipped`, `Delivered`  
- Admin updates status in dashboard  
- Customers receive email updates  

---

## 📈 Admin Analytics Dashboard

- Line chart: Revenue over time  
- Bar chart: Orders by category  
- Pie chart: Delivery status  
- Top-selling products  
- Uses Chart.js (or similar) with AJAX for live data  

---

## 🔐 Authentication & Authorization

- ASP.NET Identity-based authentication  
- Role-based access (`User`, `Admin`)  
- Separate login flows and areas  
- `[Authorize]` attribute and route protection  

---

## ⚙️ Dependency Injection

- `Microsoft.Extensions.DependencyInjection`  
- Central `ServiceConfigurator` registers:
  - Services
  - Repositories
  - Email and Stripe handlers  
- Constructor injection throughout  

---

## 🧪 Entity Framework Setup

```csharp
services.AddDbContext<FurniDbContext>(options =>
{
    options.UseLazyLoadingProxies()
           .UseSqlServer(configuration.GetConnectionString("cs"),
               sql => sql.MigrationsAssembly("DataAccess"));
});
```

### Migrations

#### Package Manager Console

```powershell
Add-Migration InitialCreate -Project DataAccess -StartupProject Furni-Ecommerce-Website
Update-Database -Project DataAccess -StartupProject Furni-Ecommerce-Website
```

#### .NET CLI

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

- IIS, Azure, or any .NET-compatible hosting  
- Shared SQL Server database  
- Supports CI/CD pipelines  
- Independent deployment for frontend/admin  

---

## 🧰 Tech Stack

| Layer               | Technology                             |
|---------------------|-----------------------------------------|
| Frontend            | Razor Views, Bootstrap                  |
| Backend             | ASP.NET MVC (.NET Framework)            |
| ORM                 | Entity Framework 6 (EF6)                |
| Payments            | Stripe API                              |
| Email               | SMTP (ASP.NET Identity support)         |
| Authentication      | ASP.NET Identity                        |
| Dependency Injection| Microsoft.Extensions.DependencyInjection|
| Charting            | Chart.js or equivalent                  |
| Database            | Microsoft SQL Server                    |

---

## ▶️ Getting Started

### ✅ Prerequisites

- Visual Studio 2019+  
- .NET Framework 4.7.2+  
- SQL Server  
- NuGet CLI or .NET CLI  

### ⚙️ Installation Steps

1. **Clone the Repository**

   ```bash
   git clone https://github.com/yourusername/furni-ecommerce-website.git
   ```

2. **Configure Connection Strings**

   - `Furni-Ecommerce-Website\web.config`  
   - `Furni-Ecommerce-DashBoard\web.config`  

3. **Set up Stripe & SMTP Keys**

4. **Run EF Migrations**

5. **Build and Run**

   - Set the desired project as startup  
   - Press `F5` in Visual Studio  

---

## 📷 Screenshots _(Optional)_

- Product catalog  
- Product detail page  
- Cart and checkout  
- Login / Register  
- Admin dashboard  
- Sales chart  
- Order tracking  

---

## 🙌 Contributing

1. Fork the repository  
2. Create a feature branch  
3. Commit your changes  
4. Open a pull request  

Open issues for bugs, ideas, or suggestions.

---

## 📝 License

This project is licensed under the **MIT License**.  
See the [LICENSE](./LICENSE) file for details.

---

## 💬 Contact

- 📧 Email: [youremail@example.com](mailto:youremail@example.com)  
- 🌐 Website: [yourwebsite.com](https://yourwebsite.com)
