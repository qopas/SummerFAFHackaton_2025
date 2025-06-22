FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project files - Updated for your actual structure
COPY ["Ediki.Domain/Ediki.Domain.csproj", "Ediki.Domain/"]
COPY ["Ediki.Application/Ediki.Application.csproj", "Ediki.Application/"]
COPY ["Ediki.Infrastructure/Ediki.Infrastructure.csproj", "Ediki.Infrastructure/"]
COPY ["Ediki.Api/Ediki.Api.csproj", "Ediki.Api/"]

# Restore dependencies
RUN dotnet restore "Ediki.Api/Ediki.Api.csproj"

# Copy all source code
COPY . .

# Build the application
WORKDIR "/src/Ediki.Api"
RUN dotnet build "Ediki.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ediki.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Create non-root user for security
RUN adduser --disabled-password --gecos '' appuser && chown -R appuser /app
USER appuser

# Configure environment for Railway
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "Ediki.Api.dll"] 