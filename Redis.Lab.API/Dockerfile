FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Redis.Lab.API/Redis.Lab.API.csproj", "Redis.Lab.API/"]
RUN dotnet restore "./Redis.Lab.API/Redis.Lab.API.csproj"
COPY . .
WORKDIR "/src/Redis.Lab.API"
RUN dotnet build "./Redis.Lab.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Redis.Lab.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Redis.Lab.API.dll"]