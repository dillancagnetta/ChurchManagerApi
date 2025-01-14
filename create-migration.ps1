# .\create-migration.ps1 -MigrationName "Added_Messages"

param(
    [Parameter(Mandatory=$true)]
    [string]$MigrationName
)



# Navigate to the Persistence project directory
Set-Location -Path "src\Infrastructure\ChurchManager.Infrastructure.Persistence"

# Create the migration
dotnet ef migrations add $MigrationName -c ChurchManagerDbContext -o Migrations -s ..\..\API\ChurchManager.Api\ChurchManager.Api.csproj

# Navigate back to the root directory
Set-Location -Path "..\..\..\"

# Update the database
dotnet ef database update --project src\Infrastructure\ChurchManager.Infrastructure.Persistence\ChurchManager.Infrastructure.Persistence.csproj --startup-project src\API\ChurchManager.Api\ChurchManager.Api.csproj --context ChurchManager.Infrastructure.Persistence.Contexts.ChurchManagerDbContext

Write-Host "Migration '$MigrationName' has been created and applied to the database."