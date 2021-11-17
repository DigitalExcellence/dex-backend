FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build-env

COPY . .


ENV ApiAddress = "https://api:5001/"
ENV IdentityAddress = "http://identity:5004/"


WORKDIR /API.Tests


ENTRYPOINT ["dotnet", "test","--filter 02_API.Tests"]
