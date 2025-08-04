# Church Manager

## Dependencies 

	- RabbitMQ
	- PostgreSQL
	- Seq
	- Jaeger

## Running Locally

### Prerequisites

1. Set AWS Keys

> set AWS_ACCESS_KEY_ID =hello

> set AWS_SECRET_ACCESS_KEY =hello

> set AWS_REGION=us-east-1

> set SUBDOMAIN_KEY=localhost

**Powershell Commands**
```Powershell
dotnet tool update --global dotnet-ef --version 6.0.1

$Env:AWS_ACCESS_KEY_ID = "hello"
$Env:AWS_SECRET_ACCESS_KEY = "hello"
$Env:AWS_REGION = "us-east-1"
$Env:SUBDOMAIN_KEY = "localhost"

dotnet ef database update --project src\Infrastructure\ChurchManager.Infrastructure.Persistence\ChurchManager.Infrastructure.Persistence.csproj --startup-project src\API\ChurchManager.Api\ChurchManager.Api.csproj --context ChurchManager.Infrastructure.Persistence.Contexts.ChurchManagerDbContext
```

2. Run all the docker commands in the `docker containers` file in the root directory.

> docker compose -f docker-compose.dependencies.yml up

### Database

*Migrations Visual Studio*

1. In `Package manager console` change to `ChurchManager.Infrastructure.Persistence` project

2.
	- `Add-Migration InitialDbMigration -Context ChurchManagerDbContext -o Migrations -StartupProject ChurchManager.Api` 

3. 
	- `Update-Database -Context ChurchManagerDbContext -StartupProject ChurchManager.Api`

*Migrations Powershell/Rider*

1. cd into `Persistence` directory `cd src\Infrastructure\ChurchManager.Infrastructure.Persistence`
2. 
   - `dotnet ef migrations add LinkFamilyToPerson -c ChurchManagerDbContext -o Migrations -s ..\..\API\ChurchManager.Api\ChurchManager.Api.csproj`
3. cd back to root: ` cd ../../..`
   - `dotnet ef database update --project src\Infrastructure\ChurchManager.Infrastructure.Persistence\ChurchManager.Infrastructure.Persistence.csproj --startup-project src\API\ChurchManager.Api\ChurchManager.Api.csproj --context ChurchManager.Infrastructure.Persistence.Contexts.ChurchManagerDbContext`
#### Troubleshooting

`Cannot delete because connections are not closed`
	
	- in the docker container execute:   `psql -U admin`
	- `SELECT * FROM pg_stat_activity WHERE pg_stat_activity.datname='churchmanager_db';`  shows open connections
	- `SELECT pg_terminate_backend(pid) FROM pg_stat_activity WHERE datname = 'churchmanager_db';`   close open connections
	- `DROP DATABASE churchmanager_db;`

`Unable to create a 'DbContext' of type 'ChurchManagerDbContext'`
- check that the environment variables are set as per step 1
- logs should have values here and not empty: `AWS_ACCESS_KEY_ID : []  AWS_REGION  : []`

## Environment settings

Most settings for production e.g. database connection will come from `AWS Parameter store`.


## Updating

> dotnet tool install -g upgrade-assistant

> upgrade-assistant upgrade 'Path to csproj to upgrade'


## Reporting

https://odbc.postgresql.org/

https://postgresblog.blogspot.com/

> install 32bit driver
> Create ODBC DSN
> In ReportBuilder from Microsoft: set

Connection String
```Driver={PostgreSQL UNICODE};Server=localhost;Port=5432;Database=churchmanager_db;Schema=public;Uid=postgres;Pwd=*****;```

## Payments

Start reverse proxy locally

> ngrok http 5001

Update in `churchmanager_master_db` Tenants record : `ApiUrl` to ngrok address e.g. `https://82c40fcd5b90.ngrok-free.app`

Generate a payment request with `TestMode=true`

### Workflow

Phase 1: Payment Processing Only

- Create PaymentTransaction when payment is initiated (Status = Pending)
- Update to Completed/Failed based on PayFast notifications
- Do NOT create Giving records yet

Phase 2: Giving Creation via Batch Process

- Run a scheduled job (similar to bank import) to convert completed PaymentTransaction records to Giving
- This prevents duplicates when bank statements are imported later
- Maintains consistency with your existing reconciliation process

## Plugins

### Development
Plugins get copied and files dont get cleaned , so we need to run before `build`

` .\clear-plugins.ps1 ` then  do  a `build` to copy the new files

### Installation
 - need to logged in as user
 - Install plugins first via `PluginsController/install-all` - this will install the plugin for the tenant

## Jobs

## MultiTenancy
 - a subdomain e.g `test.churchmanager.io` will contain multiple tenants
 - tenants are defined in `MasterDbTenant` database and are grouped by the subdomain name e.g. `test`
 - migrations will be done for all tenants in a subdomain
 - subdomain is defined with environment variable: `SUBDOMAIN_KEY`
 - Tenants resolving process:
   - `Tenant` claim e.g. `tenant1` (admin authenticated users)
   - `tenant` querystring (login mostly)
   - `X-Tenant` header (public access urls translate tenant in url to the header)
