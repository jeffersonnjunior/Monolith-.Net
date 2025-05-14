FROM mcr.microsoft.com/dotnet/sdk:9.0

RUN apt-get update && \
    apt-get install -y postgresql-client && \
    dotnet tool install --global dotnet-ef && \
    rm -rf /var/lib/apt/lists/*

ENV PATH="$PATH:/root/.dotnet/tools"

WORKDIR /src