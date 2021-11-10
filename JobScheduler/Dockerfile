FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build-env

WORKDIR /app

COPY . ./

RUN dotnet publish JobScheduler/13_JobScheduler.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:3.1

WORKDIR /app

COPY --from=build-env /app/out .

ENTRYPOINT ["dotnet", "14_JobScheduler.dll"]
