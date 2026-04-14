# Mini POS

## Migration commands

Run this in POSSampleOWN\POSSampleOWN.csproj:

Migration script:

```cmd
dotnet ef migrations add InitialCreate --project POSSampleOWN.database --startup-project POSSampleOWN   
```

```cmd
dotnet ef database update
```

Please use quotation when viewing tables in PgAdmin or DataGrip:

```sql
SELECT * FROM "PRODUCTS";
```

## Docker commands

Run this in the root directory (where docker-compose.yml exists):

```cmd
docker compose up -d --build
```

apply database migration (only run this if you are initializing the database) :

```cmd
dotnet ef database update --project POSSampleOWN.database --startup-project POSSampleOWN
```

to run docker:

```cmd
docker compose up
```

to stop docker:

```cmd
docker compose down
```

To check if docker is running:

```cmd
docker compose ps
```
