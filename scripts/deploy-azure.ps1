# Dharone Academy — one-shot Azure deploy (after az login)
# Usage (PowerShell):
#   .\scripts\deploy-azure.ps1

$ErrorActionPreference = "Stop"
$env:Path = [System.Environment]::GetEnvironmentVariable("Path","Machine") + ";" + [System.Environment]::GetEnvironmentVariable("Path","User")

$Root = Split-Path -Parent $PSScriptRoot
Set-Location $Root

$Subscription = az account show --query id -o tsv
if (-not $Subscription) { throw "Not logged in. Run: az login --use-device-code" }

$Location = "centralindia"
$Rg = "rg-dharoneacademy"
$Plan = "plan-dharoneacademy"
$App = "dharoneacademy-portal"
$SqlServer = "dharoneacademy-sql-$(Get-Random -Maximum 9999)"
# Keep names stable after first run by reading env override
if ($env:DHARONE_SQL_SERVER) { $SqlServer = $env:DHARONE_SQL_SERVER }

$SqlAdmin = "dharoneadmin"
$SqlPassword = "Dh@rone" + (Get-Random -Maximum 999999) + "Aa!"
$DbName = "DharoneAcademy"

Write-Host "Subscription: $Subscription"
Write-Host "Creating resource group $Rg in $Location..."

az group create --name $Rg --location $Location | Out-Null

Write-Host "Creating App Service plan $Plan (B1)..."
az appservice plan create --name $Plan --resource-group $Rg --sku B1 --is-linux | Out-Null

Write-Host "Creating Web App $App..."
az webapp create --resource-group $Rg --plan $Plan --name $App --runtime "DOTNETCORE:8.0" | Out-Null

Write-Host "Creating Azure SQL server $SqlServer (this can take a few minutes)..."
az sql server create `
  --name $SqlServer `
  --resource-group $Rg `
  --location $Location `
  --admin-user $SqlAdmin `
  --admin-password $SqlPassword | Out-Null

az sql server firewall-rule create `
  --resource-group $Rg `
  --server $SqlServer `
  --name AllowAzureServices `
  --start-ip-address 0.0.0.0 `
  --end-ip-address 0.0.0.0 | Out-Null

az sql db create `
  --resource-group $Rg `
  --server $SqlServer `
  --name $DbName `
  --service-objective Basic | Out-Null

$conn = "Server=tcp:$SqlServer.database.windows.net,1433;Initial Catalog=$DbName;Persist Security Info=False;User ID=$SqlAdmin;Password=$SqlPassword;MultipleActiveResultSets=True;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"

Write-Host "Configuring app settings..."
az webapp config connection-string set `
  --resource-group $Rg `
  --name $App `
  --connection-string-type SQLAzure `
  --settings DefaultConnection="$conn" | Out-Null

az webapp config appsettings set --resource-group $Rg --name $App --settings `
  ASPNETCORE_ENVIRONMENT=Production `
  Firebase__AnalyticsEnabled=true `
  Firebase__ProjectId=dharoneacademy `
  Firebase__ApiKey=AIzaSyCZ7KiQydppwCaSuLz5o8k3Rrx8B8a7ZsM `
  Firebase__AuthDomain=dharoneacademy.firebaseapp.com `
  Firebase__StorageBucket=dharoneacademy.firebasestorage.app `
  Firebase__MessagingSenderId=845517575019 `
  Firebase__AppId=1:845517575019:web:22707a0cb2bf0b9decf22e `
  Firebase__MeasurementId=G-LV6V344NZH | Out-Null

Write-Host "Publishing app..."
dotnet publish DharoneAcademy.csproj -c Release -o .\publish

Write-Host "Zipping and deploying..."
if (Test-Path .\site.zip) { Remove-Item .\site.zip -Force }
Compress-Archive -Path .\publish\* -DestinationPath .\site.zip -Force

az webapp deployment source config-zip `
  --resource-group $Rg `
  --name $App `
  --src .\site.zip | Out-Null

$url = "https://$App.azurewebsites.net"
Write-Host ""
Write-Host "DEPLOY COMPLETE"
Write-Host "App URL: $url"
Write-Host "SQL Server: $SqlServer.database.windows.net"
Write-Host "SQL Admin: $SqlAdmin"
Write-Host "SQL Password saved only to deploy-credentials.local.txt (gitignored)"
Write-Host ""

@"
AppUrl=$url
ResourceGroup=$Rg
WebApp=$App
SqlServer=$SqlServer.database.windows.net
SqlDatabase=$DbName
SqlUser=$SqlAdmin
SqlPassword=$SqlPassword
ConnectionString=$conn
CreatedUtc=$(Get-Date -AsUTC -Format o)
"@ | Set-Content -Path .\deploy-credentials.local.txt -Encoding UTF8

Write-Host "Credentials file: deploy-credentials.local.txt"
Write-Host "Demo login: admin@dharoneacademy.com / Admin@123"
