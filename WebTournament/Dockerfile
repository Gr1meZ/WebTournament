FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["WebTournament/WebTournament.Presentation.MVC.csproj", "WebTournament/"]
COPY ["WebTournament.DependencyInjection/DependencyInjection.csproj", "WebTournament.DependencyInjection/"]
COPY ["DataAccess.Common/DataAccess.Common.csproj", "DataAccess.Common/"]
COPY ["DataAccess.PostgreSQL/DataAccess.PostgreSQL.csproj", "DataAccess.PostgreSQL/"]
COPY ["WebTournament.DataAccess.Abstract/DataAccess.Abstract.csproj", "WebTournament.DataAccess.Abstract/"]
COPY ["DataAccess.Domain/DataAccess.Domain.csproj", "DataAccess.Domain/"]
COPY ["WebApplication.Business/WebTournament.Business.csproj", "WebApplication.Business/"]
COPY ["WebTournament.Business.Abstract/WebTournament.Business.Abstract.csproj", "WebTournament.Business.Abstract/"]
COPY ["WebTournament.Models/WebTournament.Models.csproj", "WebTournament.Models/"]
RUN dotnet restore "WebTournament/WebTournament.Presentation.MVC.csproj"
COPY . .
WORKDIR "/src/WebTournament"
RUN dotnet build "WebTournament.Presentation.MVC.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebTournament.Presentation.MVC.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebTournament.Presentation.MVC.dll"]
