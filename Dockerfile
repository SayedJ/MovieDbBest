
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source

COPY *.sln .
COPY *.csproj
RUN dotnet restore

COPY ..
WORKDIR /source
RUN dotnet publish -c release -o /publish --no-restore

RUN dotnet publish webapp-cloudrun.csproj -o /publish
WORKDIR /publish

ENV ASPNETCORE_URLS="http://0.0.0.0:8080"

FROM mcr.microsoft.com/dotnet/asp.net:6.0
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "webapp-cloudrun.dll"]
