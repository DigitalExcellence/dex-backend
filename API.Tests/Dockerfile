FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build-env

COPY . .


ENV ApiAddress = "https://api:5001/"
ENV IdentityAddress = "https://identity:5005/"


WORKDIR /API.Tests


ENTRYPOINT ["dotnet", "test"]
