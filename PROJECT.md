# Dharone Academy — Complete Project Documentation

> **Status:** Local development ready · Firebase Analytics + Hosting live · Azure deploy waiting for your subscription  
> **Stack:** ASP.NET Core MVC (.NET 8) · SQL Server · Firebase · Cookie Auth  
> **Repo:** https://github.com/puni76/dharoneAcademy

This single document explains the **entire project** from idea → code → local run → Firebase → Azure (when you get a subscription).

---

Admin: admin@dharoneacademy.com / Admin@123
Student: student@dharoneacademy.com / Student@123

## Table of contents

1. [Project overview](#1-project-overview)
2. [Business & branding](#2-business--branding)
3. [What was built (features)](#3-what-was-built-features)
4. [Technology stack](#4-technology-stack)
5. [Prerequisites / install](#5-prerequisites--install)
6. [Repository & folder structure](#6-repository--folder-structure)
7. [Architecture](#7-architecture)
8. [Database design](#8-database-design)
9. [Authentication & roles](#9-authentication--roles)
10. [Pages & routes map](#10-pages--routes-map)
11. [CRUD operations explained](#11-crud-operations-explained)
12. [Configuration reference](#12-configuration-reference)
13. [How to run locally](#13-how-to-run-locally)
14. [Demo accounts](#14-demo-accounts)
15. [Firebase (done)](#15-firebase-done)
16. [Azure deployment (do later)](#16-azure-deployment-do-later)
17. [GitHub Actions](#17-github-actions)
18. [How to use the portal (end users)](#18-how-to-use-the-portal-end-users)
19. [Troubleshooting](#19-troubleshooting)
20. [Security checklist](#20-security-checklist)
21. [Roadmap / next improvements](#21-roadmap--next-improvements)
22. [Useful links](#22-useful-links)

---

## 1. Project overview

**Dharone Academy** is a production-style website and student portal for a **3-month internship & training program**.

It includes:

- A **public marketing website** (program, curriculum, career support, contact)
- A **secure student/admin portal**
- A **daily task tracker** (attendance, topics, status, mentor feedback)
- **SQL Server** persistence with EF Core
- **Firebase Analytics** tracking
- A **Firebase Hosting** static landing page
- Scripts/docs ready for **Azure App Service + Azure SQL** when you activate a subscription

### Current hosting reality

| Layer | Status | Where |
|-------|--------|--------|
| ASP.NET + SQL portal | Run locally now | Your PC (`dotnet run`) |
| Firebase Analytics | Live / configured | Project `dharoneacademy` |
| Firebase Hosting landing | Live | https://dharoneacademy.web.app |
| Azure App Service | Pending subscription | Deploy later with script |

> Firebase Hosting cannot run ASP.NET Core + SQL Server. Use Azure (or IIS/Windows Server) for the full portal.

---

## 2. Business & branding

### Academy identity

| Item | Value |
|------|--------|
| Name | Dharone Academy |
| Company | Dharone Technologies Pvt Ltd |
| Tagline | Learn • Build • Innovate • Succeed |
| Slogan | Bridging the gap: academic knowledge to industry demands |
| Location | Bengaluru, Karnataka, India |
| Phone | +91 82174 15668 |
| Email | dharoneacademy26@gmail.com |
| Website | www.dharoneacademy.com |

### Program summary

- **Audience:** BCA · MCA · BE · Diploma students  
- **Duration:** 3 months  
- **Fee:** ₹30,000  
- **Timing:** Daily sessions from 6:00 PM  
- **Certificate:** Official internship certificate through Dharone Technologies Pvt Ltd  
- **Curriculum tech:** HTML5, CSS3, JS, React, .NET / ASP.NET, SQL  

### Visual brand

| Token | Value |
|-------|--------|
| Navy | `#002147` |
| Navy deep | `#00152d` |
| Gold | `#C9A227` |
| Fonts | Playfair Display (headings) + Manrope (UI) |
| Logo | `wwwroot/images/logo.png` |
| Flyer | `wwwroot/images/flyer.png` |

---

## 3. What was built (features)

### Public website

- Home (hero, why academy, tech stack, gallery, CTA)
- Program details
- Curriculum
- Career building checklist
- About / mentors & contact info
- Contact enquiry form (saved to SQL)

### Auth

- Register (creates User + Student profile)
- Login / Logout
- Password hashing with **BCrypt**
- Cookie authentication

### Portal

- Dashboard (stats + recent daily tasks)
- Daily Tasks CRUD (students + admin)
- Students management CRUD (**Admin only**)
- Enquiries list / resolve (**Admin only**)

### Infrastructure helpers

- Auto DB create + seed data on startup
- Firebase Analytics partial on all pages
- Azure one-shot deploy script
- GitHub Actions Azure workflow template
- Deployment docs

---

## 4. Technology stack

| Layer | Technology |
|-------|------------|
| Framework | ASP.NET Core MVC (.NET 8) |
| Language | C# |
| UI | Razor Views + custom CSS (responsive) |
| ORM | Entity Framework Core 8 |
| Database | SQL Server Express / Full / LocalDB / Azure SQL |
| Auth | Cookie Auth + Roles |
| Password security | BCrypt.Net-Next |
| Analytics | Firebase Analytics (Measurement ID `G-LV6V344NZH`) |
| Static mirror | Firebase Hosting |
| Source control | GitHub (`puni76/dharoneAcademy`) |

### NuGet packages

- `Microsoft.EntityFrameworkCore.SqlServer` (8.0.11)
- `Microsoft.EntityFrameworkCore.Tools` (8.0.11)
- `Microsoft.EntityFrameworkCore.Design` (8.0.11)
- `BCrypt.Net-Next` (4.2.0)

---

## 5. Prerequisites / install

Install before running locally:

1. **Visual Studio 2022** (workload: ASP.NET and web development) **or** VS Code + .NET SDK  
2. **.NET 8 SDK** — https://dotnet.microsoft.com/download/dotnet/8.0  
3. **SQL Server Express** or **LocalDB** (LocalDB comes with VS)  
4. (Optional) **SSMS** to view tables  
5. (Optional later) **Azure CLI** — already usable for deploy script  
6. (Optional) **Firebase CLI** — for hosting redeploys  

### Verify

```powershell
dotnet --version
sqllocaldb info
```

---

## 6. Repository & folder structure

```text
dharoneAcademy/
├── Controllers/
│   ├── HomeController.cs          # Public pages
│   ├── AccountController.cs       # Login / Register / Logout
│   ├── DashboardController.cs     # Portal home
│   ├── StudentsController.cs      # Admin student CRUD
│   ├── DailyTasksController.cs    # Daily tracker CRUD
│   └── ContactController.cs       # Public contact + admin enquiries
├── Models/
│   ├── AppUser.cs
│   ├── StudentProfile.cs
│   ├── DailyTask.cs
│   ├── ContactEnquiry.cs
│   └── ViewModels/
├── Data/
│   ├── AppDbContext.cs            # EF Core mapping
│   └── DbInitializer.cs           # EnsureCreated + seed
├── Options/
│   └── FirebaseOptions.cs
├── Views/
│   ├── Home/                      # Marketing pages
│   ├── Account/                   # Auth UI
│   ├── Dashboard/
│   ├── Students/
│   ├── DailyTasks/
│   ├── Contact/
│   └── Shared/
│       ├── _Layout.cshtml
│       └── _FirebaseAnalytics.cshtml
├── wwwroot/
│   ├── css/site.css
│   ├── js/site.js
│   └── images/                    # Logo, flyer, gallery, hero
├── firebase-hosting/              # Static landing for Firebase
├── scripts/
│   └── deploy-azure.ps1           # Run after Azure subscription
├── docs/
│   └── AZURE_FIREBASE_DEPLOYMENT.md
├── .github/workflows/
│   └── azure-deploy.yml
├── Program.cs
├── appsettings.json               # SQL Express + Firebase config
├── appsettings.Development.json   # LocalDB
├── firebase.json
├── .firebaserc
├── README.md
└── PROJECT.md                     # This file
```

---

## 7. Architecture

```text
Browser
  │
  ├── Public pages (Home/Program/...) ──► Controllers ──► Views
  │
  ├── Contact form ─────────────────────► SQL: ContactEnquiries
  │
  ├── Login / Register ─────────────────► Users (+ StudentProfiles)
  │         cookies
  ▼
Portal (Authorize)
  ├── Dashboard stats
  ├── DailyTasks CRUD ──────────────────► SQL: DailyTasks
  └── Admin: Students + Enquiries

Cross-cutting:
  └── Firebase Analytics JS on every page layout
```

### Request flow (MVC)

1. Browser hits a URL  
2. Routing selects Controller + Action  
3. Action uses `AppDbContext` (EF Core)  
4. Data returned to a Razor View  
5. HTML/CSS/JS sent to browser  

---

## 8. Database design

Database name: **`DharoneAcademy`**

### Tables

#### `Users` (`AppUser`)

| Column | Notes |
|--------|--------|
| Id | PK |
| FullName | required |
| Email | unique |
| Phone | required |
| PasswordHash | BCrypt hash (never plain text) |
| Role | `Admin` or `Student` |
| IsActive | bool |
| CreatedAt | UTC |

#### `StudentProfiles`

| Column | Notes |
|--------|--------|
| Id | PK |
| UserId | FK → Users (1:1) |
| EnrollmentId | unique, e.g. `DA-2026-001` |
| CourseStream | BCA/MCA/BE/Diploma |
| CollegeName | optional |
| BatchName | e.g. `DA-2026-A` |
| JoinDate | date |
| IsActive | bool |

#### `DailyTasks`

| Column | Notes |
|--------|--------|
| Id | PK |
| StudentProfileId | FK → StudentProfiles |
| TaskDate | date |
| Attendance | Present / Absent / Late |
| TopicCovered | string |
| TaskTitle | string |
| Status | Pending / In Progress / Completed |
| HoursSpent | decimal |
| MentorFeedback | admin notes |
| StudentNotes | student notes |
| CreatedAt | UTC |

#### `ContactEnquiries`

| Column | Notes |
|--------|--------|
| Id | PK |
| FullName, Email, Phone | contact info |
| CourseInterest | optional |
| Message | required |
| IsResolved | admin flag |
| CreatedAt | UTC |

### Creation method

On app startup, `DbInitializer`:

1. `EnsureCreatedAsync()` (creates DB/tables if missing)
2. Seeds admin, sample student, sample tasks, sample enquiry (only if empty)

---

## 9. Authentication & roles

### Flow

1. User submits email/password  
2. App loads user from SQL  
3. `BCrypt.Verify(...)` checks password  
4. Claims created: Name, Email, Role, UserId  
5. Auth cookie issued (`DharoneAcademy.Auth`)  
6. `[Authorize]` pages require login  
7. `[Authorize(Roles = "Admin")]` restricts admin features  

### Roles

| Role | Can do |
|------|--------|
| Student | Dashboard, own daily tasks, public site |
| Admin | Everything student can + manage students + resolve enquiries + mentor feedback |

---

## 10. Pages & routes map

### Public

| URL | Description |
|-----|-------------|
| `/` | Home |
| `/Home/Program` | Internship program |
| `/Home/Curriculum` | Curriculum |
| `/Home/Career` | Career building |
| `/Home/About` | About academy |
| `/Contact` | Enquiry form |
| `/Account/Login` | Sign in |
| `/Account/Register` | Join / create account |

### Portal (login required)

| URL | Role | Description |
|-----|------|-------------|
| `/Dashboard` | Any | Stats + recent tasks |
| `/DailyTasks` | Any | List/filter tasks |
| `/DailyTasks/Create` | Any | Add task |
| `/DailyTasks/Edit/{id}` | Owner/Admin | Edit task |
| `/DailyTasks/Delete/{id}` | Owner/Admin | Delete task |
| `/Students` | Admin | Student list |
| `/Students/Details/{id}` | Admin | Student detail |
| `/Students/Edit/{id}` | Admin | Edit student |
| `/Students/Delete/{id}` | Admin | Delete student |
| `/Contact/Enquiries` | Admin | Lead enquiries |

---

## 11. CRUD operations explained

CRUD = **Create, Read, Update, Delete**

### Students (Admin)

| Action | Method | Path |
|--------|--------|------|
| Read list | GET | `/Students` |
| Read one | GET | `/Students/Details/{id}` |
| Update | GET/POST | `/Students/Edit/{id}` |
| Delete | GET/POST | `/Students/Delete/{id}` |

Students are mainly created through **Register**.

### Daily Tasks

| Action | Method | Path |
|--------|--------|------|
| Read | GET | `/DailyTasks` |
| Create | GET/POST | `/DailyTasks/Create` |
| Update | GET/POST | `/DailyTasks/Edit/{id}` |
| Delete | GET/POST | `/DailyTasks/Delete/{id}` |

Students only see/edit their own tasks. Admin can see all.

### Contact enquiries

- Public **Create** via `/Contact`
- Admin **Read/Resolve** via `/Contact/Enquiries`

---

## 12. Configuration reference

### `appsettings.json` (production-oriented)

- SQL Express connection: `Server=.\SQLEXPRESS;Database=DharoneAcademy;...`
- Firebase Analytics config (already filled)

### `appsettings.Development.json`

- LocalDB connection for easy local runs:

```text
Server=(localdb)\mssqllocaldb;Database=DharoneAcademy;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true
```

### Firebase keys (wired)

| Setting | Value |
|---------|--------|
| ProjectId | `dharoneacademy` |
| AppId | `1:845517575019:web:22707a0cb2bf0b9decf22e` |
| MeasurementId | `G-LV6V344NZH` |
| AnalyticsEnabled | `true` |

Analytics is injected by `Views/Shared/_FirebaseAnalytics.cshtml`.

---

## 13. How to run locally

```powershell
cd C:\Users\Dell\source\repos\dharoneAcademy

# optional: start LocalDB
sqllocaldb start mssqllocaldb

dotnet restore
dotnet run
```

Open the URL shown in terminal (from `Properties/launchSettings.json`).

### Stop

`Ctrl + C` in the terminal.

---

## 14. Demo accounts

Seeded automatically on first successful DB init:

| Role | Email | Password |
|------|-------|----------|
| Admin | `admin@dharoneacademy.com` | `Admin@123` |
| Student | `student@dharoneacademy.com` | `Student@123` |

> Change these passwords before production.

---

## 15. Firebase (done)

### Project

- Console: https://console.firebase.google.com/project/dharoneacademy/
- Hosting URL: https://dharoneacademy.web.app
- Web app name: **Dharone Academy Web**

### What works today

- Analytics SDK loads in ASP.NET layout
- Static hosting landing page deployed
- `.firebaserc` points to `dharoneacademy`

### Redeploy static hosting only

```powershell
cd C:\Users\Dell\source\repos\dharoneAcademy
firebase deploy --only hosting
```

After Azure is live, update redirect URL in `firebase-hosting/index.html` to your App Service URL.

---

## 16. Azure deployment (do later)

You said you will activate an Azure subscription later. When ready:

### Option A — one-shot script (recommended)

```powershell
az login
cd C:\Users\Dell\source\repos\dharoneAcademy
.\scripts\deploy-azure.ps1
```

The script creates:

- Resource group `rg-dharoneacademy`
- Linux App Service Plan (B1)
- Web App `dharoneacademy-portal`
- Azure SQL server + database `DharoneAcademy`
- Connection string + Firebase app settings
- Zip deploy of published app

Credentials are written to **`deploy-credentials.local.txt`** (gitignored).

Public URL format:

```text
https://dharoneacademy-portal.azurewebsites.net
```

### Option B — Visual Studio Publish

1. Right-click project → Publish → Azure App Service  
2. Create Web App + Azure SQL  
3. Set connection string  
4. Publish  

### Option C — detailed manual guide

See: [`docs/AZURE_FIREBASE_DEPLOYMENT.md`](docs/AZURE_FIREBASE_DEPLOYMENT.md)

### After Azure is live checklist

- [ ] Open App Service URL and login  
- [ ] Change demo passwords  
- [ ] Add custom domain `www.dharoneacademy.com`  
- [ ] Enable free managed HTTPS certificate  
- [ ] Update Firebase hosting redirect to Azure URL  
- [ ] Confirm Analytics events in Firebase console  

---

## 17. GitHub Actions

File: `.github/workflows/azure-deploy.yml`

When Azure exists:

1. Download App Service publish profile  
2. GitHub → Settings → Secrets → Actions  
3. Add secret `AZURE_WEBAPP_PUBLISH_PROFILE`  
4. Push to `main` to auto-deploy  

Until subscription exists, this workflow will fail if run — that is expected.

---

## 18. How to use the portal (end users)

### Student journey

1. Open site → **Join Now**  
2. Register with name/email/phone/stream  
3. Go to Dashboard  
4. Add Daily Tasks after each session  
5. Track attendance, topics, hours, status  

### Admin journey

1. Login as admin  
2. Review Dashboard stats  
3. Manage Students  
4. Review Daily Tasks / add mentor feedback  
5. Resolve Contact Enquiries  

---

## 19. Troubleshooting

### Cannot connect to SQL / LocalDB

```powershell
sqllocaldb start mssqllocaldb
```

Or switch connection string to SQL Express:

```text
Server=.\SQLEXPRESS;Database=DharoneAcademy;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true
```

### Build file locked (`demo1.exe` / app exe in use)

Stop running instance:

```powershell
Get-Process DharoneAcademy -ErrorAction SilentlyContinue | Stop-Process -Force
```

### Login redirects back to login

Ensure middleware order in `Program.cs`:

```csharp
app.UseAuthentication();
app.UseAuthorization();
```

### Analytics not showing

- Wait for Firebase reporting delay (can be hours for standard reports)
- Confirm `Firebase:AnalyticsEnabled` is true
- Confirm no `YOUR_` placeholders remain (already configured)
- Browse a few pages with the app running

### Azure deploy later fails on name taken

Change web app name / SQL server name in `scripts/deploy-azure.ps1` (SQL names must be globally unique).

---

## 20. Security checklist

Before public launch:

- [ ] Change seeded admin/student passwords  
- [ ] Keep SQL passwords only in Azure App Settings / local credential file (not git)  
- [ ] Use HTTPS in production (Azure managed cert)  
- [ ] Restrict Azure SQL firewall  
- [ ] Review contact form spam protections later (rate limit/captcha)  
- [ ] Backup Azure SQL regularly  

Note: Firebase web API keys are expected in client-side apps; protect data with Firebase rules/backends, not by hiding the web key alone.

---

## 21. Roadmap / next improvements

Nice future upgrades:

1. EF Core Migrations (instead of EnsureCreated)
2. Email notifications for new enquiries
3. File upload for assignments
4. Attendance charts / weekly report download
5. Payment / enrollment fee tracking
6. Role management UI
7. CI tests (unit + integration)
8. Custom domain + CI/CD fully automated

---

## 22. Useful links

| Resource | URL |
|----------|-----|
| GitHub repo | https://github.com/puni76/dharoneAcademy |
| Firebase console | https://console.firebase.google.com/project/dharoneacademy/ |
| Firebase Hosting | https://dharoneacademy.web.app |
| Azure deploy detail doc | [`docs/AZURE_FIREBASE_DEPLOYMENT.md`](docs/AZURE_FIREBASE_DEPLOYMENT.md) |
| Azure deploy script | [`scripts/deploy-azure.ps1`](scripts/deploy-azure.ps1) |
| Academy site (brand) | https://www.dharoneacademy.com |

---

## Quick start card

```powershell
# 1) Run locally today
cd C:\Users\Dell\source\repos\dharoneAcademy
dotnet run

# 2) Login
# admin@dharoneacademy.com / Admin@123

# 3) When Azure subscription is active
az login
.\scripts\deploy-azure.ps1
```

---

## Document history

| Date | Note |
|------|------|
| Jul 2026 | Initial complete project documentation created |
| Jul 2026 | Firebase Analytics + Hosting configured |
| Jul 2026 | Azure deploy deferred until subscription is available |

---

**Maintained for:** Dharone Academy / Dharone Technologies Pvt Ltd  
**Primary codebase path:** `C:\Users\Dell\source\repos\dharoneAcademy`
