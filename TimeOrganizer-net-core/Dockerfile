FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
ARG APP_UID=1001
ARG APP_GID=1001
RUN addgroup --gid $APP_GID appgroup \
    && adduser --uid $APP_UID --gid $APP_GID --disabled-password --gecos "" appuser

USER appuser

WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TimeOrganizer-net-core/TimeOrganizer-net-core.csproj", "TimeOrganizer-net-core/"]
RUN dotnet restore "TimeOrganizer-net-core/TimeOrganizer-net-core.csproj"
COPY . .
WORKDIR "/src/TimeOrganizer-net-core"
RUN dotnet build "TimeOrganizer-net-core.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "TimeOrganizer-net-core.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TimeOrganizer-net-core.dll"]
