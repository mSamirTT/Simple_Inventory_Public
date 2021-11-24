#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["src/StarterApp/StarterApp.csproj", "src/StarterApp/"]
COPY ["src/StarterApp.Core/StarterApp.Core.csproj", "src/StarterApp.Core/"]
COPY ["src/StarterApp.Infrastructure/StarterApp.Infrastructure.csproj", "src/StarterApp.Infrastructure/"]
RUN dotnet restore "src/StarterApp/StarterApp.csproj"
COPY . .
WORKDIR "/src/src/StarterApp"
RUN dotnet build "StarterApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StarterApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# ENTRYPOINT ["dotnet", "StarterApp.dll"]
# heroku uses the following
CMD ASPNETCORE_URLS=http://*:$PORT dotnet StarterApp.dll