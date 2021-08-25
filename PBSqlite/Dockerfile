#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.


#RASPBERRY PI Docker Image

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.2-buster-slim-arm32v7 AS base
WORKDIR /app
EXPOSE 5005
 
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src

COPY ["PBSqlite.csproj", "./"]
RUN dotnet restore "./PBSqlite.csproj"

COPY . .
WORKDIR "/src/."
RUN dotnet build "PBSqlite.csproj" -c Release -o /app/build
 
FROM build AS publish
RUN dotnet publish "PBSqlite.csproj" -c Release -o /app/publish
 
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PBSqlite.dll"]