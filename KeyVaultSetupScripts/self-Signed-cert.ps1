$todaydt = Get-Date;

$3years = $todaydt.AddYears(3);

$certificate = New-SelfSignedCertificate -DnsName "test.whitestrikeit.com" -CertStoreLocation Cert:\CurrentUser\My -FriendlyName "testvaultcert" -NotAfter $3years -KeySpec Signature;
