# powershell -ExecutionPolicy Bypass -NoProfile -File .\Clone-Form.ps1 -SourceForm "BaseMultiples" -NewForms One Two Three etc
# powershell -ExecutionPolicy Bypass -NoProfile -File .\Clone-Form.ps1 -SourceForm "BasePandoraPCs" -NewForms SampleRequest
param(
    [string]$SourceForm = "SampleData",
    [string[]]$NewForms
)

if (-not $NewForms -or $NewForms.Count -eq 0) {
    Write-Error "Please provide at least one new form name with -NewForms"
    exit 1
}

foreach ($form in $NewForms) {
    Write-Host "Creating $form from $SourceForm..."

    $files = @(
        "$SourceForm.cs",
        "$SourceForm.Designer.cs",
        "$SourceForm.resx"
    )

    foreach ($file in $files) {
        if (-Not (Test-Path $file)) {
            Write-Error "File not found: $file"
            exit
        }

        $newFile = $file -replace $SourceForm, $form
        Copy-Item $file $newFile -Force

        (Get-Content $newFile) -replace $SourceForm, $form |
            Set-Content $newFile -Encoding UTF8

        Write-Host "  Created $newFile"
    }
}

