# CarParkingTDS

CarParkingTDS is a .NET Take-Home Task project implementing a simple car park management API.

---

## Requirements

1. **SQL Server 2022 Developer**
2. **.NET 9.0 (SDK 9.0.306)**
3. **Visual Studio 2022**

---

## Installation

1. Install **SQL Server 2022 Developer** from [here](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
2. Install **.NET 9.0 (SDK 9.0.306)** from [here](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
3. Install **Visual Studio 2022** from [here](https://visualstudio.microsoft.com/downloads/)

---

## Database Seeding

The SQL scripts are located in: `..\CarParkingTDS\Scripts`

### Steps:

1. **Update the password** in the `InitDB.sql` script from the `..\CarParkingTDS\Scripts` folder before running it. Replace the example password with your own:

   ```sql
   -- Create login
   CREATE LOGIN ParkingSa WITH PASSWORD = 'ExampleStrongPassword123!';
   ```

2. **Update the connection string** in `appsettings.json` to use your actual SQL Server instance name and password:

   ```json
   "ConnectionStrings": {
     "ParkingDatabase": "Server=localhost\\EXAMPLE_SERVER_NAME;Database=TDS_ParkingDB;User Id=ParkingSa;Password=ExampleStrongPassword123!;TrustServerCertificate=True;"
   }
   ```

3. **Run the SQL initialization script** `InitDB.sql` to seed the database.

---

## Running the Application

1. Open the solution in **Visual Studio 2022**.
2. The solution contains two projects: the main API project and the unit test project.
3. Launch the application using **HTTPS**.
4. Once running, you can test the API via **Swagger UI** at:

   > [https://localhost:7031/swagger/index.html](https://localhost:7031/swagger/index.html)

---

## Post-Implementation Questions

1. **Charging rule clarification:** Should the additional £1 charge every 5 minutes be applied at the *start* or *end* of each 5-minute block? For example, if a driver leaves after 4 minutes and 30 seconds, should the extra £1 be charged?

2. **Duplicate registration numbers:** Should the system allow parking two different vehicles with the same registration number?

---

## Notes
- Ensure the SQL Server instance is running before launching the API.
- All scripts and configuration files are provided in the `Scripts` folder and the API project root.
- Swagger UI provides a convenient way to test all endpoints interactively.