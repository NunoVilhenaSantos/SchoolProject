Clear-Host

CHCP 65001

# Obtém o diretório do script atual
$scriptDir = $PSScriptRoot

# Define o diretório para o diretório do script
Write-Host "Script Directory: $scriptDir"
Set-Location $scriptDir


$entitiesDirectory = $scriptDir + "\Data\Entities"
# $rootDirectory = "C:\Users\nunov\source\repos\SchoolProject\SchoolProject.Web\"
# Set-Location $rootDirectory

# $dataContext = "DataContextMsSql"
$dataContext = "DataContextMySql"


function PluralizeControllerName
{
    param (
        [string]$modelName
    )

    if ( $modelName.EndsWith("y"))
    {
        $newModelName = $modelName.substring(0, $modelName.length - 1) + "iesController"
        return "$newModelName"
    }
    elseif ($modelName.EndsWith("ss"))
    {
        return "${modelName}esController"
    }
    else
    {
        return "${modelName}sController"
    }
}

function GenerateControllersRecursively
{
    param (
        [string]$directory
    )

    # Loop por todos os arquivos .cs no diretório atual
    foreach ($model in Get-ChildItem -Path $directory -Filter *.cs)
    {
        $modelName = $model.BaseName
        $controllerName = PluralizeControllerName $modelName

        # Write-Host "dotnet aspnet-codegenerator controller --model $modelName --dataContext $dataContext --useDefaultLayout --force --relativeFolderPath Controllers --controllerName $controllerName --useAsyncActions"
        # dotnet aspnet-codegenerator controller --model $modelName --dataContext $dataContext --useDefaultLayout --force --relativeFolderPath Controllers --controllerName $controllerName --useAsyncActions

        Write-Host "dotnet aspnet-codegenerator controller --model $modelName --dataContext $dataContext --useDefaultLayout --relativeFolderPath Controllers --controllerName $controllerName --useAsyncActions"
        dotnet aspnet-codegenerator controller --model $modelName --dataContext $dataContext --useDefaultLayout --relativeFolderPath Controllers --controllerName $controllerName --useAsyncActions
    }

    # Loop recursivamente em todas as sub-pastas
    foreach ($dir in Get-ChildItem -Path $directory -Directory)
    {
        GenerateControllersRecursively $dir.FullName
    }
}

Clear-Host

# instalar dot net entity framework e o code-generator para os controladores
dotnet tool install --global dotnet-ef
dotnet tool install --global dotnet-aspnet-codegenerator

# Chame a função para gerar scaffolding de controladores para todas as classes em sub-pastas
GenerateControllersRecursively $entitiesDirectory
