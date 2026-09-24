# Dharone Academy — Production Web App

ASP.NET Core MVC (.NET 8) academy website + student portal with SQL Server.

**Brand:** navy `#002147` + gold `#C9A227`  
**GitHub:** https://github.com/puni76/dharoneAcademy  
**Live portal (MonsterASP):** http://dharoneacademy.runasp.net/  
**Firebase console:** https://console.firebase.google.com/project/dharoneacademy/  
**Firebase Hosting (static marketing home):** https://dharoneacademy.web.app

> **Full project bible:** see **[PROJECT.md](PROJECT.md)**. Hosting / DNS notes: **[docs/HOSTING.md](docs/HOSTING.md)**.

## Features

- Public marketing site (Home, Program, Curriculum, Career, About, Contact)
- Student registration & secure login (BCrypt + cookie auth)
- Student / Admin dashboard
- Daily Tasks tracker (attendance, topics, status, mentor feedback)
- Students CRUD (Admin)
- Contact enquiries stored in SQL Server
- Fully responsive production UI with academy logo & images
- Firebase Analytics wired
- Azure deploy script ready (run after you get a subscription)

## Tech stack

- ASP.NET Core MVC (.NET 8)
- Entity Framework Core + SQL Server
- Cookie authentication + roles (`Admin`, `Student`)
- Firebase Analytics + Hosting landing

## Quick start (local)

1. Install .NET 8 SDK and SQL Server Express **or** LocalDB.
2. Run:

```bash
cd C:\Users\Dell\source\repos\dharoneAcademy
dotnet restore
dotnet run
```

### Demo logins

| Role | Email | Password |
|------|-------|----------|
| Admin | `admin@dharoneacademy.com` | `Admin@123` |
| Student | `student@dharoneacademy.com` | `Student@123` |

## SQL Server connection

Production default (`appsettings.json`):

```text
Server=.\SQLEXPRESS;Database=DharoneAcademy;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true
```

LocalDB for development (`appsettings.Development.json`):

```text
Server=(localdb)\mssqllocaldb;Database=DharoneAcademy;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true
```

On first run the app creates the database and seeds demo data.

## Hosting plan

| Piece | Now | Later |
|-------|-----|--------|
| Full ASP.NET + SQL portal | http://dharoneacademy.runasp.net/ (MonsterASP) | Azure App Service + Azure SQL (optional) |
| Analytics | Firebase (live) | same |
| Static marketing home | https://dharoneacademy.web.app (Join/Sign in → portal) | custom domain when DNS is registered |

See [docs/HOSTING.md](docs/HOSTING.md) for DNS (`dharoneacademy.com`) and Azure steps.

### Deploy to Azure when ready

```powershell
az login
cd C:\Users\Dell\source\repos\dharoneAcademy
.\scripts\deploy-azure.ps1
```

Details: [PROJECT.md §16](PROJECT.md#16-azure-deployment-do-later) and [docs/AZURE_FIREBASE_DEPLOYMENT.md](docs/AZURE_FIREBASE_DEPLOYMENT.md)

## Contact (from academy materials)

- Phone: +91 82174 15668  
- Email: dharoneacademy26@gmail.com  
- Web: http://dharoneacademy.runasp.net/ (custom domain pending DNS)  
- Location: Bengaluru, Karnataka, India  

© Dharone Technologies Pvt Ltd
