# Roomie — Teacher-Style Split (API + Model, .NET 8)

This mirrors your lecturer's Lecture 4 pattern: Solution → API → Model. It uses BaseRepository with ADO.NET (Npgsql), the connection string key "AppProgDb", and HTTPS redirection removed.

## 0) Prereqs
- .NET 8 SDK
- PostgreSQL + pgAdmin

## 1) Create the solution and API (exactly like L04)
Open the folder in VS Code, then run:
```
dotnet new sln --name RoomieSystem
dotnet new webapi -n RoomieSystem.API --use-controllers
dotnet sln add RoomieSystem.API
```
Open `RoomieSystem.API/Program.cs` and remove this line:
```
// app.UseHttpsRedirection();
```
Then build and run:
```
dotnet build
dotnet run --project RoomieSystem.API
```

## 2) Create the database (pgAdmin)
Create DB `roomie-db`, open Query Tool, and run the SQL file:
- roomie_schema.sql

## 3) Add a Model project
```
dotnet new classlib -n RoomieSystem.Model
dotnet sln add RoomieSystem.Model
dotnet add RoomieSystem.Model package Npgsql
dotnet add RoomieSystem.Model package Microsoft.Extensions.Configuration
```
In `RoomieSystem.Model.csproj`, set:
```
<Nullable>disable</Nullable>
```

## 4) Prepare Model (Entities + Repositories)
Place these into RoomieSystem.Model:
- Entities/Room.cs
- Repositories/BaseRepository.cs
- Repositories/RoomRepository.cs

## 5) Connection string in API
Edit `RoomieSystem.API/appsettings.json` and set the "AppProgDb" value for your Postgres.

## 6) Wire API to Model
```
dotnet add RoomieSystem.API/RoomieSystem.API.csproj reference RoomieSystem.Model/RoomieSystem.Model.csproj
```
In `Program.cs`:
```
using RoomieSystem.Model.Repositories;
builder.Services.AddScoped<RoomRepository, RoomRepository>();
```

## 7) Controller
Ensure `RoomController.cs` is in `RoomieSystem.API/Controllers/`.

## 8) Run and test
```
export ASPNETCORE_ENVIRONMENT=Development
dotnet run --project RoomieSystem.API/RoomieSystem.API.csproj
```
- Open Swagger at `/swagger`.
- Or import the Postman collection: Roomie_TeacherStyle.postman_collection.json