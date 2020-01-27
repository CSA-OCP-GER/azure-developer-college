# Challenge 7: Networking: Loadbalancing your WWW Server Farm

[back](../../readme.md)

## Here is what you will learn ##

- How to load balance http traffic to 2 vms 
- By creating an external load balancer using the azure portal
- Learn to know the requirements for a loadbalancer and how to configure it.

architecture overview / picture
1. Deploy start environment
2. Deploy Azure Loadbalancer

## 1. Deploy the 'starting point' ##
In this directory there is an ARM-template which includes 2 web server vms and its requirements (networking, disks,...).  
_Image architecture_  
You should deploy the scenario using the 
<a href="https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2FCSA-OCP-GER%2Fazure-developer-college%2Ffeatures%2Fday1handson%2Fday1%2Fchallenges%2FChallenge%207%2FChallenge7Start.json"><img src="deploytoazure.png"/></a>
button into your subscription.  

| Name | Value |
|---|---|
| Resource group  |  **(new)** rg-lbwww |
| Location  |  **North Europe** |   
| Admin user  |  demouser |   
| Admin password  |  **_some complex value_** |   
| Vm Size  |  **Standard_B2s**  or try e.g. **Standard_F2s_v2**|   
| Disk Sku  |  StandardSSD_LRS |   

In the next step you will add an external azure loadbalancer in front of the 2 parallel web server machines.
![alt](https://link)
[text](https://link)