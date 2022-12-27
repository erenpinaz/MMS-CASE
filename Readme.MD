# Merchandise Management System (Product Domain Implementation)

Stack:
- NET Core 7
- EF Core 7
- PostgreSQL
- Elasticsearch
- RabbitMQ
- Swagger
- Polly
- Fluent Validation

Requirements:
- NET Core 7 SDK
- EF Core tools
- Docker
- Docker-Compose v2

Service URLs:
- pgAdmin: http://localhost:5050/
- RabbitMQ Management: http://localhost:15672/
- Kibana: http://localhost:5601/
- Catalog.API: http://localhost:5117/swagger/index.html 
- Search.API: http://localhost:5118/swagger/index.html

TODOS
- System Overview Schema
- ApiGateway

Instructions

```
# To start the system
docker compose -f ./docker-compose-prod.yml up -d --build

# To Stop the system
docker compose -f ./docker-compose-prod.yml down
```