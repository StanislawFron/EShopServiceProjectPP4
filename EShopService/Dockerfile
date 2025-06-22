# Existing configuration - LEAVE UNCHANGED
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["EShopService/EShopService.csproj", "EShopService/"]
RUN dotnet restore "./EShopService/EShopService.csproj"
COPY . .
WORKDIR "/src/EShopService"
RUN dotnet build "./EShopService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EShopService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY wait-for-it.sh /wait-for-it.sh
ENTRYPOINT ["dotnet", "EShopService.dll"]

# ============= ADD THIS DEVELOPMENT STAGE =============
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS development
USER root

# Konfiguracja użytkownika developerskiego
ARG USER_UID=1000
ARG USER_GID=1000
RUN groupadd -g $USER_GID developer && \
    useradd -m -u $USER_UID -g $USER_GID developer && \
    mkdir -p /home/developer/.nuget/packages && \
    chown -R developer:developer /home/developer

# Środowisko developerskie
ENV DOTNET_USE_POLLING_FILE_WATCHER=1
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://*:8080

WORKDIR /workspace/EShopService
USER developer

# Instalacja narzędzi developerskich
RUN dotnet tool install --global dotnet-watch

ENTRYPOINT ["dotnet", "watch", "run", "--no-launch-profile"]