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

**Powershell Commands**
```Powershell
dotnet tool update --global dotnet-ef --version 6.0.1

$Env:AWS_ACCESS_KEY_ID = "hello"
$Env:AWS_SECRET_ACCESS_KEY = "hello"
$Env:AWS_REGION = "us-east-1"

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

## Configurations

AWS Cognito IDP
> https://cognito-idp.us-east-1.amazonaws.com/us-east-1_i6pWJxu8q/.well-known/openid-configuration


### Manual Deploy

> set AWS_DEFAULT_PROFILE=personal

```
aws ecr get-login-password --region us-east-1 | docker login --username AWS --password-stdin 977844596384.dkr.ecr.us-east-1.amazonaws.com
```

1. Create ECR Respository

```
aws ecr create-repository --repository-name frontend-angular --image-scanning-configuration scanOnPush=true --image-tag-mutability IMMUTABLE  --region us-east-1 --profile personal
```

2. Build the docker image
3. Tag the docker image

```
docker tag church-manager-ui:latest 977844596384.dkr.ecr.us-east-1.amazonaws.com/frontend-angular:local
```

4. Push the image

`docker push 977844596384.dkr.ecr.us-east-1.amazonaws.com/frontend-angular:local`


## Reporting

https://odbc.postgresql.org/

https://postgresblog.blogspot.com/

> install 32bit driver
> Create ODBC DSN
> In ReportBuilder from Microsoft: set

Connection String
```Driver={PostgreSQL UNICODE};Server=localhost;Port=5432;Database=churchmanager_db;Schema=public;Uid=postgres;Pwd=*****;```




