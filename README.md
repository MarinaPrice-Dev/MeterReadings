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
- dotnet ef migrations add InitialCreate
- dotnet ef database update
- run project (to apply migrations and seeds db)

# todo
- automapper for dtos
- update .http file
- tests (unit, integration)
- basic ui for uploads
- test 8081 in docker (or just remove)
- remove date mapping code


# Decisions made

## Primary keys in database
Rather than using AccountId as the primary key, a separate int ID primary key has been added.
We don't know if account Id format will ever change in the future, or if a certain account Id will ever need modifying for a customer. It is a business identifier.

Creating a seperate Id allows a better separation of concerns and allows it to remain decoupled from external systems.

## Database deletions
`OnDelete(DeleteBehavior.Restrict); `

For this application, Accounts cannot be deleted if they have meter readings against them. 

For a production ready app, we should think about whether we want cascade deletes, or if we intend to keep data for archiving purposes, whether soft deletes should be considered, etc.

## Saving meter readings indivudually
Talk about why we don't save as range