FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build-env

WORKDIR /app

COPY . ./

ENV ConnectionStrings__DefaultConnection = ''

ENV App__Frontend__FrontendUrl = ''

ENV App__Frontend__ClientId = ''

ENV App__Frontend__ClientSecret = ''

ENV App__IdentityServer__IdentityUrl = ''

ENV SENTRY_DSN = ''

RUN dotnet publish API/01_API.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:3.1

WORKDIR /app

COPY --from=build-env /app/out .

ENTRYPOINT ["dotnet", "01_API.dll"]
