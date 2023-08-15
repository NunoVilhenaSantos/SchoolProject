Clear-Host

# Obtém o diretório do script atual
$scriptDir = $PSScriptRoot

# Define o diretório para o diretório do script
Write-Host "Script Directory: $scriptDir"
Set-Location $scriptDir

# Set-Location "C:\Users\nunov\source\repos\SchoolProject\SchoolProject.Web\"

Remove-Item .\Migrations\ -Recurse -Force

dotnet tool update dotnet-ef

dotnet ef migrations add InitDB     --context DataContextMySQL
dotnet ef database   drop           --context DataContextMySQL   --force
dotnet ef database   update         --context DataContextMySQL
dotnet ef dbcontext  info           --context DataContextMySQL
dotnet ef dbcontext  optimize       --context DataContextMySQL
Start-Sleep 3

dotnet ef migrations add InitDB     --context DataContextSqLite
dotnet ef database   drop           --context DataContextSqLite  --force
dotnet ef database   update         --context DataContextSqLite
dotnet ef dbcontext  info           --context DataContextSqLite
dotnet ef dbcontext  optimize       --context DataContextSqLite
Start-Sleep 3

dotnet ef migrations add InitDB     --context DataContextMsSql
# dotnet ef database   drop          --context DataContextMsSql --force
dotnet ef database   update         --context DataContextMsSql
dotnet ef dbcontext  info           --context DataContextMsSql
dotnet ef dbcontext  optimize       --context DataContextMsSql
Start-Sleep 3


dotnet ef dbcontext  list
Start-Sleep 3
