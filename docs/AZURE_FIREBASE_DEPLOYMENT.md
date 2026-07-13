# Azure + Firebase Production Deployment Guide

This guide deploys the **ASP.NET Core MVC app + SQL Server** to Azure, and wires **Firebase Analytics** (project [`dharoneacademy`](https://console.firebase.google.com/project/dharoneacademy/)).

---

## Architecture (recommended)

```text
Users
  │
  ├─► Azure App Service  →  DharoneAcademy ASP.NET (.NET 8)
  │         │
  │         └─► Azure SQL Database
  │
  └─► Firebase Analytics (browser SDK on every page)
         optional: Firebase Hosting landing → redirects to Azure URL
```

Firebase Hosting does **not** run ASP.NET. Use Azure (or IIS) for the real app.

---

## Part A — Firebase Analytics setup

### 1) Open Firebase project

Go to: https://console.firebase.google.com/project/dharoneacademy/

### 2) Enable Google Analytics

1. **Build** is not required for Analytics
2. Open **Analytics → Dashboard** (or Project settings → Integrations)
3. Enable Google Analytics for the project if prompted

### 3) Register a Web App

1. Project **Settings** (gear) → **Your apps**
2. Click **Web** (`</>`)
3. App nickname: `Dharone Academy Web`
4. Optionally check Firebase Hosting
5. Register app and copy the config object

Example (yours will differ):

```js
const firebaseConfig = {
  apiKey: "AIza...",
  authDomain: "dharoneacademy.firebaseapp.com",
  projectId: "dharoneacademy",
  storageBucket: "dharoneacademy.appspot.com",
  messagingSenderId: "123456789",
  appId: "1:123456789:web:abcdef",
  measurementId: "G-XXXXXXXX"
};
```

### 4) Paste values into the app

Edit `appsettings.json` (or better: Azure App Settings / User Secrets):

```json
"Firebase": {
  "ProjectId": "dharoneacademy",
  "AnalyticsEnabled": true,
  "ApiKey": "AIza...",
  "AuthDomain": "dharoneacademy.firebaseapp.com",
  "StorageBucket": "dharoneacademy.appspot.com",
  "MessagingSenderId": "123456789",
  "AppId": "1:123456789:web:abcdef",
  "MeasurementId": "G-XXXXXXXX"
}
```

Until real values replace `YOUR_...` placeholders, Analytics stays **off** automatically (safe for local/dev).

### 5) Verify

1. Run the site
2. Open browser DevTools → Network and confirm Firebase scripts load
3. In Firebase Console → **Analytics → DebugView** (optional with debug mode)
4. Within 24h, reports appear under Analytics → Dashboard

### 6) Optional: deploy static Firebase Hosting mirror

```bash
npm i -g firebase-tools
firebase login
firebase use dharoneacademy
firebase deploy --only hosting
```

Update `firebase-hosting/index.html` redirect URL to your Azure site when ready.

---

## Part B — Azure SQL Database

### 1) Create Azure SQL

1. Azure Portal → **Create a resource** → **SQL Database**
2. Create a new **SQL server** (example: `dharoneacademy-sql`)
3. Database name: `DharoneAcademy`
4. Pricing: Basic/S0 is fine to start
5. Networking: enable **Allow Azure services** + add your client IP for admin access

### 2) Get connection string

Azure SQL → **Connection strings** → ADO.NET:

```text
Server=tcp:YOUR_SERVER.database.windows.net,1433;Initial Catalog=DharoneAcademy;Persist Security Info=False;User ID=YOUR_USER;Password=YOUR_PASSWORD;MultipleActiveResultSets=True;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;
```

---

## Part C — Azure App Service (ASP.NET)

### 1) Create Web App

1. Azure Portal → **Create a resource** → **Web App**
2. Name: e.g. `dharoneacademy-portal`
3. Runtime: **.NET 8 (Isolated)** / **.NET 8**
4. OS: **Linux** (recommended) or Windows
5. Region: close to your users (Central India / South India)

### 2) Configure settings

App Service → **Configuration** → **Application settings**:

| Name | Value |
|------|--------|
| `ConnectionStrings__DefaultConnection` | Azure SQL connection string |
| `Firebase__AnalyticsEnabled` | `true` |
| `Firebase__ApiKey` | from Firebase |
| `Firebase__AuthDomain` | `dharoneacademy.firebaseapp.com` |
| `Firebase__ProjectId` | `dharoneacademy` |
| `Firebase__StorageBucket` | `dharoneacademy.appspot.com` |
| `Firebase__MessagingSenderId` | from Firebase |
| `Firebase__AppId` | from Firebase |
| `Firebase__MeasurementId` | from Firebase |
| `ASPNETCORE_ENVIRONMENT` | `Production` |

Save and restart the app.

### 3) Deploy from Visual Studio (easiest for beginners)

1. Right-click project → **Publish**
2. Target: **Azure** → **Azure App Service**
3. Sign in and select the web app
4. Publish

### 4) Deploy from CLI

Install Azure CLI, then:

```bash
az login
az webapp up --name dharoneacademy-portal --resource-group YOUR_RG --runtime "DOTNET|8.0" --sku B1
```

Or publish zip:

```bash
dotnet publish -c Release -o ./publish
cd publish
Compress-Archive -Path * -DestinationPath ../site.zip -Force
az webapp deployment source config-zip --resource-group YOUR_RG --name dharoneacademy-portal --src ../site.zip
```

### 5) First-run check

1. Open `https://YOURAPP.azurewebsites.net`
2. Confirm homepage loads
3. Login with seeded admin (change password after go-live):
   - `admin@dharoneacademy.com` / `Admin@123`
4. Confirm SQL tables exist (created automatically on startup)

---

## Part D — Custom domain (www.dharoneacademy.com)

1. App Service → **Custom domains** → Add `www.dharoneacademy.com`
2. Create DNS records as Azure shows (CNAME / TXT)
3. Turn on **App Service Managed Certificate** (free HTTPS)
4. Optionally redirect apex `dharoneacademy.com` → `www`

---

## Part E — GitHub Actions auto-deploy

Workflow file: `.github/workflows/azure-deploy.yml`

### One-time setup

1. Azure Portal → App Service → **Download publish profile**
2. GitHub repo → **Settings → Secrets and variables → Actions**
3. New secret name: `AZURE_WEBAPP_PUBLISH_PROFILE`
4. Paste publish profile XML
5. Optional secret values for Firebase can stay in Azure App Settings (preferred)

Push to `main` to deploy automatically.

---

## Security checklist before public launch

- [ ] Change default admin/student passwords
- [ ] Put SQL credentials only in Azure App Settings (not committed)
- [ ] Enable HTTPS only / HSTS (already on in Production)
- [ ] Restrict Azure SQL firewall
- [ ] Turn on App Service insights / logging
- [ ] Confirm Firebase Analytics measurement ID is production web app

---

## Cost starter tip

For a student academy MVP:

- App Service **B1**
- Azure SQL **Basic**
- Firebase Analytics **free tier**

Scale up when traffic grows.

---

## Need help?

If deploy fails, check:

1. App Service **Log stream**
2. Connection string correctness (`Encrypt=True` for Azure SQL)
3. Firebase config values (no `YOUR_` placeholders left)
