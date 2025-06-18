# .\create-migration.ps1 -MigrationName "Added_Messages" -ShouldUpdateDatabase $true
# .\create-migration.ps1 -MigrationName "Added_Messages" -DbContextName "MasterDbContext" -ShouldUpdateDatabase $true
param(
    [Parameter(Mandatory=$true)]
    [string]$MigrationName,

    [Parameter(Mandatory=$false)]
    [bool]$ShouldUpdateDatabase = $false,
    
    [Parameter(Mandatory=$false)]
    [string]$DbContextName = "ChurchManagerDbContext"
)

# Function to log errors
function Log-Error {
    param(
        [string]$ErrorMessage
    )
    $timestamp = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
    $logMessage = "$timestamp - ERROR: $ErrorMessage"
    Write-Host $logMessage -ForegroundColor Red
    Add-Content -Path "migration_error.log" -Value $logMessage
}

try {
    # Navigate to the Persistence project directory
    Set-Location -Path "src\Infrastructure\ChurchManager.Infrastructure.Persistence"
    
    # Determine the output directory based on DbContextName
    $outputDir = if ($DbContextName -eq "ChurchManagerDbContext") { "Migrations" } else { "MasterMigrations" }

    # Create the migration
    dotnet ef migrations add $MigrationName -c $DbContextName -o $outputDir -s ..\..\API\ChurchManager.Api\ChurchManager.Api.csproj
    if ($LASTEXITCODE -ne 0) {
        Set-Location -Path "..\..\..\"
        throw "Failed to create migration."
    }

    # Navigate back to the root directory
    Set-Location -Path "..\..\..\"

    # Update the database if ShouldUpdateDatabase is true
    if ($ShouldUpdateDatabase) {
        dotnet ef database update --project src\Infrastructure\ChurchManager.Infrastructure.Persistence\ChurchManager.Infrastructure.Persistence.csproj --startup-project src\API\ChurchManager.Api\ChurchManager.Api.csproj --context ChurchManager.Infrastructure.Persistence.Contexts.$DbContextName
        if ($LASTEXITCODE -ne 0) {
            throw "Failed to update database."
        }
        Write-Host "Migration '$MigrationName' has been created and applied to the database using $DbContextName." -ForegroundColor Green
    } else {
        Write-Host "Migration '$MigrationName' has been created using $DbContextName. Database update was skipped." -ForegroundColor Yellow
    }
}
catch {
    Log-Error $_.Exception.Message
    exit 1
}