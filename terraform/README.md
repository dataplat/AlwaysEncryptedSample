# Terraform support (experimental)

## Overview

This folder contains a terraform file to create an Azure Resource Group with all the necessary infrastructure to deploy the AlwaysEncryptedSample app to (in theory).

All thise commands assume you are in the `/terraform/` folder in the git repo. The best way to ensure you are there (assuming your terminals working directory is anywhere in the git repo) is `Join-Path -Path "$(git rev-parse --show-toplevel)" -childpath 'terraform' | Set-Location`

## Howto

### Creating the resource group

```powershell
az login
az account set --subscription='SUBSCRIPTION_ID_I_WANT_TO_USE'
# TODO: This line doesn't work and i need to fix
$env:TF_VAR_certificate_creator  = $(az account show --query id -otsv)
terraform init
terraform plan
terraform apply
```

### Cleaning up

If you don't want to rack of charges then the command is `az group delete --name AlwaysEncryptedSample`. You are also going to want to delete your terraform state (i.e. the `terraform.tfstate` file, hereafter referred to as tfstate) after deleting the reource grouns as your next `terraform apply` will fail otherwise. The tfstate associates the terraform objects in your `.tf` files with the guid identifiers of the azure resources. I haven't looked to hard into the details, but the script as is can't create everything from scratch if there is an existing tfstate. Therefore you probably want to do the following:

```powershell
az group delete --name AlwaysEncryptedSample
Remove-Item -Path .\terraform.tfstate
Remove-Item -Path .\terraform.tfstate.backup
```

## Further directions

* I'd like t0 store the state in Azure cloud storage and just be smarter about things.
