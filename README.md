# Spirit Quest

#### _Asp.Net MVC Application, March, 2017_

#### By _**Erik Killops**_

Visit live version here: http://spiritquest.azurewebsites.net/

## Description

_Cocktail recipe recommendations and search using ADDB api._

## Setup/Installation Requirements

_Requires .Net Framework and Visual Studio_

1. Clone repository.
2. Open project in Visual Studio
3. Restore dependencies.
4. Visit the ADDb signup page and create an account to obtain an API key: https://addb.absolutdrinks.com/docs/signup/
5. In the 'Models' folder create a class called EnvironmentVariables.cs and add this code with your API key:
    
    ```c#
     namespace CocktailRecipeLookup.Models
     {
         public static class EnvironmentVariables
         {
             public static readonly string ADDbApiKey = "YOUR_KEY_HERE";
         }
     }
     ```
     
6. In Visual Studio run program by hitting 'Play' or 'F5'

## Technologies Used

C#, .NetCore, Asp.Net MVC

### License

*MIT*

Copyright (c) 2017 **_Erik Killops_**
