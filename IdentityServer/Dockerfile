FROM mcr.microsoft.com/dotnet/core/aspnet:3.1

ENV DOTNET_CLI_TELEMETRY_OPTOUT 1

ARG BuildConfiguration=Release

WORKDIR /app

COPY bin/Release/PublishOutput/ .

ENV App__Self__JwtAuthority = ' '

ENV App__Self__IdentityApplications = [ ]

ENV App__Api__DeXApiUrl = ' '

ENV App__Frontend__RedirectUrisFrontend = [ ]

ENV App__Frontend__PostLogoutUrisFrontend = [ ]

ENTRYPOINT ["dotnet", "6_IdentityServer.dll"]
