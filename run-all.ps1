# Pad naar je projecten (pas aan indien nodig)
$wasmPath = "C:\Users\Ed\source\repos\ActielijstGSApril\KlantBaseWASM"
$apiPath = "C:\Users\Ed\source\repos\ActielijstGSApril\ActielijstAPI"
$testPath = "C:\Users\Ed\source\repos\ActielijstGSApril\KlantBaseTests"

# Start KlantBaseWASM
Write-Host "Start KlantBaseWASM..."
Start-Process "dotnet" "run --project `"$wasmPath`"" -WindowStyle Hidden

# Start ActielijstAPI
Write-Host "Start ActielijstAPI..."
Start-Process "dotnet" "run --project `"$apiPath`"" -WindowStyle Hidden

# Wacht tot de apps zijn opgestart
Write-Host "Wachten tot apps zijn gestart..."
Start-Sleep -Seconds 10

# Voer de Playwright-tests uit
Write-Host "Start Playwright-tests..."
dotnet test "$testPath" 
Write-Host "Klaar!"
