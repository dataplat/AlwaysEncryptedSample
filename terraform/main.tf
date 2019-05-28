data "azurerm_client_config" "current" {

}

output "tenant_id" {
  value = "${data.azurerm_client_config.current.tenant_id}"
}

variable "resource_location" {
  type = "string"
  default = "East US"
}

variable "resource_names" {
    type = "map"
    default = {
      "ApplicationInsights"       = "AlwaysEncryptedSample"
      "AppServicePlan"            = "always-encrypted-sample-appserviceplan"
      "ResourceGroup"             = "AlwaysEncryptedSample"
      "SqlServer"                 = "alwaysencryptedsample"
      "SqlDatabase"               = "AlwaysEncryptedSample"
      "AppService"                = "AlwaysEncryptedSampleWeb3"
      "KeyVault"                  = "AlwaysEncryptedSampleKeyVault"
    }

}

variable "sql_settings" {
  type = "map"
  default = {
    "admin_login" = "essay"
    "admin_password" = "lbDG62XZy6i3pL8aC%Lw%uY7RYLN8o3aG2XhaH8dM2wbu0NPCMo0R"
  }
}

resource "azurerm_resource_group" "always_encrypted_sample" {
    name = "${var.resource_names["ResourceGroup"]}"
    location = "${var.resource_location}"
}



resource "azurerm_app_service_plan" "always_encrypted_sample" {
  name                = "${var.resource_names["AppServicePlan"]}"
  location            = "${azurerm_resource_group.always_encrypted_sample.location}"
  resource_group_name = "${azurerm_resource_group.always_encrypted_sample.name}"
  kind = "app"
  sku {
    tier = "Free"
    size = "F1"
  }
}

resource "azurerm_application_insights" "app_insights" {
  name                = "${var.resource_names["ApplicationInsights"]}"
  resource_group_name = "${azurerm_resource_group.always_encrypted_sample.name}"
  location            = "${azurerm_resource_group.always_encrypted_sample.location}"
  application_type    = "Web"
}

output "instrumentation_key" {
  value = "${azurerm_application_insights.app_insights.instrumentation_key}"
}


resource "azurerm_sql_server" "sql_server" {
    name                          = "${var.resource_names["SqlServer"]}"
    resource_group_name           = "${azurerm_resource_group.always_encrypted_sample.name}"
    location                      = "${azurerm_resource_group.always_encrypted_sample.location}"
    version                       = "12.0"
    administrator_login           = "${var.sql_settings["admin_login"]}"
    administrator_login_password  = "${var.sql_settings["admin_password"]}"
}

resource "azurerm_sql_database" "sql_database" {
  name                            = "${var.resource_names["SqlDatabase"]}"
  resource_group_name             = "${azurerm_resource_group.always_encrypted_sample.*.name[0]}"
  location                        = "${azurerm_resource_group.always_encrypted_sample.*.location[0]}"
  server_name                     = "${azurerm_sql_server.sql_server.*.name[0]}"
  edition                         = "Standard"
  create_mode                     = "Default"
  # requested_service_objective_name = "S1"
  # tags                             = "${local.tags}"
}


resource "azurerm_app_service" "web_3" {
  name                            = "${var.resource_names["AppService"]}"
  resource_group_name             = "${azurerm_resource_group.always_encrypted_sample.*.name[0]}"
  app_service_plan_id             = "${azurerm_app_service_plan.always_encrypted_sample.id}"
  location                        = "${azurerm_resource_group.always_encrypted_sample.*.location[0]}"
  https_only                      = "true"
  app_settings = {
    APPINSIGHTS_INSTRUMENTATIONKEY = "${azurerm_application_insights.app_insights.instrumentation_key}"
  }
  site_config {
    default_documents         = [
      "Default.htm",
      "Default.html",
      "Default.asp",
      "index.htm",
      "index.html",
      "iisstart.htm",
      "default.aspx",
      "index.php",
      "hostingstart.html",
    ]
    http2_enabled = false //TODO: figure out if enabling this helps anything
    ftps_state = "Disabled"
    use_32_bit_worker_process = true
  }

}


resource "azurerm_key_vault" "always_encrypted_sample" {
  name                            = "${azurerm_resource_group.always_encrypted_sample.*.name[0]}"
  resource_group_name             = "${var.resource_names["ResourceGroup"]}"
  location                        = "${azurerm_resource_group.always_encrypted_sample.*.location[0]}"
  tenant_id                       = "${data.azurerm_client_config.current.tenant_id}"
  sku {
    name = "standard"
  }
}

output "key_vault_uri" {
  value = "${azurerm_key_vault.always_encrypted_sample.vault_uri}"
}
