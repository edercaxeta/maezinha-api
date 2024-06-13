# Estágio 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copie o arquivo .csproj e restaure as dependências
COPY *.csproj ./
RUN dotnet restore

# Copie o restante dos arquivos e construa a aplicação
COPY . ./
RUN dotnet publish -c Release -o out

# Estágio 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

ENTRYPOINT ["dotnet", "maezinha.dll"]
