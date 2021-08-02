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

### Items
| Method | Path           | Body       | Parameters | Description                                                 | Possible responses | Who can access |
|--------|----------------|------------|------------|-------------------------------------------------------------|--------------------|----------------|
| GET    | api/items/{id} | *none* | id       | Returns an item with given id or 404 if item doesn't exist. | ItemObject, 404    | everyone       |
| POST   | api/items?     | *none* | brand, name, category, make, model | Returns a list of items. List can be empty if none is found. | ItemObject[] | everyone |
| POST   | api/items      | CreateItemRequest | *none* | Creates a new item based on the data in the body. Returns 500 error code if data is invalid. | 200, 500 | admin |
| PUT    | api/items/{id} | UpdateItemRequest | id | Updates an item if found. Returns 500 otherwise. | 200, 500 | admin |
| DELETE | api/items/{id} | DeleteItemRequest | id | Updates an item if found. Returns 500 otherwise. | 200, 500 | admin |
