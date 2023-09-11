Clear-Host

# Defina a variável de ambiente LANG para UTF-8
$env:LANG = "en_US.UTF-8"

# Obtém o diretório do script atual
$scriptDir = $PSScriptRoot

# Define o diretório para o diretório do script
Write-Host "Script Directory: $scriptDir"
Set-Location $scriptDir
# Set-Location "C:\repos\SchoolProject\SchoolProject.Web\"

# Obtém o diretório pai do diretório do script
$parentDir = Split-Path -Path $scriptDir -Parent

# Define o caminho de saída usando o diretório pai
$outputPath = Join-Path -Path $parentDir -ChildPath "SQL\migration_script_MsSql.sql"

# Agora $outputPath contém o caminho desejado
Write-Host "Caminho de Saída: $outputPath"

