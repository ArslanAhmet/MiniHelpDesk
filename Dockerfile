#build stage
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env

# Nerede calistigimizi gosteriyor
WORKDIR /minihelpdesk

#restore
COPY ["Services/MiniHelpDesk.Services.TicketManagement/MiniHelpDesk.Services.TicketManagement.csproj", "Services/MiniHelpDesk.Services.TicketManagement/"]
RUN dotnet restore "Services/MiniHelpDesk.Services.TicketManagement/MiniHelpDesk.Services.TicketManagement.csproj"

RUN ls -alR

COPY ["Tests/UnitTests/UnitTests.csproj", "UnitTests/"]
RUN dotnet restore "UnitTests/UnitTests.csproj"

#copy source
COPY . .

#test 
ENV TEAMCITY_PROJECT_NAME=fake
RUN dotnet test "UnitTests/UnitTests.csproj"

#publish
RUN dotnet publish "Services/MiniHelpDesk.Services.TicketManagement/MiniHelpDesk.Services.TicketManagement.csproj" -o /publish

#runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:5.0 
COPY --from=build-env /publish /publish
WORKDIR /publish

ENTRYPOINT ["dotnet","MiniHelpDesk.Services.TicketManagement.dll"]