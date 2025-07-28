FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
COPY ChurchManager.sln .
COPY . .
RUN dotnet restore 

FROM build AS publish
# Publish the API
RUN dotnet publish --no-restore ./src/API/ChurchManager.Api/ChurchManager.Api.csproj -c Release -o /app/publish

# Publish each plugin to the plugins directory
RUN dotnet publish --no-restore ./src/Plugins/Payments.PayFast/Payments.PayFast.csproj -c Release -o /app/publish/Plugins/Payments.PayFast

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Production 
ENTRYPOINT ["dotnet", "ChurchManager.Api.dll"]