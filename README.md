# OpsControl

**Incident Management System built with .NET 10, React and SQL Server**

🌐 **Live Demo:** http://opscontrol.runasp.net

OpsControl is a full-stack web application for managing operational incidents throughout their lifecycle.

The project was built as a practical implementation of a modern .NET architecture, separating business logic, application use cases, infrastructure and presentation concerns.

<img width="1301" height="666" alt="resumenIncidencias" src="https://github.com/user-attachments/assets/f4b5dd8d-e517-4e0d-baed-d817ae73ac1b" />
<img width="1273" height="648" alt="tablaIncidenciasYDetalle" src="https://github.com/user-attachments/assets/4a1f763e-b159-453b-9247-e2654e69e38f" />
<img width="1208" height="648" alt="nuevaIncidencia" src="https://github.com/user-attachments/assets/39310e30-1084-4068-9e31-7fe441ef4916" />

---

## 🚀 Live Demo

👉 **http://opscontrol.runasp.net**

The application is deployed online with both the React frontend and ASP.NET Core API running under the same application.

---

## ✨ Features

- Create incidents
- List and paginate incidents
- Filter incidents by status
- View incident details
- Update incidents
- Delete incidents
- Start incident processing
- Resolve incidents
- Automatic priority calculation based on Impact and Urgency
- Incident status workflow:
  - New
  - In Progress
  - Resolved
- Incident summary/dashboard
- Persistent SQL Server storage
- REST API

---

## 🛠 Tech Stack

### Backend

- C#
- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- Dependency Injection
- Async / Await
- REST

### Frontend

- React
- TypeScript
- Vite
- CSS
- Fetch API

### Deployment

- IIS
- MonsterASP.NET
- WebDeploy
- SQL Server hosted database

---

## 🏗 Architecture

The backend follows a layered architecture:

```text
OpsControl.Api
      │
      ▼
OpsControl.Application
      │
      ▼
OpsControl.Domain

OpsControl.Infrastructure
      │
      ├── Entity Framework Core
      ├── SQL Server
      └── Repository implementations
```

### Domain

Contains the core business model and business rules.

For example, an `Incident` controls its own lifecycle and supports operations such as:

```text
New → InProgress → Resolved
```

### Application

Contains the application's use cases, such as:

- CreateIncident
- GetIncidents
- GetIncidentById
- UpdateIncident
- DeleteIncident
- StartIncident
- ResolveIncident
- GetIncidentSummary

### Infrastructure

Contains persistence-related implementations:

- Entity Framework Core
- AppDbContext
- SQL Server
- Repository implementations

### API

Exposes the application functionality through REST endpoints and handles HTTP requests/responses.

---

## 🔄 Request Flow

A typical request follows this flow:

```text
React
  │
  ▼
ASP.NET Core Controller
  │
  ▼
Application Use Case
  │
  ▼
Repository Interface
  │
  ▼
Infrastructure Repository
  │
  ▼
Entity Framework Core
  │
  ▼
SQL Server
```

This keeps HTTP, business logic and data-access responsibilities separated.

---

## 📊 Incident Priority

OpsControl calculates incident priority using two business values:

- **Impact**
- **Urgency**

This allows the domain model to determine the appropriate priority instead of leaving that responsibility to the UI or database.

---

## 🌐 Production Deployment

The production application is deployed as a single website.

```text
Browser
   │
   ▼
React Frontend
   │
   │ /api/incidents
   ▼
ASP.NET Core API
   │
   ▼
Entity Framework Core
   │
   ▼
SQL Server
```

The React application is built with Vite and its production files are served by ASP.NET Core from `wwwroot`.

This allows the frontend and API to share the same domain.

---

## 💻 Local Development

### Backend

Configure the local SQL Server connection in:

```text
appsettings.Development.json
```

Then run:

```bash
dotnet run
```

### Frontend

From the `opscontrol-web` directory:

```bash
npm install
npm run dev
```

The development environment uses:

```text
VITE_API_URL=https://localhost:7085/api/incidents
```

---

## 🎯 Why I Built OpsControl

I built OpsControl to apply modern .NET development practices to a complete application rather than a simple CRUD demo.

The project gave me the opportunity to work with:

- .NET 10
- ASP.NET Core
- React and TypeScript
- Entity Framework Core
- Dependency Injection
- Repository abstraction
- Layered architecture
- Domain business rules
- REST APIs
- SQL Server
- Production deployment

It also represents the evolution of my professional experience with C#, .NET Framework and SQL Server towards the current .NET ecosystem.

---

## 👤 Author

**Raúl Sánchez del Cueto**

.NET Developer

GitHub: https://github.com/rulsanchez
