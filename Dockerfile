FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY *.csproj .
RUN dotnet restore "webapp-cloudrun.csproj"
COPY . .
RUN dotnet publish "webapp-cloudrun.csproj" -c Release -o /app/publish



# final stage/image
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "webapp-cloudrun.dll"]
