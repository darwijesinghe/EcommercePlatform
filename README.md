# E-Commerce Microservices Platform

## Project Purpose
This is a beginner-level backend e-commerce application built using a microservices architecture with .NET 8 and REST APIs.
The project is designed to showcase backend development skills and provide hands-on experience with event-driven architecture using RabbitMQ.

It includes three core services:

- **EP.Auth Service** – Handles authentication (JWT).
- **EP.User Service** – Handles user-related operations.
- **EP.Product Service** – Manages product creation.
- **EP.Order Service** – Processes orders.

Additionally, there's an **EP.Inventory Service**, which automatically updates stock levels whenever a product is created or an order is placed. These updates are triggered through messages sent via RabbitMQ, demonstrating asynchronous communication between services.

## Contributors
Darshana Wijesinghe

## App Features
- Modular microservices architecture.
- Containerized using Docker Compose.
- JWT authentication with role-based `(Admin, User)` authorization.
- Event-driven messaging with RabbitMQ.
- MSSQL for persistent storage.
- Inventory update on product creation via event.
- Configurable environment settings via `appsettings`.

## Packages
- RabbitMQ.Client
- Mapster

## Services Overview

NOTE: Some services are still under enhancement.

| **Service**                          | **Description**                                | **Port**            |
|:---                                  |:---                                            |:---                 |
| **Auth Service (EP.Auth)**           | Handles authentication and JWT token issuance  | 5001                |
| **User Service (EP.User)**           | User registration and validation               | 5002                |
| **Product Service (EP.Product)**     | Product management (CRUD)                      | 5003                |
| **Order Service (EP.Order)**         | Order creation and validation                  | 5004                |
| **Inventory Service (EP.Inventory)** | Inventory tracking per product                 | 5005                |
| **RabbitMQ**                         | Message broker for inter-service events        | 5672 / 15672 (UI)   |
| **SQL Server**                       | Database for services                          | 1433                |


## API Endpoints
```json5
Auth Service

- POST /api/auth/register – Register new user.
- POST /api/auth/login – Login user to get the tokens.
- POST /api/auth/refresh – Get a refresh token.

User Service

- POST /api/user/register-user – Register new user.
- POST /api/user/validate-user – User validation.
- GET  /api/user/get-all-user – Get all users.
- GET  /api/user/get-user?id={id} – Get a single user.

Product Service

- POST /api/product/add-product – Add product (publishes event to inventory service).
- Access: Accessible by users with the `Admin` role.

- GET  /api/product/get-all-products – List all products.
- GET  /api/product/get-a-product?id={id} – Get a single product.

Order Service

- POST /api/order/create-order – Place an order (validates products and stock).
- GET  /api/order/get-all-orders - Get all orders.
- GET  /api/order/get-a-order?id={id} - Get a single order.

Inventory Service

- GET  /api/inventory/get-inventory – List all inventory items.
```

## Sample Request: Add Product

`POST http://localhost:5002/api/product/add-product`

```json
{
  "name": "Keels banana",
  "description": "Fresh green banana 1Kg",
  "price": 275.00,
  "quantity": 5
}
```
### This triggers:

- Product creation in product service
- Inventory service receives event and creates inventory entry

## RabbitMQ Events

| **Event**           | **Triggered By** | **Consumed By**          | **Purpose**                       |
|:---                 |:---              |:---                      |:---                               |
| Product creation    | ProductService   | ProductCreatedConsumer   | Add product to inventory          |
| Order placing       | OrderService     | OrderPlacedConsumer      | Manage the product inventory      |


## Access services at:

http://localhost:5001/swagger – API.Auth

http://localhost:5002/swagger – API.User

http://localhost:5003/swagger – API.Product

http://localhost:5004/swagger – API.Ordeer

http://localhost:5005/swagger – API.Inventory

http://localhost:15672 – RabbitMQ UI

## Future Enhancements
- Centralized API Gateway (e.g., Ocelot)
- Unit & integration tests

## Support
Darshana Wijesinghe  
Email address - [dar.mail.work@gmail.com](mailto:dar.mail.work@gmail.com)  
Linkedin - [darwijesinghe](https://www.linkedin.com/in/darwijesinghe/)  
GitHub - [darwijesinghe](https://github.com/darwijesinghe)

## License
This project is licensed under the terms of the **MIT** license.