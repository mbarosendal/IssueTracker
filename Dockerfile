FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["IssueTracker/IssueTracker.csproj", "IssueTracker/"]
RUN dotnet restore "IssueTracker/IssueTracker.csproj"

COPY . .
WORKDIR "/src/IssueTracker"

RUN dotnet publish "IssueTracker.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS migrator
WORKDIR /src

COPY ["IssueTracker/IssueTracker.csproj", "IssueTracker/"]
RUN dotnet restore "IssueTracker/IssueTracker.csproj"

COPY . .
WORKDIR "/src/IssueTracker"

RUN dotnet tool install --global dotnet-ef --version 8.*
ENV PATH="$PATH:/root/.dotnet/tools"

ENTRYPOINT ["dotnet", "ef", "database", "update"]


FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "IssueTracker.dll"]