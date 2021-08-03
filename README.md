[Follow this link to find our way of working with this repo](https://www.atlassian.com/git/tutorials/comparing-workflows/gitflow-workflow)

## List of contents
* [Developing the database](#developing-the-database)
* [Endpoints (WIP)](#endpoints-wip)

## Developing the database

- Connection string can be found in [`appsettings.json`](./src/Api/appsettings.json) file

- To add migration (make sure you start in Infrastructure directory): <br>
``dotnet ef migrations add <name of new migration> -c ApplicationDbContext -s ./../Api`` <br> 

- To remove migration (make sure you start in Infrastructure directory): <br>
`dotnet ef migrations remove -c ApplicationDbContext -s ./../Api` 

## Endpoints (WIP)

JWT token must be provided in the header when accessing sensitive data.

### Items
| Method | Path           | Body       | Parameters | Description                                                 | Possible responses | Who can access |
|--------|----------------|------------|------------|-------------------------------------------------------------|--------------------|----------------|
| GET    | api/items/{id} | *none*     | id         | Returns an item with given id or 404 if item doesn't exist. | ItemObject, 404    | everyone       |
| POST   | api/items?     | *none*     | brand, name, category, make, model | Returns a list of items. List can be empty if none is found. | ItemObject[] | everyone |
| POST   | api/items      | CreateItemRequest | *none* | Creates a new item based on the data in the body. Returns 500 error code if data is invalid. | 200, 500 | admin |
| PUT    | api/items/{id} | UpdateItemRequest | id | Updates an item if found. Returns 500 otherwise. | 200, 500 | admin |
| DELETE | api/items/{id} | DeleteItemRequest | id | Updates an item if found. Returns 500 otherwise. | 200, 500 | admin |

### UserSettings
| Method | Path           | Body       | Parameters | Description                                                 | Possible responses | Who can access |
|--------|----------------|------------|------------|-------------------------------------------------------------|--------------------|----------------|
| GET    | api/user       | *none*     | *none*     | Returns user data.                                          | UserSettingsObject | Signed in users |
| PUT    | api/user       | *none*     | *none*     | Returns user data.                                          | 200                | Signed in users |

### Asks
| Method | Path           | Body       | Parameters | Description                                                 | Possible responses | Who can access |
|--------|----------------|------------|------------|-------------------------------------------------------------|--------------------|----------------|
| GET    | api/asks/{id}  | *none*     | id         | Returns ask details or 404 it ask doesn't exist.            | AskObject, 404     | Signed in users |
| GET    | api/asks/      | *none*     | *none*     | Returns list of asks for a user. List can be empty if user has no asks | AskObject[] | Signed in users   |
| POST   | api/asks/      | CreateAskRequest | *none* | Creates a new ask.                                        | 200                | Signed in users |
| PUT    | api/asks/{id}  | UpdateAskRequest | id   | Updates an ask if found. Returns 500 otherwise.             | 200, 500           | Ask owner       |
| DELETE | api/asks/{id}  | DeleteAskRequest | id   | Deletes an ask if found. Returns 500 otherwise.             | 200, 500           | Ask owner       |

### Bids
| Method | Path           | Body       | Parameters | Description                                                 | Possible responses | Who can access |
|--------|----------------|------------|------------|-------------------------------------------------------------|--------------------|----------------|
| GET    | api/bids/{id}  | *none*     | id         | Returns bid details or 404 it ask doesn't exist.            | BidObject, 404     | Signed in users |
| GET    | api/bids/      | *none*     | *none*     | Returns list of bids for a useer. List can be empty if user has no asks | BidObject[] | Signed in users   |
| POST   | api/bids/      | CreateBidRequest | *none* | Creates a new bid.                                        | 200                | Signed in users |
| PUT    | api/bids/{id}  | UpdateBidRequest | id   | Updates an bid if found. Returns 500 otherwise.             | 200, 500           | Bid owner       |
| DELETE | api/bids/{id}  | DeleteBidRequest | id   | Deletes an bid if found. Returns 500 otherwise.             | 200, 500           | Bid owner       |

### Transactions
| Method | Path           | Body       | Parameters | Description                                                 | Possible responses | Who can access |
|--------|----------------|------------|------------|-------------------------------------------------------------|--------------------|----------------|
| GET    | api/transactions/{id} | *none* | id |    | Returns TransactionObject if transaction with given id. Otherwise, returns 500. Code 403 is returned when user doesn't own the transaction| TransactionObject[], 500, 403 | Buyer and seller
| GET   | api/transactions/ | *none*   | *none*     | Returns list of user transactions.                          | TransactionObject[] | Signed in user |

### Category
| Method | Path           | Body       | Parameters | Description                                                 | Possible responses | Who can access |
|--------|----------------|------------|------------|-------------------------------------------------------------|--------------------|----------------|
| GET    | api/category/{categoryName}  | *none*     | categoryName    | Returns list of items with given category name | ItemObject[] | everyone       |


TODO: admin panel, supervisor panel



