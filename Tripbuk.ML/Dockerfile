FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Tripbuk.ML/Tripbuk.ML.csproj", "Tripbuk.ML/"]
RUN dotnet restore "Tripbuk.ML/Tripbuk.ML.csproj"
COPY . .
WORKDIR "/src/Tripbuk.ML"
RUN dotnet build "Tripbuk.ML.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Tripbuk.ML.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tripbuk.ML.dll"]
