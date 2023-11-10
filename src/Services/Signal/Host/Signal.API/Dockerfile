#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Services/Signal/Host/Signal.API/Signal.API.csproj", "src/Services/Signal/Host/Signal.API/"]
COPY ["src/BuildingBlocks/Common/Common.csproj", "src/BuildingBlocks/Common/"]
COPY ["src/Services/Signal/Core/Application/Signal.Application/Signal.Application.csproj", "src/Services/Signal/Core/Application/Signal.Application/"]
COPY ["src/Services/Signal/Core/Domain/Signal.Domain/Signal.Domain.csproj", "src/Services/Signal/Core/Domain/Signal.Domain/"]
COPY ["src/Services/Signal/Infrastructure/Signal.Infrastructure/Signal.Infrastructure.csproj", "src/Services/Signal/Infrastructure/Signal.Infrastructure/"]
RUN dotnet restore "src/Services/Signal/Host/Signal.API/Signal.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Signal/Host/Signal.API"
RUN dotnet build "Signal.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Signal.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Signal.API.dll"]