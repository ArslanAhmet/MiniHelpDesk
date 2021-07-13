#Build Stage
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /integration

#restore

COPY IntegrationTests.csproj .
RUN dotnet restore 

#copy source
COPY . .

ENTRYPOINT ["dotnet", "test"]
