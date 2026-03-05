# Rename script: EasyStay -> Reservio
$oldName = "EasyStay"
$newName = "Reservio"
$rootPath = ".\$oldName"
$newRootPath = ".\$newName"

Write-Host "=== Starting rename from '$oldName' to '$newName' ===" -ForegroundColor Cyan

# 1. Rename root folder
if (Test-Path $rootPath) {
    Rename-Item -Path $rootPath -NewName $newName
    Write-Host "[OK] Root folder renamed: '$oldName' -> '$newName'" -ForegroundColor Green
} else {
    Write-Host "[ERROR] Root folder '$rootPath' not found. Make sure you are in the correct directory." -ForegroundColor Red
    exit 1
}

# 2. Rename project folders and .csproj files
$projectFolders = @(
    "$newName.Application",
    "$newName.Domain",
    "$newName.Infrastructure",
    "$newName.WebApi",
    "$newName.WebApi.Tests"
)

foreach ($folder in $projectFolders) {
    $oldFolderPath = Join-Path $newRootPath ($folder -replace $newName, $oldName)
    $newFolderPath = Join-Path $newRootPath $folder

    # Rename folder
    if (Test-Path $oldFolderPath) {
        Rename-Item -Path $oldFolderPath -NewName $folder
        Write-Host "  [FOLDER] '$($folder -replace $newName, $oldName)' -> '$folder'" -ForegroundColor Yellow
    } else {
        Write-Host "  [WARNING] Folder '$oldFolderPath' not found, skipping." -ForegroundColor DarkYellow
    }

    # Rename .csproj inside the new folder
    $oldCsproj = Join-Path $newFolderPath ($folder -replace $newName, $oldName) + ".csproj"
    $newCsproj = Join-Path $newFolderPath "$folder.csproj"
    if (Test-Path $oldCsproj) {
        Rename-Item -Path $oldCsproj -NewName "$folder.csproj"
        Write-Host "  [PROJECT] '$($folder -replace $newName, $oldName).csproj' -> '$folder.csproj'" -ForegroundColor Yellow
    } else {
        Write-Host "  [WARNING] Project file '$oldCsproj' not found, skipping." -ForegroundColor DarkYellow
    }
}

# 3. Update solution file
$slnPath = Join-Path $newRootPath "$newName.sln"
if (Test-Path $slnPath) {
    $content = Get-Content $slnPath -Raw
    $newContent = $content -replace $oldName, $newName
    Set-Content -Path $slnPath -Value $newContent
    Write-Host "[OK] Solution file updated: '$slnPath'" -ForegroundColor Green
} else {
    Write-Host "[ERROR] Solution file '$slnPath' not found." -ForegroundColor Red
}

# 4. Replace text in code files
$fileCount = 0
$extensions = @('*.cs', '*.json', '*.cshtml', '*.config', '*.xml', '*.txt', '*.md', '*.dockerignore', '*.gitignore', 'Dockerfile')
$files = Get-ChildItem -Path $newRootPath -Recurse -File | Where-Object {
    $extensions | ForEach-Object { if ($_.Name -like $_) { $true } else { $false } }
}

foreach ($file in $files) {
    $content = Get-Content $file.FullName -Raw
    if ($content -match $oldName) {
        $newContent = $content -replace $oldName, $newName
        Set-Content -Path $file.FullName -Value $newContent
        $fileCount++
        Write-Host "  [UPDATE] $($file.FullName)" -ForegroundColor Gray
    }
}

Write-Host "[OK] Text replacement completed. Modified $fileCount files." -ForegroundColor Green
Write-Host "[DONE] Renaming process finished! Please review changes and rebuild the solution." -ForegroundColor Cyan