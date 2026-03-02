# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Runtime base image targeting ARM64
FROM --platform=linux/arm64 mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER root
# Install sqlcmd
RUN apt-get update && apt-get install -y curl apt-transport-https gnupg2 && \
    curl -sSL https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor -o /usr/share/keyrings/microsoft-prod.gpg && \
    curl -sSL https://packages.microsoft.com/config/debian/12/prod.list -o /etc/apt/sources.list.d/mssql-release.list && \
    apt-get update && \
    ACCEPT_EULA=Y apt-get install -y mssql-tools18 unixodbc-dev && \
    echo 'export PATH="$PATH:/opt/mssql-tools18/bin"' >> /etc/profile.d/mssql-tools.sh && \
    apt-get clean && rm -rf /var/lib/apt/lists/*
ENV PATH="$PATH:/opt/mssql-tools18/bin"
USER $APP_UID
WORKDIR /app
EXPOSE 8080

# Build stage targeting ARM64
FROM --platform=linux/arm64 mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Performance/Performance.csproj", "Performance/"]
RUN dotnet restore "./Performance/Performance.csproj"
COPY . .
WORKDIR "/src/Performance"
RUN dotnet build "./Performance.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Performance.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Performance.dll"]