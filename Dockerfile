# Use the official .NET SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
WORKDIR /app

# Copy solution file and project files
COPY *.sln ./
COPY Ediki.Api/*.csproj ./Ediki.Api/
COPY Ediki.Application/*.csproj ./Ediki.Application/
COPY Ediki.Domain/*.csproj ./Ediki.Domain/
COPY Ediki.Infrastructure/*.csproj ./Ediki.Infrastructure/
COPY Ediki.Tests.Unit/*.csproj ./Ediki.Tests.Unit/
COPY Ediki.Tests.Integration/*.csproj ./Ediki.Tests.Integration/

# Restore dependencies
RUN dotnet restore

# Copy the entire source code
COPY . ./

# Build and publish the application
RUN dotnet publish Ediki.Api/Ediki.Api.csproj -c Release -o /app/publish --no-restore

# Use the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS runtime
WORKDIR /app

# Install .NET EF tool
RUN apk add --no-cache curl
RUN dotnet tool install --global dotnet-ef --version 9.0.0
ENV PATH="$PATH:/root/.dotnet/tools"

# Copy the published application
COPY --from=build /app/publish .

# Create entrypoint script
RUN echo '#!/bin/sh' > /app/entrypoint.sh && \
    echo 'echo "Starting application..."' >> /app/entrypoint.sh && \
    echo 'echo "Waiting for database to be ready..."' >> /app/entrypoint.sh && \
    echo 'sleep 10' >> /app/entrypoint.sh && \
    echo 'echo "Running database migrations..."' >> /app/entrypoint.sh && \
    echo 'dotnet ef database update --no-build --verbose' >> /app/entrypoint.sh && \
    echo 'echo "Starting the API..."' >> /app/entrypoint.sh && \
    echo 'exec dotnet Ediki.Api.dll' >> /app/entrypoint.sh && \
    chmod +x /app/entrypoint.sh

# Expose port 8080 (default for .NET 9)
EXPOSE 8080

# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080

# Run the application
ENTRYPOINT ["/app/entrypoint.sh"] 