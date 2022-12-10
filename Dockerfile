
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ..
RUN dotnet restore

RUN dotnet publish webapp-cloudrun.csproj -o /publish
WORKDIR /publish
ENV ASPNETCORE_URLS="http://0.0.0.0:8080"

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "webapp-cloudrun.dll"]
