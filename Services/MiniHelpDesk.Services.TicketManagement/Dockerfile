# environment olarak base set ediyor
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# build tage
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Services/MiniHelpDesk.Services.TicketManagement/MiniHelpDesk.Services.TicketManagement.csproj", "Services/MiniHelpDesk.Services.TicketManagement/"]
COPY ["BuildingBlocks/Common/Common.csproj", "BuildingBlocks/Common/"]
RUN dotnet restore "Services/MiniHelpDesk.Services.TicketManagement/MiniHelpDesk.Services.TicketManagement.csproj"
COPY . .
WORKDIR "/src/Services/MiniHelpDesk.Services.TicketManagement"
RUN dotnet build "MiniHelpDesk.Services.TicketManagement.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MiniHelpDesk.Services.TicketManagement.csproj" -c Release -o /app/publish

# Runtime tage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MiniHelpDesk.Services.TicketManagement.dll"]