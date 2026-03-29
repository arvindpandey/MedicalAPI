# Setup Instructions

## 1. Clone the Repository

```
git clone https://github.com/arvindpandey/MedicalAPI.git
cd MedicalAPI
```

## 2. Setup Database

* Open SQL Server
* Create a new database (e.g., `MedicalDB`)
* Run the script:

```
database/MedicalDBScripts.sql
```

## 3. Update Connection String

* Go to `appsettings.json`
* Update:

```
"ConnectionStrings": {
  "DefaultConnection": "Your_SQL_Server_Connection_String"
}
```

## 4. Run Backend

```
dotnet run
```

## 5. Run Frontend

* Navigate to frontend folder

```
npm install
npm start
```

## Notes

* Make sure SQL Server is running
* Ensure correct DB credentials
* Backend should run before frontend
