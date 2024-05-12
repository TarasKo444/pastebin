FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["backend/Pastebin.Api/Pastebin.Api.csproj", "backend/Pastebin.Api/"]
COPY ["backend/Pastebin.Application/Pastebin.Application.csproj", "backend/Pastebin.Application/"]
COPY ["backend/Pastebin.Infrastructure/Pastebin.Infrastructure.csproj", "backend/Pastebin.Infrastructure/"]
COPY ["backend/Pastebin.Common/Pastebin.Common.csproj", "backend/Pastebin.Common/"]
COPY ["backend/Pastebin.Domain/Pastebin.Domain.csproj", "backend/Pastebin.Domain/"]
RUN dotnet restore "backend/Pastebin.Api/Pastebin.Api.csproj"
COPY . .
WORKDIR "/src/backend/Pastebin.Api"
RUN dotnet build "Pastebin.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Pastebin.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pastebin.Api.dll"]
