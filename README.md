# Documentação do Projeto

## Regra de Negócio
Projeto para gerenciamento de cinema, com funcionalidades como compra de ingressos e gestão de salas, incluindo organização de assentos e sessões de filmes.

## Tecnologias
- C#, ASP.NET Core, PostgreSQL, Entity Framework, xUnit e Docker

## Padrões de Design
- DDD, SOLID, Factory, Notification  

## Arquitetura
Monolítica com Clean Architecture, garantindo modularidade e separação de responsabilidades.

# Separação do Projeto

## Camada Api

### Controllers
Contém as controllers responsáveis por gerenciar as requisições e respostas da aplicação, servindo como ponto de entrada para os endpoints.

### Filters
Armazena a configuração dos filters, que gerenciam exceções e fornecem um tratamento consistente para erros no projeto.

### Middlewares
Configuração exclusiva para políticas de CORS, permitindo controlar o acesso de origens externas à API.

### Versioning
Contém a configuração de versionamento, permitindo gerenciar diferentes versões da API de forma organizada.

## Camada Application

### DependencyInjection
Configuração dos serviços de injeção de dependência para garantir a resolução correta das classes na camada de aplicação.

### Dtos
Definição dos Data Transfer Objects (DTOs) para transferência de dados entre as camadas de forma estruturada e segura.

### Factory
Mapeamento entre as entidades e os DTOs, facilitando a conversão de dados para comunicação entre as camadas.

### Interfaces
Contratos que definem as operações e métodos utilizados na camada de aplicação, garantindo flexibilidade e desacoplamento.

### Services
Implementação das regras de negócio, onde são realizadas as operações principais da aplicação.

### Specification
Validação e tratamento inicial dos dados, assegurando que atendem aos critérios definidos antes de seguir para o processamento.


