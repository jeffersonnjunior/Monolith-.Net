FROM mcr.microsoft.com/dotnet/sdk:9.0 AS migrator

# Instala depend�ncias
RUN apt-get update && \
    apt-get install -y postgresql-client && \
    rm -rf /var/lib/apt/lists/*

# Instala EF Core CLI
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

WORKDIR /src

# Copia arquivos necess�rios
COPY ["Api/Api.csproj", "Api/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]

# Restaura pacotes
RUN dotnet restore "Api/Api.csproj"

# Copia todo o c�digo fonte
COPY . .

# Diret�rio de trabalho para execu��o de comandos
WORKDIR "/src/Api"