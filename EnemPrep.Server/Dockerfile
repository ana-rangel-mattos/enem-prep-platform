FROM mcr.microsoft.com/dotnet/sdk:9.0 AS migrator
WORKDIR /src

RUN dotnet tool install --global dotnet-ef --version 9.*
ENV PATH="$PATH:/root/.dotnet/tools"

COPY . .
RUN dotnet restore EnemPrep.Api/EnemPrep.Api.csproj

FROM migrator AS publish
RUN dotnet publish EnemPrep.Api/EnemPrep.Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS final
WORKDIR /app
RUN apt-get update && apt-get install -y --no-install-recommends curl && rm -rf /var/lib/apt/lists/*
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EnemPrep.Api.dll"]
