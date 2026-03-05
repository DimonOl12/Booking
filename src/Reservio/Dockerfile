# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# copy csproj and restore as distinct layers
WORKDIR /src
COPY Reservio.sln ./

COPY Reservio.Domain/*.csproj ./Reservio.Domain/
COPY Reservio.Application/*.csproj ./Reservio.Application/
COPY Reservio.Infrastructure/*.csproj ./Reservio.Infrastructure/
COPY Reservio.WebApi/*.csproj ./Reservio.WebApi/
COPY Reservio.WebApi.Tests/*.csproj ./Reservio.WebApi.Tests/
RUN dotnet restore

# copy everything else and build app
COPY . .

WORKDIR /src/Reservio.WebApi
RUN dotnet publish "Reservio.WebApi.csproj" -c Release -o /app

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .

# information
EXPOSE 8080

# environments
ENV ASPNETCORE_ENVIRONMENT=Docker

# run
ENTRYPOINT ["dotnet", "Reservio.WebApi.dll"]
