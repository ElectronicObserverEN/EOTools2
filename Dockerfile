#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["EOToolsWeb.Api/EOToolsWeb.Api.csproj", "EOToolsWeb.Api/"]
COPY ["EOToolsWeb.Shared/EOToolsWeb.Shared.csproj", "EOToolsWeb.Shared/"]
RUN dotnet restore "./EOToolsWeb.Api/EOToolsWeb.Api.csproj"
COPY . .
WORKDIR "/src/EOToolsWeb.Api"
RUN dotnet build "./EOToolsWeb.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EOToolsWeb.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EOToolsWeb.Api.dll"]