# Orleans Prototype 

Built using Microsoft Orleans for the middle tier where all business logic and state manipulation will take place. Within this prototype, it does not leverage Orleans built in stateful system, instead it uses a custom state system using EF Core as the DAL. 

## Layers
 1. Client
 2. Business Logic
 3. DAL/Repositories
 4. DataObjects


 ### Client

 Client end is the front facing, user interaction based area where all data manipulation will begin. This layer within the project is built using .NET Core MVC Web App.


 ### Business Logic

 The Buisness Logic Layer is where the incoming user input from the Client Layer comes to be processed. This layer is where Orleans is being used as all user input must go through a grain instance to perform any Business Logic or State Manipulation. 

 ### DAL/Repository Layer

 The Data Access Layer is where the Business Layer sends all of it state manipluation to be processed through persistent storage. This layer uses the Repository Pattern that has extensive usage within enterprise grade .NET solutions. 

 Entity Framework Core already natively implements a Repository Pattern underneath, which can seem redundent to build another over top. However, this pattern also adds a nice level of data abstraction, along with better maintainability. 
 
### *Still a work in progress*
