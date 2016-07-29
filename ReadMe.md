Always Encrypted Sample Application
===================================

This is a smple ASP.NET application that demonstrates Always Encrypted 
features based on the Visual Studio 2013 Default ASP.NET MVC project.
It is ONLY meant to demonstrate Always Encrypted. It is **NOT** meant
to be a general purposes best practice web application.

Running
-------

This application should compile in Visual Studio 2015 out of the box. To 
compile it with Visual Studio 2013. the [Microsoft .NET Framework 4.6 Targeting
Pack](https://www.microsoft.com/en-us/download/details.aspx?id=48136) is
required. ADO.NET support for Always Encrypted is only present in .NET 4.6

If you edit the one connection string in the web.config to point to a SQL 
Server 2016 instance that your user has sa access to, and press F5 to run the
project it will create the database with three schemas and several tables. Once
the database is created then you should be able to run the app as a user with 
db_datareader and  db_datawriter permissions. However, this is a sample app
not intended for production usage.

Initially the columns are not encrypted. To encrypt them run the included
`Encryption.ps1`. This script supports the `-Debug` and `-Verbose` paramaters
This will create a self signed certificate in your user script repository and
encrypt all the appropiate columns.

Schemas
-------

There are there schemas in the sample database.

* **Logging:** This contains one table called log written to by 
[log4net](https://logging.apache.org/log4net/) using the `AdoNetAppender`. The
user name and client ip columns are encrypted using deterministric encryption.
Note that this hurts performance a lot The point of including this is to show
that as long as you are using paramaterized queries, Always Encrypted can be 
added without code changes.
* **Authentication:** This contains an implementation of the EF Code first
[IdentityContext](https://msdn.microsoft.com/en-us/library/microsoft.aspnet.identity.entityframework.identitydbcontext(v=vs.108).aspx).
This is the current recommended authentication framework for web apps for
ASP.NET applications. Since this already stores and transmits passwords in an
encrypted fashion, the PasswordHash is **not** encrypted via Always Encrypted.
However, a SSN (Social Security Number) field has been added to the User table
which is encrypted.
* **Purchasing:** This contains two tables for storing Credit Card information.
The credit card number is stored using random encryption and CCV code is stored
using deterministic encryption.

Using the app:
--------------
The app has two users `Administrator` with the password of `P3ter!` and CCAdmin
with the password of `P@ul!`. The user administrator has access to an
"Internals" page that lists the columns in the database, as well as the
encryption information for them. The CCadmin user has a page that will let
them import 1000 randomly generated fake credit cards, and then display them
in a table. The app also suppors self registration.