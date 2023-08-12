#!/bin/bash

entitiesDirectory='C:\Users\nunov\source\repos\SchoolProject\SchoolProject.Web\Data\Entities'
rootDirectory='C:\Users\nunov\source\repos\SchoolProject\SchoolProject.Web\'

# dataContext="DataContextMsSql"
dataContext="DataContextMySql"

cd "$rootDirectory"

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

    echo "dotnet aspnet-codegenerator controller --model $modelName --dataContext $dataContext --useDefaultLayout --force --relativeFolderPath Controllers --controllerName $controllerName --useAsyncActions"
    dotnet aspnet-codegenerator controller --model "$modelName" --dataContext $dataContext --useDefaultLayout --force --relativeFolderPath Controllers --controllerName "$controllerName" --useAsyncActions
  done

  # Loop recursivamente em todas as subpastas
  for dir in "$directory"/*; do
    if [ -d "$dir" ]; then
      GenerateControllersRecursively "$dir"
    fi
  done
}

# Chame a função para gerar scaffolding de controladores para todas as classes em subpastas
GenerateControllersRecursively "$entitiesDirectory"
