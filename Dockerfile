FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# copy csproj(s) and restore separately so dependencies are cached
COPY cdn/*.csproj ./
RUN dotnet restore

# copy the rest of the app
COPY cdn/. ./
RUN dotnet publish -c Release -o /app/publish /p:TrimUnusedDependencies=true

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish ./

EXPOSE 80
ENTRYPOINT ["dotnet", "cdn.dll"]
