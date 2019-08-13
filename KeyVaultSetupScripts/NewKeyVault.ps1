Login-AzureRmAccount

$certificate = New-Object System.Security.Cryptography.X509Certificates.X509Certificate2

$certificate.Import('c:\temp\testcert.cer')

$startDate = $certificate.GetEffectiveDateString()
$endDate = $certificate.GetExpirationDateString()
$credValue = [System.Convert]::ToBase64String($certificate.GetRawCertData())

Write-Host $startDate

Write-Host $endDate

$azureADApplication = New-AzureRmADApplication -DisplayName "whitestriketestapp" -HomePage "https://wstest" -IdentifierUris "https://wstest" -CertValue $credValue -EndDate $endDate

$principal= New-AzureRmADServicePrincipal -ApplicationId $azureADApplication.ApplicationId

Write-Host $azureADApplication.ApplicationId

Write-Host $principal.ApplicationId

Write-Host $principal.ServicePrincipalNames

Write-Host $principal.DisplayName

Set-AzureRmKeyVaultAccessPolicy -VaultName 'wstestvault' -ServicePrincipalName $principal.ServicePrincipalNames[0] -PermissionsToSecrets get, list -ResourceGroupName 'TestVault'

