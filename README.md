[Follow this link to find our wayf of working with this repo](https://www.atlassian.com/git/tutorials/comparing-workflows/gitflow-workflow)

### Working with database

- Connection string can be found in [`appsettings.json`](./src/Api/appsettings.json) file

- To add migration (make sure you start in Infrastructure directory): <br>
``dotnet ef migrations add <name of new migration> -c ApplicationDbContext -s ./../Api`` <br> 

- To remove migration (make sure you start in Infrastructure directory): <br>
`dotnet ef migrations remove -c ApplicationDbContext -s ./../Api` 