

Clear-Host

Set-Location "C:\Users\nunov\source\repos\SchoolProject\SchoolProject.Web\"
 
Remove-Item .\Migrations\ -Recurse -Force


dotnet ef migrations add InitDB        --context DataContextMsSql
# dotnet ef database   drop              --context DataContextMsSql --force
dotnet ef database   update            --context DataContextMsSql


dotnet ef migrations add InitDB        --context DataContextMySQL
dotnet ef database   drop              --context DataContextMySQL   --force
dotnet ef database   update            --context DataContextMySQL


dotnet ef migrations add InitDB        --context DataContextSqLite
dotnet ef database   drop              --context DataContextSqLite  --force
dotnet ef database   update            --context DataContextSqLite


dotnet ef dbcontext  list

dotnet ef dbcontext  info               --context DataContextMsSql

dotnet ef dbcontext  optimize           --context DataContextMsSql


