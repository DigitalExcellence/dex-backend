FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build-env

WORKDIR /app

COPY . ./

ENV ConnectionStrings__DefaultConnection = ''

ENV App__Self__JwtAuthority = ''

ENV App__Self__PublicOrigin = ''

ENV App__Api__DeXApiUrl = ''

ENV App__Api__ClientId = ''

ENV App__Api__ClientSecret = ''

ENV App__Frontend__RedirectUriFrontend = ''

ENV App__Frontend__RefreshUriFrontend = ''

ENV App__Frontend__RedirectUriFrontendPostman = ''

ENV App__Frontend__PostLogoutUrisFrontend = ''

ENV App__Frontend__ClientId = ''

ENV App__Frontend__ClientSecret = ''

ENV App__swagger__RedirectUrisSwagger = ''

ENV App__swagger__PostLogoutUrisSwagger = ''

ENV App__FfhictOIDC__ClientId = ''

ENV App__FfhictOIDC__ClientSecret = ''

ENV App__FfhictOIDC__RedirectUri = ''

ENV SENTRY_DSN = ''

ENV Use__In_Memory_Database=''

RUN dotnet tool install --tool-path ./ dotnet-certificate-tool --version 2.0.1

RUN dotnet publish IdentityServer/09_IdentityServer.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:3.1

WORKDIR /app

COPY --from=build-env /app .

ENTRYPOINT ["./certs/docker-entrypoint.sh"]
