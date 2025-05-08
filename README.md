# MeterReadings

## Setup
### .env file
You will need to create an .env in the project root directory. It needs a secure database password:
`SA_PASSWORD=[your password]`

Replace **[your password]**

### User secrets
Setup your database connection string inside user secrets:
`{
    "ConnectionStrings": {
        "DefaultConnection": "Server=localhost,1433;Database=MeterReadingsDb;User Id=sa;Password=[your password];TrustServerCertificate=True;"
    }
}`

Replace **[your password]** with the same password as above

## Starting the project
### Docker
Run `docker compose up -d`


## Swagger Api Documentation

Available here:
http://localhost:5272/swagger


## Add migrations
dotnet ef migrations add InitialCreate

dotnet ef database update

# todo
- automapper for dtos
- update .http file
- tests (unit, integration)
- basic ui for uploads