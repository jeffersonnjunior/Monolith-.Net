# Documentação do Projeto

## Regra de Negócio
Projeto para gerenciamento de cinema, com funcionalidades como compra de ingressos e gestão de salas, incluindo organização de assentos e sessões de filmes.

## Tecnologias
- C#, ASP.NET Core, PostgreSQL, Entity Framework, xUnit, Docker

## Padrões de Design
- DDD, SOLID, Factory, Notification  

## Arquitetura
Monolítica com Clean Architecture, garantindo modularidade e separação de responsabilidades.

# Separação do Projeto

## Camada API

### Controllers
Contém as controllers responsáveis por gerenciar as requisições e respostas da aplicação, servindo como ponto de entrada para os endpoints.

### Filters
Armazena a configuração dos filters, que gerenciam exceções e fornecem um tratamento consistente para erros no projeto.

### Middlewares
Configuração exclusiva para políticas de CORS, permitindo controlar o acesso de origens externas à API.

### Versioning
Contém a configuração de versionamento, permitindo gerenciar diferentes versões da API de forma organizada.

