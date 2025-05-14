FROM mcr.microsoft.com/dotnet/sdk:9.0 AS migrator

# Instala dependências
RUN apt-get update && \
    apt-get install -y postgresql-client && \
    rm -rf /var/lib/apt/lists/*

# Instala EF Core CLI
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

WORKDIR /src

# Copia arquivos necessários
COPY ["Api/Api.csproj", "Api/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]

# Restaura pacotes
RUN dotnet restore "Api/Api.csproj"

# Copia todo o código fonte
COPY . .

# Diretório de trabalho para execução de comandos
WORKDIR "/src/Api"