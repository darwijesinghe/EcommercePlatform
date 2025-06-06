# E-Commerce Microservices Platform

## Project Purpose
This is a beginner-level backend e-commerce application built using a microservices architecture with .NET 8 and REST APIs.
The project is designed to showcase backend development skills and provide hands-on experience with event-driven architecture using RabbitMQ.

It includes three core services:

- **API.User Service** – handles user-related operations
- **API.Product Service** – manages product creation
- **API.Order Service** – processes orders

Additionally, there's an **API.Inventory Service**, which automatically updates stock levels whenever a product is created or an order is placed. These updates are triggered through messages sent via RabbitMQ, demonstrating asynchronous communication between services.

## Contributors
Darshana Wijesinghe

## App Features
- Modular microservices architecture
- Containerized using Docker Compose
- Event-driven messaging with RabbitMQ
- MSSQL for persistent storage
- Inventory update on product creation via event
- Configurable environment settings via `appsettings`

## Packages
- RabbitMQ.Client
- Mapster

## Services Overview

NOTE: Some services are still under enhancement.

<table style="border: 1px solid #474747; border-collapse: collapse;" cellpadding="8" cellspacing="0">
  <thead>
    <tr style="border: 1px solid #474747;">
      <th style="border: 1px solid #474747;">Service</th>
      <th style="border: 1px solid #474747;">Description</th>
      <th style="border: 1px solid #474747;">Port</th>
    </tr>
  </thead>
  <tbody>
    <tr style="border: 1px solid #474747;">
      <td style="border: 1px solid #474747;"><strong>User Service (EP.User)</strong></td>
      <td style="border: 1px solid #474747;">User registration and authentication</td>
      <td style="border: 1px solid #474747;">5001</td>
    </tr>
    <tr style="border: 1px solid #474747;">
      <td style="border: 1px solid #474747;"><strong>Product Service (EP.Product)</strong></td>
      <td style="border: 1px solid #474747;">Product management (CRUD)</td>
      <td style="border: 1px solid #474747;">5002</td>
    </tr>
    <tr style="border: 1px solid #474747;">
      <td style="border: 1px solid #474747;"><strong>Order Service (EP.Order)</strong></td>
      <td style="border: 1px solid #474747;">Order creation and validation</td>
      <td style="border: 1px solid #474747;">5003</td>
    </tr>
    <tr style="border: 1px solid #474747;">
      <td style="border: 1px solid #474747;"><strong>Inventory Service (EP.Inventory)</strong></td>
      <td style="border: 1px solid #474747;">Inventory tracking per product</td>
      <td style="border: 1px solid #474747;">5004</td>
    </tr>
    <tr style="border: 1px solid #474747;">
      <td style="border: 1px solid #474747;"><strong>RabbitMQ</strong></td>
      <td style="border: 1px solid #474747;">Message broker for inter-service events</td>
      <td style="border: 1px solid #474747;">5672 / 15672 (UI)</td>
    </tr>
    <tr style="border: 1px solid #474747;">
      <td style="border: 1px solid #474747;"><strong>SQL Server</strong></td>
      <td style="border: 1px solid #474747;">Database for services</td>
      <td style="border: 1px solid #474747;">1433</td>
    </tr>
  </tbody>
</table>

## API Endpoints
```json5
User Service

- POST /api/user/register-user – Register new user
- GET  /api/user/get-all-user – Get all users

Product Service

- POST /api/product/add-product – Add product (publishes event to inventory service)
- GET  /api/product/get-all-products – List all products
- GET  /api/product/get-a-product?id={id} – Get product by ID

Order Service

- POST /api/order/create-order – Place an order (validates products and stock)
- GET  /api/order/get-all-orders - Get all orders
- GET  /api/order/get-a-order?id={id} - Get order by ID

Inventory Service

- GET  /api/inventory/get-inventory – List all inventory items
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

<table style="border: 1px solid #474747; border-collapse: collapse;" cellpadding="8" cellspacing="0">
  <thead>
    <tr style="border: 1px solid #474747;">
      <th style="border: 1px solid #474747;">Event</th>
      <th style="border: 1px solid #474747;">Triggered By</th>
      <th style="border: 1px solid #474747;">Consumed By</th>
      <th style="border: 1px solid #474747;">Purpose</th>
    </tr>
  </thead>
  <tbody>
    <tr style="border: 1px solid #474747;">
      <td style="border: 1px solid #474747;">Product creation</td>
      <td style="border: 1px solid #474747;">ProductService</td>
      <td style="border: 1px solid #474747;">ProductCreatedConsumer</td>
      <td style="border: 1px solid #474747;">Add product to inventory</td>
    </tr>
    <tr style="border: 1px solid #474747;">
      <td style="border: 1px solid #474747;">Order placing</td>
      <td style="border: 1px solid #474747;">OrderService</td>
      <td style="border: 1px solid #474747;">OrderPlacedConsumer</td>
      <td style="border: 1px solid #474747;">Manage the product inventory</td>
    </tr>
  </tbody>
</table>

## Access services at:

http://localhost:5001/swagger – API.User

http://localhost:5002/swagger – API.Product

http://localhost:5003/swagger – API.Ordeer

http://localhost:5004/swagger – API.Inventory

http://localhost:15672 – RabbitMQ UI

## Future Enhancements
- Centralized API Gateway (e.g., Ocelot)
- Adding authentication/authorization services
- Unit & integration tests

## Support
Darshana Wijesinghe  
Email address - [dar.mail.work@gmail.com](mailto:dar.mail.work@gmail.com)  
Linkedin - [darwijesinghe](https://www.linkedin.com/in/darwijesinghe/)  
GitHub - [darwijesinghe](https://github.com/darwijesinghe)

## License
This project is licensed under the terms of the **MIT** license.