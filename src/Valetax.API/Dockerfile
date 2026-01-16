# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["Valetax.sln", "./"]
COPY ["src/Valetax.Domain/Valetax.Domain.csproj", "src/Valetax.Domain/"]
COPY ["src/Valetax.Application/Valetax.Application.csproj", "src/Valetax.Application/"]
COPY ["src/Valetax.Infrastructure/Valetax.Infrastructure.csproj", "src/Valetax.Infrastructure/"]
COPY ["src/Valetax.API/Valetax.API.csproj", "src/Valetax.API/"]

# Restore dependencies
RUN dotnet restore "src/Valetax.API/Valetax.API.csproj"

# Copy all source code
COPY . .

# Build the application
WORKDIR "/src/src/Valetax.API"
RUN dotnet build "Valetax.API.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "Valetax.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Valetax.API.dll"]
