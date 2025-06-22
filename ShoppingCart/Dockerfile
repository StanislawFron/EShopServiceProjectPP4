# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ShoppingCart/ShoppingCart.csproj", "ShoppingCart/"]
RUN dotnet restore "./ShoppingCart/ShoppingCart.csproj"
COPY . .
WORKDIR "/src/ShoppingCart"
RUN dotnet build "./ShoppingCart.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ShoppingCart.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ShoppingCart.dll"]

# ============= ADD THIS DEVELOPMENT STAGE =============
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS development
USER root
RUN apt-get update && apt-get install -y curl

# Instalacja narzędzia dotnet-ef
RUN dotnet tool install --global dotnet-ef --version 8.0.14

ARG USER_UID=1000
ARG USER_GID=1000
RUN groupadd -g $USER_GID developer && \
    useradd -m -u $USER_UID -g $USER_GID developer

ENV DOTNET_USE_POLLING_FILE_WATCHER=1
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://*:8082

WORKDIR /workspace/ShoppingCart
USER developer

RUN dotnet tool install --global dotnet-watch

COPY wait-for-it.sh /wait-for-it.sh

CMD ["dotnet", "watch", "run", "--no-launch-profile"]