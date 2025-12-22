FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src

COPY . .
RUN dotnet restore

# Entrypoint contains the command to run tests
ENTRYPOINT ["dotnet", "test", "tests/CDN.Tests"]
