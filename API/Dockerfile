FROM mcr.microsoft.com/dotnet/core/aspnet:3.1

ENV DOTNET_CLI_TELEMETRY_OPTOUT 1

ARG BuildConfiguration=Release

WORKDIR /app

COPY bin/Release/PublishOutput/ .

ENV ConnectionStrings__DefaultConnection = ' '

ENV App__Frontend__FrontendUrl = ' '

ENV App__Frontend__ClientId = ' '

ENV App__Frontend__ClientSecret = ' '

ENV App__IdentityServer__IdentityUrl = ' '

ENTRYPOINT ["dotnet", "1_API.dll"]
