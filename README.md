## List of contents

- ### [Developing the database](#developing-the-database)
- ### [Endpoints (WIP)](#endpoints-wip)
- ### [Resources](#resources)

## Developing the database

- Connection string can be found in [`appsettings.json`](./src/Api/appsettings.json) file

- To add migration (make sure you start in Infrastructure directory): <br>
  `dotnet ef migrations add <name of new migration> -c ApplicationDbContext -s ./../Api` <br>

- To remove migration (make sure you start in Infrastructure directory): <br>
  `dotnet ef migrations remove -c ApplicationDbContext -s ./../Api`

## Endpoints (WIP)

JWT token must be provided in the header when accessing sensitive data.

### Items

| Method | Path                | Body                                                                                      | Params                                                         | Description                                                     | Responses                                                             | Who can access |
| ------ | ------------------- | ----------------------------------------------------------------------------------------- | -------------------------------------------------------------- | --------------------------------------------------------------- | --------------------------------------------------------------------- | -------------- |
| GET    | api/items/{id}      | _none_                                                                                    | id                                                             | Returns an ItemCard with given id or 404 if item doesn't exist. | [ItemCard](./src/Application/Models/ApiModels/Items/ItemCard.cs), 404 | everyone       |
| GET    | api/items?          | _none_                                                                                    | brand, name, category, make, model, gender, minPrice, maxPrice | Returns a list of items. List can be empty if none is found.    | [ItemObject[]](src/Application/Models/DTOs/ItemObject.cs)             | everyone       |
| GET    | api/items/category? | _none_                                                                                    | category, pageIndex, pageSize                                  | Returns list of items with given category name                  | [ItemObject[]](src/Application/Models/DTOs/ItemObject.cs)             | everyone       |
| GET    | api/items/trending? | _none_                                                                                    | category, count                                                | Returns list of trending items                                  | [ItemCard[]](./src/Application/Models/ApiModels/Items/ItemCard.cs)    | everyone       |
| POST   | api/items           | [CreateItemCommand](src/Application/Models/ApiModels/Items/Commands/CreateItemCommand.cs) | _none_                                                         | Creates a new item based on the data in the body.               | 200, 500                                                              | admin          |
| PUT    | api/items/{id}      | [UpdateItemCommand](src/Application/Models/ApiModels/Items/Commands/UpdateItemCommand.cs) | id                                                             | Updates an item if found. Returns 404 otherwise.                | 200, 404                                                              | admin          |
| DELETE | api/items/{id}      | _none_                                                                                    | id                                                             | Deletes an item if found. Returns 404 otherwise.                | 200, 404                                                              | admin          |

### UserSettings

| Method | Path           | Body   | Params | Description                          | Responses                                                               | Who can access  |
| ------ | -------------- | ------ | ------ | ------------------------------------ | ----------------------------------------------------------------------- | --------------- |
| GET    | api/user       | _none_ | _none_ | Returns user data.                   | [UserSettingsObject](src/Application/Models/DTOs/UserSettingsObject.cs) | Signed in users |
| GET    | api/user/level | _none_ | _none_ | Returns UserSellerLevel as a string. | string                                                                  | Signed in users |
| PUT    | api/user       | _none_ | _none_ | Returns user data.                   | 200                                                                     | Signed in users |

### Asks

| Method | Path          | Body                                                                                   | Params                                                              | Description                                                             | Responses                                                  | Who can access  |
| ------ | ------------- | -------------------------------------------------------------------------------------- | ------------------------------------------------------------------- | ----------------------------------------------------------------------- | ---------------------------------------------------------- | --------------- |
| GET    | api/asks/{id} | _none_                                                                                 | id                                                                  | Returns ask details or 404 it ask doesn't exist.                        | [AskObject](src/Application/Models/DTOs/AskObject.cs), 404 | Signed in users |
| GET    | api/asks/     | _none_                                                                                 | [SearchAsksQuery](src/Application/Common/Models/SearchAsksQuery.cs) | Returns list of asks for a user. List can be empty if user has no asks. | [AskObject[]](src/Application/Models/DTOs/AskObject.cs)    | Signed in users |
| POST   | api/asks/     | [CreateAskCommand](src/Application/Models/ApiModels/Asks/Commands/CreateAskCommand.cs) | _none_                                                              | Creates a new ask.                                                      | 200                                                        | Signed in users |
| PUT    | api/asks/{id} | [UpdateAskCommand](src/Application/Models/ApiModels/Asks/Commands/UpdateAskCommand.cs) | id                                                                  | Updates an ask if found. Returns 404 otherwise.                         | 200, 404                                                   | Ask owner       |
| DELETE | api/asks/{id} | _none_                                                                                 | id                                                                  | Deletes an ask if found. Returns 404 otherwise.                         | 200, 404                                                   | Ask owner       |

### Bids

| Method | Path          | Body                                                                                   | Parameters                                                          | Description                                                             | Responses                                                  | Who can access  |
| ------ | ------------- | -------------------------------------------------------------------------------------- | ------------------------------------------------------------------- | ----------------------------------------------------------------------- | ---------------------------------------------------------- | --------------- |
| GET    | api/bids/{id} | _none_                                                                                 | id                                                                  | Returns bid details or 404 it ask doesn't exist.                        | [BidObject](src/Application/Models/DTOs/BidObject.cs), 404 | Signed in users |
| GET    | api/bids/     | _none_                                                                                 | [SearchBidsquery](src/Application/Common/Models/SearchBidsQuery.cs) | Returns list of bids for a user. List can be empty if user has no bids. | [BidObject[]](src/Application/Models/DTOs/BidObject.cs)    | Signed in users |
| POST   | api/bids/     | [CreateBidCommand](src/Application/Models/ApiModels/Bids/Commands/CreateBidCommand.cs) | _none_                                                              | Creates a new bid.                                                      | 200                                                        | Signed in users |
| PUT    | api/bids/{id} | [UpdateBidCommand](src/Application/Models/ApiModels/Bids/Commands/UpdateBidCommand.cs) | id                                                                  | Updates a bid if found. Returns 404 otherwise.                          | 200, 404                                                   | Bid owner       |
| DELETE | api/bids/{id} | _none_                                                                                 | id                                                                  | Deletes a bid if found. Returns 404 otherwise.                          | 200, 404                                                   | Bid owner       |

### Transactions

| Method | Path                  | Body   | Params | Description                                                                                                                                        | Responses                                                                       | Who can access   |
| ------ | --------------------- | ------ | ------ | -------------------------------------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------- | ---------------- |
| GET    | api/transactions/{id} | _none_ | id     | Returns TransactionObject if transaction with given id exists. Otherwise, returns 404. Code 403 is returned when user doesn't own the transaction. | [TransactionObject](src/Application/Models/DTOs/TransactionObject.cs), 404, 403 | Buyer and seller |
| GET    | api/transactions/     | _none_ | _none_ | Returns list of user transactions.                                                                                                                 | [TransactionObject](src/Application/Models/DTOs/TransactionObject.cs)[]         | Signed in user   |

### AdminPanel

| Method | Path                       | Body                                                                                                             | Params | Description                                            | Responses                                                               | Who can access |
| ------ | -------------------------- | ---------------------------------------------------------------------------------------------------------------- | ------ | ------------------------------------------------------ | ----------------------------------------------------------------------- | -------------- |
| GET    | api/adminPanel/orders      | _none_                                                                                                           | _none_ | Returns list of transactions.                          | [TransactionObject](src/Application/Models/DTOs/TransactionObject.cs)[] | admin          |
| PUT    | api/adminPanel/orders/{id} | [UpdateTransactionCommand](src/Application/Models/ApiModels/Transactions/Commands/UpdateTransactionCommand.cs)[] | id     | Updates a transaction if found. Returns 404 otherwise. | 200, 404                                                                | admin          |

### SupervisorPanel

| Method | Path                            | Body                                                                                                           | Params | Description                                                                                                                                         | Responses                                                                         | Who can access |
| ------ | ------------------------------- | -------------------------------------------------------------------------------------------------------------- | ------ | --------------------------------------------------------------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------- | -------------- |
| GET    | api/supervisorPanel/orders      | _none_                                                                                                         |        | Returns list of transactions assigned to the supervisor.                                                                                            | [TransactionObject](src/Application/Models/DTOs/TransactionObject.cs)[]           | supervisor     |
| GET    | api/supervisorPanel/orders/{id} | _none_                                                                                                         | id     | Returns TransactionObject with given id. Returns 403 if the transaction isn't assigned to the supervisor. Returns 404 if transaction doesn't exist. | [TransactionObject](src/Application/Models/DTOs/TransactionObject.cs)[], 403, 404 | supervisor     |
| PUT    | api/supervisorPanel/orders/{id} | [UpdateTransactionCommand](src/Application/Models/ApiModels/Transactions/Commands/UpdateTransactionCommand.cs) | id     | Updates a transaction if found. Returns 404 otherwise.                                                                                              | 200, 404                                                                          | supervisor     |

### Identity

| Method | Path                       | Body                                                                                    | Params | Description           | Responses                                                                                                                                                                  | Who can access |
| ------ | -------------------------- | --------------------------------------------------------------------------------------- | ------ | --------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | -------------- |
| POST   | api/identity/register      | [UserRegistrationRequest](src/Application/Identity/Requests/UserRegistrationRequest.cs) | _none_ | Creates a new user.   | [AuthFailedResponse](src/Application/Identity/Responses/AuthFailedResponse.cs), [AuthSuccessResponse](src/Application/Identity/Responses/AuthSuccessResponse.cs), 400, 200 | everyone       |
| POST   | api/identity/login         | [UserLoginRequest](src/Application/Identity/Requests/UserLoginRequest.cs)               | _none_ | Returns JWT token.    | [AuthFailedResponse](src/Application/Identity/Responses/AuthFailedResponse.cs), [AuthSuccessResponse](src/Application/Identity/Responses/AuthSuccessResponse.cs), 400, 200 | everyone       |
| POST   | api/identity/resetPassword | [ResetPasswordRequest](src/Application/Identity/Requests/ResetPasswordRequest.cs)       | _none_ | Resets user password. | [ResetPasswordResponse](src/Application/Identity/Responses/ResetPasswordResponse.cs), 400, 200                                                                             | everyone       |

## Resources

#### [Follow this link to find our way of working with this repo](https://www.atlassian.com/git/tutorials/comparing-workflows/gitflow-workflow)
