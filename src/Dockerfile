# Use the official .NET SDK for build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR ./src

# Copy the solution and project files first (for layer caching)
COPY ["WorkoutTracker.sln", "."]
COPY ["WorkoutTracker.Web.Host/*.csproj", "WorkoutTracker.Web.Host/"]
COPY ["WorkoutTracker.Domain/*.csproj", "WorkoutTracker.Domain/"]
COPY ["WorkoutTracker.Application/*.csproj", "WorkoutTracker.Application/"]
COPY ["WorkoutTracker.Persistence/*.csproj", "WorkoutTracker.Persistence/"]
COPY ["WorkoutTracker.Infrastructure/*.csproj", "WorkoutTracker.Infrastructure/"]
COPY ["WorkoutTracker.Web.Presentation/*.csproj", "WorkoutTracker.Web.Presentation/"]

COPY ["tests/WorkoutTracker.Application.Tests/*.csproj", "tests/WorkoutTracker.Application.Tests/"]
COPY ["tests/WorkoutTracker.Architecture.Tests/*.csproj", "tests/WorkoutTracker.Architecture.Tests/"]
COPY ["tests/WorkoutTracker.Domain.Tests/*.csproj", "tests/WorkoutTracker.Domain.Tests/"]
COPY ["tests/WorkoutTracker.Infrastructure.Tests/*.csproj", "tests/WorkoutTracker.Infrastructure.Tests/"]
COPY ["tests/WorkoutTracker.Persistence.Tests/*.csproj", "tests/WorkoutTracker.Persistence.Tests/"]

# Restore using the solution file
RUN dotnet restore "WorkoutTracker.sln"

# Copy everything else
COPY . .

# Build and publish
WORKDIR WorkoutTracker.Web.Host
RUN dotnet publish -c Release -o /app

# Final runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "WorkoutTracker.Web.Host.dll"]
