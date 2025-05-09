Use Sql Server Database

Step 1:  Execute the following command in Package Console Manager to migrate EF Core
         
         Add-Migration "Initial Migration" -StartupProject InventoryApp.API -Project InventoryApp.Infrastructure
         
Step 2.  Execute the following command in Package Console Manager to make change in database

         Update-Database -StartupProject InventoryApp.API -Project InventoryApp.Infrastructure
