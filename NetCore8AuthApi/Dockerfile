FROM mcr.microsoft.com/dotnet/aspnet:8.0-preview-alpine AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0-preview-alpine AS build
WORKDIR /src
COPY ["NetCore8AuthApi/NetCore8AuthApi.csproj", "NetCore8AuthApi/"]
RUN dotnet restore "NetCore8AuthApi/NetCore8AuthApi.csproj"
COPY . .
WORKDIR "/src/NetCore8AuthApi"
RUN dotnet build "NetCore8AuthApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NetCore8AuthApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NetCore8AuthApi.dll"]
