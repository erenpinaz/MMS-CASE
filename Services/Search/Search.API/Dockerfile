FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env

WORKDIR /app
COPY . .

RUN dotnet publish Services/Search/Search.API/Search.API.csproj -c Release -o /out

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0

WORKDIR /app
COPY --from=build-env /out .

# Enable PGO
ENV DOTNET_TieredPGO 1 
ENV DOTNET_TC_QuickJitForLoops 1 
ENV DOTNET_ReadyToRun 0

ENTRYPOINT dotnet Search.API.dll