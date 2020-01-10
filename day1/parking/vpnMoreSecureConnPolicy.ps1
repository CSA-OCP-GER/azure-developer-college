<#Login to Azure#>
#Note: you may comment this out in case you already logged in with PowerShell
Login-AzAccount -Environment AzureCloud

#select the right subscription
$subscription = Get-AzSubscription | Out-GridView -Title "Welche Subscription soll verwendet werden?" -PassThru | Select-AzSubscription

$VPNGW = Get-AzResource -ResourceType 'Microsoft.Network/virtualNetworkGateways' | Out-GridView -Title "Select Your VPN GW" -PassThru

$ipsecMoreComplexPolicy = New-AzIpsecPolicy -IkeEncryption AES256 -IkeIntegrity SHA384 -DhGroup DHGroup14 -IpsecEncryption AES256 -IpsecIntegrity SHA256 -PfsGroup PFS2048 -SALifeTimeSeconds 27000 -SADataSizeKilobytes 102400000

Set-AzVirtualNetworkGatewayConnection -VirtualNetworkGatewayConnection (Get-AzVirtualNetworkGatewayConnection -ResourceGroupName $VPNGW.ResourceGroupName) -IpsecPolicies $ipsecMoreComplexPolicy
