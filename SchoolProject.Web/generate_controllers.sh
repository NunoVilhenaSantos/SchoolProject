#!/bin/bash

# Obtém o diretório do script atual
scriptDirectory="$(cd "$(dirname "$0")" && pwd)"

entitiesDirectory="$scriptDirectory/Data/Entities"
rootDirectory="$scriptDirectory"

cd "$rootDirectory" || exit


# dataContext="DataContextMsSql"
dataContext="DataContextMySql"


PluralizeControllerName() {
  local modelName="$1"

  if [[ "$modelName" == *y ]]; then
    local newModelName="${modelName%y}iesController"
    echo "$newModelName"
  elif [[ "$modelName" == *ss ]]; then
    echo "${modelName}esController"
  else
    echo "${modelName}sController"
  fi
}

GenerateControllersRecursively() {
  local directory="$1"

  # Loop por todos os arquivos .cs no diretório atual
  for model in "$directory"/*.cs; do
    modelName=$(basename "${model%.*}")
    controllerName=$(PluralizeControllerName "$modelName")

    # echo "dotnet aspnet-codegenerator controller --model $modelName --dataContext $dataContext --useDefaultLayout --force --relativeFolderPath Controllers --controllerName $controllerName --useAsyncActions"
    # dotnet aspnet-codegenerator controller --model "$modelName" --dataContext $dataContext --useDefaultLayout --force --relativeFolderPath Controllers --controllerName "$controllerName" --useAsyncActions

    echo "dotnet aspnet-codegenerator controller --model $modelName --dataContext $dataContext --useDefaultLayout --relativeFolderPath Controllers --controllerName $controllerName --useAsyncActions"
    dotnet aspnet-codegenerator controller --model "$modelName" --dataContext $dataContext --useDefaultLayout --relativeFolderPath Controllers --controllerName "$controllerName" --useAsyncActions

  done

  # Loop recursivamente em todas as sub-pastas
  for dir in "$directory"/*; do
    if [ -d "$dir" ]; then
      GenerateControllersRecursively "$dir"
    fi
  done
}


Clear

# instalar dot net entity framework e o code-generator para os controladores
dotnet tool install --global dotnet-ef
dotnet tool install --global dotnet-aspnet-codegenerator


# Chame a função para gerar scaffolding de controladores para todas as classes em sub-pastas
GenerateControllersRecursively "$entitiesDirectory"

