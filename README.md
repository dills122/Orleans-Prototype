### Orleans Prototype

My take at building an Enterprise-like Orleans solution using EF Core to manage the state instead of Orleans built in state managment system. 

For more details read the App Description (still in progress) for the details of the app.

### Current working features
- User CRUD Operations
- Create Order/Process Order
- Lookup Order/Events by Username

**This gives a basic working example of all the backend features within the app**

### Setting up the DB on Initial

If it's your first time running the application

- Delete the Migration folder listed in the Orleans-Prototype project
- Run **Add Migration Initial** in the package manager console, Initial can be substituted for any other name
- Finally run **Update-Database** in the package manager console

This will create a local DB for the application
