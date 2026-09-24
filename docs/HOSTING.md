# Dharone Academy — Hosting status

## What is live today

| URL | Role | Status |
|-----|------|--------|
| http://dharoneacademy.runasp.net/ | Full ASP.NET Core portal (MonsterASP) | **Working** (home, login, SQL features) |
| https://dharoneacademy.web.app | Firebase static **marketing home** | Shows academy home (no redirect splash) |
| https://puni76.github.io/dharoneAcademy/ | GitHub Pages mirror of marketing home | Same as Firebase `firebase-hosting/` |
| https://www.dharoneacademy.com | Brand custom domain | **Not registered / no DNS** — do not use until fixed |

HTTPS on `dharoneacademy.runasp.net` currently fails TLS on this free MonsterASP hostname; use **http://** until Let’s Encrypt is enabled in the MonsterASP control panel.

## Architecture

```text
Visitor
  ├─► Firebase Hosting / GitHub Pages  →  static marketing home
  │         └─► Join / Sign in / Program / Contact → MonsterASP portal
  └─► (optional later) www.dharoneacademy.com DNS  →  MonsterASP or Azure
```

Firebase Hosting and GitHub Pages **cannot** run ASP.NET + SQL. They host the static marketing home in `firebase-hosting/` (HTML + `css/` + `images/` + `js/`). Auth, contact forms, and the student portal stay on MonsterASP.

## Redeploy Firebase home

```powershell
cd C:\Users\Dell\source\repos\dharoneAcademy
npm i -g firebase-tools
firebase login
firebase use dharoneacademy
firebase deploy --only hosting
```

Static home source: `firebase-hosting/index.html`. Portal links point at `http://dharoneacademy.runasp.net/...`.

## MonsterASP CI

Workflow: `.github/workflows/monsterasp-deploy.yml` (runs on push to `main`).

Required GitHub Actions secrets: `WEBSITE_NAME`, `SERVER_COMPUTER_NAME`, `SERVER_USERNAME`, `SERVER_PASSWORD`, `CONNECTION_STRING`.

## Custom domain (`dharoneacademy.com`) — do later

Today the domain has **no public DNS** (NXDOMAIN). When you own it:

1. MonsterASP control panel → Domains → add `www.dharoneacademy.com` (Premium may be required).
2. At the registrar, set DNS as MonsterASP shows (typically apex **A** + `www` **CNAME** to your `siteXXXX.siteasp.net` / `*.runasp.net`).
3. Enable free HTTPS in MonsterASP.
4. Point the custom domain at the full portal (or keep Firebase for marketing and CNAME apex as preferred).
5. Update `appsettings.json` → `Academy:Website` and footer links in Views / static home.

## Azure — do later (credentials required)

Azure App Service `dharoneacademy-portal` is **not** provisioned. The workflow `.github/workflows/azure-deploy.yml` is **manual only** (`workflow_dispatch`) until you have:

1. Azure subscription
2. App Service + Azure SQL (or run `scripts/deploy-azure.ps1`)
3. GitHub secret `AZURE_WEBAPP_PUBLISH_PROFILE`

Then either run the workflow manually or re-enable `on: push` in that YAML.

Full steps: [AZURE_FIREBASE_DEPLOYMENT.md](AZURE_FIREBASE_DEPLOYMENT.md).

## Demo logins (change before public launch)

| Role | Email | Password |
|------|-------|----------|
| Admin | `admin@dharoneacademy.com` | `Admin@123` |
| Student | `student@dharoneacademy.com` | `Student@123` |
