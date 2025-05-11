# MeterReadings

# Setup
## Backend
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

## Frontend
cd .\meterreadings-ui\\\
npm ci\
npm run dev\
Go to: ` http://localhost:5173/`

### Creating prod ready UI !todo!
npm run build\


# Starting the project
## Docker
Run `docker compose up -d`


# Swagger Api Documentation

Available here:
http://localhost:8080/swagger



# Decisions made

## Questions about spec
- Are we comparing the latest entries against the database data, the csv or both?

## Large scale system issues
The data in this task is pretty miniscule. In a real world scenario, how much data is being inserted and how frequently?
We may need to consider the following situations for scaling and speed issues:
- Message queues
- Caching (in memory or something like redis)
- Database indexing

## Saving meter readings
Save individually or as a range? Doing it as a range would break the current validation method.

## Primary keys in database
Rather than using AccountId as the primary key, a separate int ID primary key has been added.
We don't know if account Id format will ever change in the future, or if a certain account Id will ever need modifying for a customer. It is a business identifier.

Creating a seperate Id allows a better separation of concerns and allows it to remain decoupled from external systems.

## Database deletions
`OnDelete(DeleteBehavior.Restrict); ` \
For this application, Accounts cannot be deleted if they have meter readings against them. \
For a production ready app, we should think about whether we want cascade deletes, or if we intend to keep data for archiving purposes, whether soft deletes should be considered, etc. \


# todo

## Add migrations
dotnet ef migrations add InitialCreate\
dotnet ef database update\
run project (to apply migrations and seeds db)\

# todo
- automapper for dtos
- tests (unit, integration)
- basic ui for uploads
- test 8081 in docker (or just remove)
- remove date mapping code
- add ui to docker