# Receive an ID Token in a Fragment URL

## Here is what you'll learn

- How to register an Azure AD application
- How to create an Open ID Connect request to authenticate an user
- How to receive an ID token in a [Fragment URL](https://en.wikipedia.org/wiki/Fragment_identifier) for receiving information about the authenticated user

This is very similar to [challenge 0](challenge-0.md), except that this time we will receive the `id_token` through a fragment URL instead of it being in the body.

## Create an AAD application

Before you can authenticate an user you have to register an application in your AAD tenant.

### Azure CLI

```shell
az ad app create --display-name challengeidtokenfragment --reply-urls http://localhost:5001/api/tokenechofragment --identifier-uris https://challengeidtokenfragment
```

Retrieve and note the ID of your current AAD tenant via:

```shell
az account show 
```

## Run the Token Echo Server

Open another shell and run the Token Echo Server from [`day5/apps/token-echo-server`](../apps/token-echo-server) in this repository. This helper ASP.NET Core tool is used to echo the token issued by your Azure AD. The tool is listening on port 5001 on your local machine. Tokens are accepted on the route `http://localhost:5001/api/tokenechofragment`. This is why we initially registered an AAD application with a reply url pointing to `http://localhost:5001/api/tokenechofragment`.

```shell
dotnet run
```

## Create an authentication request

Replace `TENANT_ID` with your TenantId and `APPLICATION_ID` with your ApplicationId. Open a browser and paste the modified request.

```
https://login.microsoftonline.com/TENANT_ID/oauth2/v2.0/authorize?
client_id=APPLICATION_ID
&response_type=id_token
&redirect_uri=http%3A%2F%2Flocalhost%3A5001%2Fapi%2Ftokenechofragment
&response_mode=fragment
&scope=openid%20profile
&nonce=1234
```

Copy the `id_token` value from your browser's address bar, go to [https://jwt.ms](https://jwt.ms) and paste the token. Take a minute and have a look at the decoded token.

If you need further information about the issued claims take a look [here](https://docs.microsoft.com/en-us/azure/active-directory/develop/id-tokens#header-claims).

## Cleanup resources

### Azure CLI

```shell
az ad app delete --id <applicationid>
```

## Summary

This challenge showed how to create a new application in Azure AD and how an user can be authenticated using the Open ID Connect protocol. The full process is described [here](https://docs.microsoft.com/en-us/azure/active-directory/develop/v2-protocols-oidc).