# BinaryQuest.TodoManager
Its a web application for managing todo.

## Technologies
```
     API:  .NET 6
Database:  MS SQL Server
Frontend:  Vue.js 2
```

## Features
```
1. Create todo
2. In place update
3. Set deadline
4. Delete todo
5. Search your todos
```

## How to run 
0. **Change connection string in both projects below**

1. Apply migrations for database to create. Apply one by one
```
     Update-Database -Context UserDbContext
     Update-Database -Context TodosDbContext
     Update-Database -Context NotificationContext
```

2. Open directory BinaryQuest.TodoManager.Api /TodoManager.Api in cmd and run (**api**)
```
dotnet run
```
3. Open directory BinaryQuest.TodoManager.Api /TodoManager.BackgroundWorker in cmd and run (**background service**)
```
dotnet run
```

After running these commands the backend should be up and running. Place the URL in browser https://localhost:7288/swagger/index.html
You will see the api documentation with **swagger** ui

4. To run the frontend app follow the repository readme.md file of the frontend app: https://github.com/gitcseme/todomanager.client

## API
* The api is written with **.Net 6**
* Followed generic **repository** & **unit of work** pattern
* Write a **middleware** to handle exceptions globaly
* Used **NLog** for logging
* Api documentation with swagger

### Background service
* A worker service is used which checks if there are any todo with deadline over.
* If it finds any; it creates a notification.
* It does the check in every 5 seconds.

### Log file location
BinaryQuest.TodoManager.Api/TodoManager.Api/bin/Debug/net6.0/logfile.txt
