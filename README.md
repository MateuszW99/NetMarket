## List of contents
* ### [Developing the database](#developing-the-database)
* ### [Endpoints (WIP)](#endpoints-wip)
* ### [Resources](#resources)

## Developing the database

- Connection string can be found in [`appsettings.json`](./src/Api/appsettings.json) file

- To add migration (make sure you start in Infrastructure directory): <br>
``dotnet ef migrations add <name of new migration> -c ApplicationDbContext -s ./../Api`` <br> 

- To remove migration (make sure you start in Infrastructure directory): <br>
`dotnet ef migrations remove -c ApplicationDbContext -s ./../Api` 

## Endpoints (WIP)

JWT token must be provided in the header when accessing sensitive data.

### Items
| Method | Path           | Body       | Params     | Description                                                 | Responses          | Who can access |
|--------|----------------|------------|------------|-------------------------------------------------------------|--------------------|----------------|
| GET    | api/items/{id} | *none*     | id         | Returns an ItemCard with given id or 404 if item doesn't exist. | [ItemCard](./src/Application/ApiModels/Items/ItemCard.cs), 404    | everyone       |
| GET    | api/items?     | *none*     | brand, name, category, make, model | Returns a list of items. List can be empty if none is found. | ItemObject[] | everyone |
| GET    | api/items/category?  | *none*     | category   | Returns list of items with given category name        | ItemObject[]       | everyone       |
| POST   | api/items      | CreateItemRequest | *none* | Creates a new item based on the data in the body.  Returns 404 error code if data is invalid. | 200, 500 | admin |
| PUT    | api/items/{id} | UpdateItemRequest | id | Updates an item if found.  Returns 404 otherwise.            | 200, 404            | admin         |
| DELETE | api/items/{id} | DeleteItemRequest | id | Updates an item if found.  Returns 404 otherwise.            | 200, 404            | admin         |

### UserSettings
| Method | Path           | Body       | Params     | Description                                                 | Responses          | Who can access |
|--------|----------------|------------|------------|-------------------------------------------------------------|--------------------|----------------|
| GET    | api/user       | *none*     | *none*     | Returns user data.                                          | UserSettingsObject | Signed in users |
| PUT    | api/user       | *none*     | *none*     | Returns user data.                                          | 200                | Signed in users |

### Asks
| Method | Path           | Body       | Params     | Description                                                 | Responses          | Who can access |
|--------|----------------|------------|------------|-------------------------------------------------------------|--------------------|----------------|
| GET    | api/asks/{id}  | *none*     | id         | Returns ask details or 404 it ask doesn't exist.            | AskObject, 404     | Signed in users |
| GET    | api/asks/      | *none*     | *none*     | Returns list of asks for a user.  List can be empty if user has no asks | AskObject[] | Signed in users   |
| POST   | api/asks/      | CreateAskRequest | *none* | Creates a new ask.                                        | 200                | Signed in users |
| PUT    | api/asks/{id}  | UpdateAskRequest | id   | Updates an ask if found. Returns 404 otherwise.             | 200, 404           | Ask owner       |
| DELETE | api/asks/{id}  | DeleteAskRequest | id   | Deletes an ask if found. Returns 404 otherwise.             | 200, 404           | Ask owner       |

### Bids
| Method | Path           | Body       | Parameters | Description                                                 | Responses           | Who can access |
|--------|----------------|------------|------------|-------------------------------------------------------------|--------------------|----------------|
| GET    | api/bids/{id}  | *none*     | id         | Returns bid details or 404 it ask doesn't exist.            | BidObject, 404     | Signed in users |
| GET    | api/bids/      | *none*     | *none*     | Returns list of bids for a useer.  List can be empty if user has no bids | BidObject[] | Signed in users   |
| POST   | api/bids/      | CreateBidRequest | *none* | Creates a new bid.                                        | 200                | Signed in users |
| PUT    | api/bids/{id}  | UpdateBidRequest | id   | Updates a bid if found. Returns 404 otherwise.              | 200, 404           | Bid owner       |
| DELETE | api/bids/{id}  | DeleteBidRequest | id   | Deletes a bid if found. Returns 404 otherwise.              | 200, 404           | Bid owner       |

### Transactions
| Method | Path           | Body       | Params     | Description                                                 | Responses          | Who can access |
|--------|----------------|------------|------------|-------------------------------------------------------------|--------------------|----------------|
| GET    | api/transactions/{id} | *none* | id |    | Returns TransactionObject if transaction with given id exists.  Otherwise, returns 404. Code 403 is returned when user doesn't own the transaction.                                                                                                  | TransactionObject[], 404, 403 | Buyer and seller
| GET   | api/transactions/ | *none*   | *none*     | Returns list of user transactions.                          | TransactionObject[] | Signed in user |

### AdminPanel
| Method | Path           | Body       | Params     |  Description                                                | Responses | Who can access |
|--------|----------------|------------|------------|-------------------------------------------------------------|--------------------|----------------|
| GET    | api/adminPanel/orders |          |            | Returns list of transactions.                               | TransactionObject[]| admin          |
| PUT    | api/adminPanel/orders/{id} | UpdateTransactionRequest[] | id | Updates a transaction if found. Otherwise returns 404. | 200, 404 | admin          |

### SupervisorPanel
| Method | Path           | Body       | Params     |  Description                                                | Responses          | Who can access |
|--------|----------------|------------|------------|-------------------------------------------------------------|--------------------|----------------|
| GET    | api/supervisorPanel/orders |     |            | Returns list of transactions assigned to the supervisor.    | TransactionObject[]| supervisor     |
| GET    | api/supervisorPanel/orders/{id}  | id  |            | Returns TransactionObject with given id. Returns 403 if the transaction isn't assigned to the supervisor.  Returns 404 if transaction doesn't exist.    | TransactionObject[], 403, 404                                                           | supervisor     |
| PUT    | api/supervisorPanel/orders/{id} | UpdateTransactionRequest[] | id | Updates a transaction if found. Otherwise returns 404. | 200, 404 | supervisor|

## Resources

#### [Follow this link to find our way of working with this repo](https://www.atlassian.com/git/tutorials/comparing-workflows/gitflow-workflow)
