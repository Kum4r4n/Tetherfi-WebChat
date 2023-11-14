#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Services/Message/Host/Message.API/Message.API.csproj", "src/Services/Message/Host/Message.API/"]
COPY ["src/BuildingBlocks/Common/Common.csproj", "src/BuildingBlocks/Common/"]
COPY ["src/Services/Message/Infrastructure/Message.Infrastructure/Message.Infrastructure.csproj", "src/Services/Message/Infrastructure/Message.Infrastructure/"]
COPY ["src/Services/Message/Core/Application/Message.Application/Message.Application.csproj", "src/Services/Message/Core/Application/Message.Application/"]
COPY ["src/Services/Message/Core/Domain/Message.Domain/Message.Domain.csproj", "src/Services/Message/Core/Domain/Message.Domain/"]
RUN dotnet restore "src/Services/Message/Host/Message.API/Message.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Message/Host/Message.API"
RUN dotnet build "Message.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Message.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Message.API.dll"]