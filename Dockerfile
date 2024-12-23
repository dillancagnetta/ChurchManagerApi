FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
COPY ChurchManager.sln .
COPY . .
RUN dotnet restore 
COPY . .

FROM build AS publish
RUN dotnet publish --no-restore ./src/API/ChurchManager.Api/ChurchManager.Api.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# default environment variables - this will be set when we run the container again
ENV ASPNETCORE_ENVIRONMENT=Production 

ENTRYPOINT ["dotnet", "ChurchManager.Api.dll"]
