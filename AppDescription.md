# Orleans Prototype 

Built using Microsoft Orleans for the middle tier where all business logic and state manipulation will take place. Within this prototype, it does not leverage Orleans built in stateful system, instead it uses a custom state system using EF Core as the DAL. 

## Resources
 * [Microsoft Orleans](https://dotnet.github.io/orleans/ "Orleans Homepage")
 * [DataFloxEx](https://github.com/gridsum/DataflowEx "DataflowEx Github")
  * [TPL Dataflow](https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/dataflow-task-parallel-library "TPL Dataflow")

## Layers
 1. Client
 2. Business Logic
 3. DAL/Repositories
 4. DataObjects
 5. Pipeline Allocation Service


 ### Client

 Client end is the front facing, user interaction based area where all data manipulation will begin. This layer within the project is built using .NET Core MVC Web App.


 ### Business Logic

 The Buisness Logic Layer is where the incoming user input from the Client Layer comes to be processed. This layer is where Orleans is being used as all user input must go through a grain instance to perform any Business Logic or State Manipulation. 

 ### DAL/Repository Layer

 The Data Access Layer is where the Business Layer sends all of it state manipluation to be processed through persistent storage. This layer uses the Repository Pattern that has extensive usage within enterprise grade .NET solutions. 

 Entity Framework Core already natively implements a Repository Pattern underneath, which can seem redundent to build another over top. However, this pattern also adds a nice level of data abstraction, along with better maintainability.


 ### DataObjects

 The layer where all of the models for EF Core are placed, along with all custom Exceptions.

 ### PipelineService

 This service is a service for allocating processing pipelines. This is a generic service, which allows for the addition of many different types of pipelines that can be used for other types of data processing. These pipelines are meant as a one time use, then are expected to be disposed. 
 
### *Still a work in progress*
