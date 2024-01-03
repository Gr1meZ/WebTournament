FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["WebTournament/WebTournament.Presentation.MVC.csproj", "WebTournament/"]
COPY ["WebTournament.Application/WebTournament.Application.csproj", "WebTournament.Application/"]
COPY ["WebTournament.Domain/WebTournament.Domain.csproj", "WebTournament.Domain/"]
COPY ["WebTournament.Infrastructure.IoC/WebTournament.Infrastructure.IoC.csproj", "WebTournament.Infrastructure.IoC/"]
COPY ["WebTournament.Infrastructure.Data/WebTournament.Infrastructure.Data.csproj", "WebTournament.Infrastructure.Data/"]
COPY ["WebTournament.Infrastructure.Identity/WebTournament.Infrastructure.Identity.csproj", "WebTournament.Infrastructure.Identity/"]
RUN dotnet restore "WebTournament/WebTournament.Presentation.MVC.csproj"
COPY . .
WORKDIR "/src/WebTournament"
RUN dotnet build "WebTournament.Presentation.MVC.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "WebTournament.Presentation.MVC.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebTournament.Presentation.MVC.dll"]
