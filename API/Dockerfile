FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
 
ENV DOTNET_CLI_TELEMETRY_OPTOUT 1
 
ARG BuildConfiguration=Release

WORKDIR /app

COPY bin/Release/PublishOutput/DigitalExcellence.Api .

# ENV ConnectionString = ${ConnectionStrings__DefaultConnection}

ENTRYPOINT ["dotnet", "1_API.dll"]