$entitiesDirectory = "C:\Users\nunov\source\repos\SchoolProject\SchoolProject.Web\Data\Entities"
$rootDirectory = "C:\Users\nunov\source\repos\SchoolProject\SchoolProject.Web\"

$dataContext = "DataContextMsSql"

Set-Location $rootDirectory

function PluralizeControllerName {
    param (
        [string]$modelName
    )

    if ($modelName.EndsWith("y")) {
        $newModelName = $modelName.substring(0, $modelName.length - 1)+"iesController"
		return "$newModelName"
    }
    elseif ($modelName.EndsWith("ss")) {
        return "${modelName}esController"
    }
    else {
        return "${modelName}sController"
    }
}

function GenerateControllersRecursively {
    param (
        [string]$directory
    )

    # Loop por todos os arquivos .cs no diretório atual
    foreach ($model in Get-ChildItem -Path $directory -Filter *.cs) {
        $modelName = $model.BaseName
        $controllerName = PluralizeControllerName $modelName

        Write-Host "dotnet aspnet-codegenerator controller --model $modelName --dataContext $dataContext --useDefaultLayout --force --relativeFolderPath Controllers --controllerName $controllerName --useAsyncActions"
        dotnet aspnet-codegenerator controller --model $modelName --dataContext $dataContext --useDefaultLayout --force --relativeFolderPath Controllers --controllerName $controllerName --useAsyncActions
    }

    # Loop recursivamente em todas as subpastas
    foreach ($dir in Get-ChildItem -Path $directory -Directory) {
        GenerateControllersRecursively $dir.FullName
    }
}

Clear-Host

# Chame a função para gerar scaffolding de controladores para todas as classes em subpastas
GenerateControllersRecursively $entitiesDirectory
