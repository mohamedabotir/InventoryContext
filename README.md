# Inventory Service

This document outlines the design and implementation details of the **Inventory Service** using **Domain-Driven Design (DDD)**, **GraphQL**, **Kafka**, and **MongoDB** for recording events and **Sql server** all above services  used from docker as development evnironment was linux and ide rider

---

## Architecture Overview

### Key Components

1. **Domain-Driven Design (DDD)**:
   - Focuses on the core domain and its logic.
   - Utilizes aggregates, entities, value objects, repositories, and services.

2. **GraphQL**:
   - Provides a flexible query language and runtime for APIs.
   - Enables clients to query inventory data efficiently.

3. **Kafka**:
   - Acts as a message broker for event-driven communication.
   - Ensures reliable event publishing and processing.

4. **MongoDB**:
   - Used as the event store to persist raised events.
   - Stores inventory transactions and their lifecycle events.

---

## Service Features

### 1. **Create Item**
   - Accepts a GraphQL mutation to create a new item and its stock.
   - Validates input data against domain rules.
   - Publishes an `ItemCreated` event to Kafka.

### 2. **Track Inventory**
   - Consumes the `OrderShipped` event published by the Shipping Service.
   - Updates inventory to reflect stock changes.
   - Publishes an `OrderClosed` event to Kafka.

---

## Domain Model

### Value Objects
- **Stock**:
  - Attributes: `Id`, `Guid`, `ItemId`, `Quantity`, `Location`
- **Quantity**:
  - Attributes: `QuantityValue`, `QuantityType`

### Aggregates
- **Item Aggregate**:
  - Root entity ensuring business invariants.
  - Manages item data and stock lifecycle.

### Repositories
- Interface: `IItemRepository`, `IEventRepository`
- Implementation: SQL Server-backed repository for items, MongoDB for events.

---

## GraphQL API Design

### Schema
```graphql
schema {
  query: ItemQuery
}

type ItemQuery {
  items: [Item]
  item(id: ID!): Item
}

type Item {
  # The ID of Item.
  id: Long!

  # Item Description.
  descriptionValue: String!

  # Item Name.
  nameValue: String!

  # Stock keeping unit.
  sKUValue: String!

  # Item Price.
  moneyValue: Decimal!

  # Item Created Date.
  createdOn: DateTime!

  # Last Modified Date.
  modifiedOn: DateTime

  # Order Guid
  guid: ID!

  # The list of line items.
  stocks: [Stock]
}

scalar Long

scalar Decimal

# The `DateTime` scalar type represents a date and time. `DateTime` expects timestamps to be formatted in accordance with the [ISO-8601](https://en.wikipedia.org/wiki/ISO_8601) standard.
scalar DateTime

type Stock {
  # The ID of Item.
  id: Long!

  # Guid of Item.
  guid: ID!

  # Location Address.
  location: String!

  # Quantity Value.
  quantityValue: Int!

  # Quantity Type.
  quantityType: QuantityType!

  # Item Created Date.
  createdOn: DateTime!

  # Item Modified Date.
  modifiedOn: DateTime
}

enum QuantityType {
  KILO
  GRAM
  TAB
}
```

### Example Queries

#### Get All Items
```graphql
query {
  items {
    id
    nameValue
    descriptionValue
    sKUValue
    moneyValue
    createdOn
    stocks {
      location
      quantityValue
      quantityType
    }
  }
}
```

#### Get Item by ID
```graphql
query {
  item(id: "123") {
    id
    nameValue
    descriptionValue
    sKUValue
    moneyValue
    createdOn
    modifiedOn
    stocks {
      location
      quantityValue
      quantityType
    }
  }
}
```

---

## Event Sourcing and Kafka

### Event Types
1. **OrderClosed**:
   - Published when inventory is updated after an order is closed.

### Consumed Events
1. **OrderShipped**:
   - Consumed when shipping service marks an order as shipped.

### Kafka Configuration
- Topic: `inventoryTrack`
- Partitions: Based on `Item.Id`

### MongoDB Event Store
- Collection: `InventoryTransaction`
- Schema:
  ```json
  {
    "_id": "<EventId>",
    "itemId": "<ItemId>",
    "eventType": "<EventType>",
    "data": { <EventData> },
    "timestamp": "<Timestamp>"
  }
  ```

---

## Implementation Details

### Application Services
- **ItemService**:
  - Handles GraphQL requests.
  - Invokes domain methods and publishes events to Kafka.

### Event Handling
- Hosted service (`ConsumerHostingService`) consumes events from Kafka.
- Events are persisted in MongoDB and processed to update inventory.

### Example Workflow
1. User creates an item using api.
2. Domain validates and creates the item.
3. store it on database.
---------
 
1. Listener processes the `OrderShipped`
2. `OrderClosed` prepared
3. MongoDB stores the event.
4. An `OrderClosed` event is published to Kafka.

---

## Repositories

### Event Repository (`IEventRepository`)
- Interface for saving and retrieving events.

### Item Repository (`IItemRepository`)
- Interface for CRUD operations on items and stocks.

---

## Future Enhancements
1. Add Unit test
2. Implement retry mechanisms for failed Kafka message processing.
3. do some refactring 
