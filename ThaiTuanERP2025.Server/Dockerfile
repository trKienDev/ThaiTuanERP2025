# ================================
# STAGE 1 — BUILD
# ================================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution file
COPY ThaiTuanERP2025.sln ./

# Copy project files ONE BY ONE
COPY ThaiTuanERP2025.Server/ThaiTuanERP2025.Api/*.csproj ThaiTuanERP2025.Server/ThaiTuanERP2025.Api/
COPY ThaiTuanERP2025.Server/ThaiTuanERP2025.Application/*.csproj ThaiTuanERP2025.Server/ThaiTuanERP2025.Application/
COPY ThaiTuanERP2025.Server/ThaiTuanERP2025.Domain/*.csproj ThaiTuanERP2025.Server/ThaiTuanERP2025.Domain/
COPY ThaiTuanERP2025.Server/ThaiTuanERP2025.Infrastructure/*.csproj ThaiTuanERP2025.Server/ThaiTuanERP2025.Infrastructure/

# Restore dependencies
RUN dotnet restore ThaiTuanERP2025.sln

# Copy entire source code
COPY . .

# Publish API project
RUN dotnet publish ThaiTuanERP2025.Server/ThaiTuanERP2025.Api/ThaiTuanERP2025.Api.csproj -c Release -o /app/publish

# ================================
# STAGE 2 — RUNTIME
# ================================
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "ThaiTuanERP2025.Api.dll"]
