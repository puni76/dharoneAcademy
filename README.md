# Dharone Academy — Production Web App

ASP.NET Core MVC (.NET 8) academy website + student portal with SQL Server.

**Brand:** navy `#002147` + gold `#C9A227`  
**GitHub:** https://github.com/puni76/dharoneAcademy  
**Firebase console:** https://console.firebase.google.com/project/dharoneacademy/

## Features

- Public marketing site (Home, Program, Curriculum, Career, About, Contact)
- Student registration & secure login (BCrypt + cookie auth)
- Student / Admin dashboard
- Daily Tasks tracker (attendance, topics, status, mentor feedback)
- Students CRUD (Admin)
- Contact enquiries stored in SQL Server
- Fully responsive production UI with academy logo & images

## Tech stack

- ASP.NET Core MVC (.NET 8)
- Entity Framework Core + SQL Server
- Cookie authentication + roles (`Admin`, `Student`)

## Quick start (local)

1. Install .NET 8 SDK and SQL Server Express **or** LocalDB.
2. Update connection string in `appsettings.json` / `appsettings.Development.json`.
3. Run:

```bash
cd DharoneAcademy   # or project root
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

## Firebase vs ASP.NET hosting (important)

[Firebase Hosting](https://console.firebase.google.com/project/dharoneacademy/) is excellent for **static** sites. It does **not** run ASP.NET Core + SQL Server.

### Recommended production setup

1. **ASP.NET app + SQL Server** → Azure App Service + Azure SQL (or Windows Server / IIS + SQL Server Express/Full)
2. **Firebase** → Analytics, optional static landing mirror (`firebase-hosting/`), Storage for media later

Deploy static Firebase placeholder:

```bash
npm i -g firebase-tools
firebase login
firebase use dharoneacademy
firebase deploy --only hosting
```

Then point the Firebase page CTA / redirect to your Azure App Service URL.

## Deploy ASP.NET (Azure sketch)

```bash
dotnet publish -c Release -o ./publish
# Deploy publish folder to Azure App Service / IIS
# Set ConnectionStrings__DefaultConnection in App Settings
```

## Project structure

```text
Controllers/   Home, Account, Dashboard, Students, DailyTasks, Contact
Models/        Users, StudentProfiles, DailyTasks, ContactEnquiries
Views/         Public site + portal UI
Data/          EF Core DbContext + seed
wwwroot/       CSS, JS, brand images
firebase-hosting/  Static Firebase entry
```

## Contact (from academy materials)

- Phone: +91 82174 15668  
- Email: dharoneacademy26@gmail.com  
- Web: www.dharoneacademy.com  
- Location: Bengaluru, Karnataka, India  

© Dharone Technologies Pvt Ltd
