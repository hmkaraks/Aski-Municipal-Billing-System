<div align="center">

<img width="200" alt="ASKI Logo" src="https://github.com/user-attachments/assets/b896a002-f8bf-4930-8674-3d077e619e43" style="margin-bottom: 20px;">

# ASKI Municipal Billing System
### *A water billing and subscriber management web app built while interning at ASKİ*

<img src="https://img.shields.io/badge/Status-In%20Development-orange?style=for-the-badge" alt="Status">
<img src="https://img.shields.io/badge/.NET-10.0-512BD4?style=for-the-badge&logo=.net&logoColor=white" alt=".NET">
<img src="https://img.shields.io/badge/PostgreSQL-4169E1?style=for-the-badge&logo=postgresql&logoColor=white" alt="PostgreSQL">
<img src="https://img.shields.io/badge/Bootstrap-7952B3?style=for-the-badge&logo=bootstrap&logoColor=white" alt="Bootstrap">
<img src="https://img.shields.io/badge/JavaScript-F7DF1E?style=for-the-badge&logo=javascript&logoColor=black" alt="JavaScript">

</div>

---

## 🏛️ Project Overview

A full-stack web application that digitizes municipal water billing. It has two sides: an **admin portal** for managing subscribers, meters and tariffs, and a **citizen portal** where subscribers can view their own invoices.

This is a learning project. It demonstrates a layered .NET backend, relational modeling with EF Core, and a lightweight JavaScript frontend. It is not production-ready: see the roadmap below for what is still missing.

---

## 🛠️ Technology Stack

| Layer | Technology | Purpose |
| :--- | :--- | :--- |
| **Backend** | C#, ASP.NET Core, LINQ | REST API and business logic |
| **ORM** | Entity Framework Core | Code-First mapping and migrations |
| **Database** | PostgreSQL | Relational data storage |
| **Frontend** | HTML5, Vanilla JS (Fetch API) | Single-page style UI without page reloads |
| **Styling** | Bootstrap 5, custom CSS | Responsive layout |

---

## 💡 Key Design Decisions

* **Billing logic lives on the server.** Tariff, tax and total calculations are done in C#. The client only sends raw meter readings, so invoice amounts cannot be manipulated from the browser.
* **Decoupled frontend and API.** The UI talks to the REST API with `fetch` and `async/await`.
* **Normalized relational schema.** Citizens, Meters and Invoices are linked with one-to-many relationships via EF Core.
* **Role-based views.** Admin and citizen screens are separated, and citizens only see their own billing history.

---

## 🚀 Modules

1. **Admin / Staff Portal**
   * CRUD operations for subscribers and meters
   * Tariff configuration
   * Automated invoice calculation
2. **Citizen Portal**
   * View past and current invoices
   * Consumption and account overview

---

## ⚙️ Getting Started

1. Install the .NET SDK and PostgreSQL.
2. Store your connection string with user-secrets (never commit it):
```bash
   dotnet user-secrets init
   dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Database=aski;Username=YOUR_USER;Password=YOUR_PASSWORD"
```
3. Apply migrations and run:
```bash
   dotnet ef database update
   dotnet run
```

---

## 🗺️ Roadmap / Known Limitations

- [ ] Authentication is currently simulated on the client with LocalStorage. Replace it with JWT-based authentication and server-side authorization.
- [ ] Add Swagger/OpenAPI documentation
- [ ] Add unit tests (xUnit) for the billing calculation
- [ ] Add Docker support
- [ ] Add input validation and centralized error handling

---

## 📂 Project Structure

```text
Aski-Municipal-Billing-System/
│
├── Controller/          # REST API endpoints
├── Data/                # Database context
├── Migrations/          # EF Core migrations
├── Models/              # Entities
├── wwwroot/             # Frontend (HTML, JS, CSS)
├── Program.cs           # Entry point and dependency injection
└── README.md
```
