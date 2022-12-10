FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 443


FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src



# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore "webapp-cloudrun.csproj"

# Copy everything else and build
COPY . ./
WORKDIR "/src/."
RUN dotnet build "webapp-cloudrun.csproj" -c Release -o /app/build

FROM build As publish
RUN dotnet publish "webapp-cloudrun.csproj" -c Release -o /app/publish /p:UserAppHost=false

# Build runtime image
FROM base as final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "webapp-cloudrun.dll"]
