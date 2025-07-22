$pluginPath = "src\API\ChurchManager.Api\Plugins"

# Resolve the full path
$fullPath = Resolve-Path $pluginPath

# Check if it exists
if (Test-Path $fullPath) {
    Write-Host "Clearing contents of: $fullPath"
    
    # Delete all files
    Get-ChildItem -Path $fullPath -File -Recurse -Force | Remove-Item -Force

    # Delete all subdirectories
    Get-ChildItem -Path $fullPath -Directory -Recurse -Force | Remove-Item -Recurse -Force
    
     Write-Host "done" -ForegroundColor Green
} else {
    Write-Host "Directory not found: $pluginPath"
}
