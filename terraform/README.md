# Terraform support (experimental)

## Overview

This folder contains a terraform file to create an Azure Resource Group with all the necessary infrastructure to deploy the AlwaysEncryptedSample app to (in theory).

## Creating the resource group

```powershell
cd .\terraform\
az login
az account set --subscription='SUBSCRIPTION_ID_I_WANT_TO_USE'
terraform init
terraform plan
terraform apply
```

## Cleaning up

If you don't want to rack of charges then the command is `az group delete --name AlwaysEncryptedSample`
