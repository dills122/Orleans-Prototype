# Orleans Bank Co.

This is an example app that will leverage Microsoft Orleans to build a Enterprise-like application.

This isn't a production ready product, but rather just a blueprint of what one might look like in the .NET ecosystem.

For more details read the App Description (still in progress) for the details of the app.

**This gives a basic overview of all the backend features within the app**

## Planned Features

* Create users
* Users create accounts
* Accounts execute transactions
* Users generate account reports

### Setting up the DB on Initial

If it's your first time running the application

> Target the Repo Layer within the package manager console for these commands

- Run **Add Migration Initial** in the package manager console, Initial can be substituted for any other name
- Finally run **Update-Database** in the package manager console

This will create a local DB for the application

**If you get an error migrating, delete the migration folder and try it again**
