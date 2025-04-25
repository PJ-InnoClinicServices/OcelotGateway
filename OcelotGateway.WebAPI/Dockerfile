FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8087

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["OcelotGateway.WebAPI/OcelotGateway.WebAPI.csproj", "OcelotGateway.WebAPI/"]
RUN dotnet restore "OcelotGateway.WebAPI/OcelotGateway.WebAPI.csproj"
COPY . .
WORKDIR "/src/OcelotGateway.WebAPI"
RUN dotnet build "OcelotGateway.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "OcelotGateway.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OcelotGateway.WebAPI.dll"]
