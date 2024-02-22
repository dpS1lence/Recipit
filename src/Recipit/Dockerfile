# Use the ASP.NET Core runtime as the base image to run the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Use the .NET SDK image to build the project files
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Recipit/Recipit.csproj", "Recipit/"]
COPY ["Recipit.Infrastructure/Recipit.Infrastructure.csproj", "Recipit.Infrastructure/"]
RUN dotnet restore "Recipit/Recipit.csproj"
# Copy the rest of the source code
COPY . .
WORKDIR "/src/Recipit"
RUN dotnet build "Recipit.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "Recipit.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage/image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Recipit.dll"]
