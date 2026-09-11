<img width="205" height="205" alt="aski-logo-project" src="https://github.com/user-attachments/assets/b896a002-f8bf-4930-8674-3d077e619e43" />
<div align="center">

<img width="180" alt="ASKI Logo" src="https://github.com/user-attachments/assets/7d5b0e07-ad7e-48cf-a7d4-1841d8e11566" style="margin-bottom: 20px;">

# ASKI Municipal Billing & Infrastructure System
### *Next-Generation Municipal Water Management & Billing Automation*

<img src="https://img.shields.io/badge/Status-Production%20Ready-success?style=for-the-badge&logo=appveyor" alt="Status">
<img src="https://img.shields.io/badge/.NET%20Core-8.0-512BD4?style=for-the-badge&logo=.net&logoColor=white" alt=".NET Core">
<img src="https://img.shields.io/badge/PostgreSQL-4169E1?style=for-the-badge&logo=postgresql&logoColor=white" alt="PostgreSQL">
<img src="https://img.shields.io/badge/Bootstrap-7952B3?style=for-the-badge&logo=bootstrap&logoColor=white" alt="Bootstrap">
<img src="https://img.shields.io/badge/JavaScript-F7DF1E?style=for-the-badge&logo=javascript&logoColor=black" alt="JavaScript">

</div>

---

## 🏛️ Project Overview

The **ASKI Municipal Billing and Infrastructure Management System** is an enterprise-grade, full-stack web application designed to digitize and secure municipal water billing operations. Developed with modern software architecture principles, the system bridges the gap between administrative oversight (Admin Portal) and citizen self-service (Citizen Portal).

---

## 🛠️ Technology Stack & Architecture

| Layer | Technology / Tool | Purpose |
| :--- | :--- | :--- |
| **Backend** | C# .NET Core, LINQ | RESTful API services & business logic execution |
| **ORM** | Entity Framework Core (EF Core) | Code-First database mapping & migrations |
| **Database** | PostgreSQL | Robust, scalable relational data storage |
| **Frontend** | HTML5, Vanilla JS (Fetch API) | Asynchronous, single-page application (SPA) experience |
| **Styling** | Bootstrap 5, Custom CSS3 | Responsive, modern Glassmorphism UI |

---

## 💡 Core Engineering Highlights

* **🛡️ Zero-Trust Business Logic:** Financial calculations (water consumption tariffs, taxes, and totals) are strictly processed on the backend (C#). The client-side (Frontend) only transfers raw readings to eliminate client-side tampering risks.
* **⚡ Thin Client & Asynchronous REST API:** Decoupled architecture using Vanilla JavaScript `fetch` and `async/await` patterns for zero-reload, lightning-fast data updates.
* **🔐 Role-Based Session Management:** Secure LocalStorage-based authentication simulation with automated data scoping, ensuring citizens can only query and view their personal billing history.
* **📊 Relational Integrity:** Complete database schema normalization linking Citizens, Meters, and Invoices (`One-to-Many` relationships via EF Core).

---

## 🚀 Key Modules

1. **Administrator / Staff Portal:**
   * Full CRUD operations for subscribers and meters.
   * Secure tariff configuration and automated billing calculation engine.
   * Role-secured entry points.
2. **Citizen Portal:**
   * Authenticated tracking of past and current water bills.
   * Real-time consumption monitoring and account overview.

---

## 📂 Project Structure

```text
Aski-Municipal-Billing-System/
│
├── Controllers/         # C# REST API Endpoints (Abone, Fatura, Yonetim)
├── Models/              # Entity Framework Database Entities & Context
├── wwwroot/             # Frontend Assets (HTML, JS, Bootstrap, CSS)
├── Program.cs           # Application Entry Point & Dependency Injection
└── README.md            # Project Documentation
