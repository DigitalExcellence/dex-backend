FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env

WORKDIR /app

COPY . ./

ENV ConnectionStrings__DefaultConnection = ''

ENV App__Frontend__FrontendUrl = ''

ENV App__Frontend__ClientId = ''

ENV App__Frontend__ClientSecret = ''

ENV App__IdentityServer__IdentityUrl = ''

ENV App__Sentry__ApiDsn = ''

RUN dotnet publish API/1_API.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1

WORKDIR /app

COPY --from=build-env /app/out .

ENTRYPOINT ["dotnet", "1_API.dll"]