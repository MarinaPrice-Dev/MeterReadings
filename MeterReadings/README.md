# Setup
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


# Starting the project
## Docker
Run `docker compose up -d`

### UI url
Go to: http://localhost:5173/

### Swagger Api Documentation

Available here:
http://localhost:8080/swagger



# Discussions points

## Small woopsie!
The powerpoint wanted the POST endpoint to be in format `meter-reading-uploads`
I didn't notice until the very end, I used `MeterReadingUpload`. Not much point in changing it this late, but its a simple change

## Large scale system issues
The data in this task is pretty miniscule. In a real world scenario, how much data is being inserted and how frequently?
We may need to consider the following situations for scaling and speed issues:
- Message queues
- Caching (in memory or something like redis)
- Database indexing

## ToDos for production ready code
- Thorough tests including unit, integration, e2e
- Logging
- MeterReadingValidator contains some datetime parsing. Can refactor to IDateTimeParser for reusability
- A mapper for conversions between DTOs to entities
- Auditing for database row insertions
- automapping between DTOs and entities would be nice
- ValidateReading takes in 4 params, would be better having a request object

## Primary keys in database
Rather than using AccountId as the primary key, a separate int ID primary key has been added.
We don't know if account Id format will ever change in the future, or if a certain account Id will ever need modifying for a customer. It is a business identifier.

Creating a separate Id allows a better separation of concerns and allows it to remain decoupled from external systems.

## Database deletions
For a production ready app, consider: 
- cascade deletes
- soft deletes
- archiving data for deleted accounts

## Culture set to en-gb
This is to allow correct parsing of dates regardless of environment (including when running through docker)

## Known bugs
- docker keeps trying to recreate the database when it already exists 


## Ignore
#### Ignore this section, its just helpful commands for me
migrations:
dotnet ef migrations add InitialCreate

docker help:

docker-compose down -v  
docker-compose up --build

sql server:
TRUNCATE TABLE [MeterReadingsDb].[dbo].[MeterReadings]

