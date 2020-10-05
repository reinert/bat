## Install pre-requisites
- Docker and docker-compose



# Quick start
If you just want to setup the project quickly, run the tests and see the results do the following:

```bash
./setup-dev
docker-compose up
```

After both `bat_db_1` and `bat_server_1` containers are built and running, open another terminal and run the tests in server container with the command:

```bash
docker exec bat_server_1 sh -c "./run-tests.sh"
```

Enjoy.



# Development instructions
Following you have detailed information of development tools and how to write and run tests.

## Starting development environment
Before you get docker up **FOR THE FIRST TIME ONLY** you must run dev setup script:

```bash
./setup-dev
```

Now you can properly get up docker enviroment with:

```bash
docker-compose up
```

When you start docker, you will have two services running: a Postgresql container a Server container.
* The Postgresql container runs under the service named **"db"**. It only provides a pgsql database and is able to receive connections from inner services.
* The Server container runs under the service named **"server"**. It supports dotnet development with all requirements necessary to run the server project in F#.
For detailed information, please check *docker-compose.yml*.

In order to start developing you need to enter the "server" container, with the following command:

```bash
docker exec -it bat_server_1 bash
```


Now you're inside the "server" container where you can use dotnet tools to build and test the server project.


## Managing DBs with EF Core

This application leverages database communication via Entity Framework Core ORM and uses its Migrations feature to evolve database schema.

So, before running tests for the first time you need to setup database schema using "ef" tool.

It happens that this app supports two different databases at the same time: Sqlite and PostgreSql.

So, whenever you execute any *ef* migration command, you need to specify in which DB Context you want the migration to be executed.

Therefore, in order to setup both databases **FOR THE FIRST TIME**, please run the following commands:

```bash
dotnet ef --project src/Server/ database update --context SqliteContext -v
dotnet ef --project src/Server/ database update --context PgsqlContext -v
```

* The `--project` option is necessary because this project is divided into three subprojects (Shared, Server and Client). So you need to in which project the *ef* command should be executed. Note this option is not necessary if you're in 'src/Server' directory.
* The `database update` command tells *ef* to update database schema to the last migration.
* The `--context` option tells *ef* to use a specific DbContext. Put "SqliteContext" to run in Sqlite database, or "PgsqlContext" to run in PosgreSql database.
* The `-v` option is just verbose. It helps you identify erros when they show up.


Anytime you modify your domain models, you need to generate a new migration and execute it to adapt DB schema. In order to **create new migrations** please enter src/Server directory and run the following command (**supposing you're located at 'src/Server' directory**):

```bash
dotnet ef migrations add <MigrationName> --context SqliteContext --output-dir Migrations/Sqlite -v
dotnet ef migrations add <MigrationName> --context PgsqlContext --output-dir Migrations/Pgsql -v
```

* Notice there's no `--project src/Server` option because you should already be at this directory.
* Substitute <MigrationName> by any name you want that reminds you what this migration does.
* The `--output-dir` option specifies where the generated migration codes should be created. Please, pay attention that this is relative to the src/Server directory where you are located now.


After creating Migration codes, **you need to manually add them to your `Server.fsproj`**, like this:

```xml
<ItemGroup>
    ...
    <Compile Include="Migrations/Sqlite/20201002151314_MyNewMigration.fs" />
    ...
    <Compile Include="Migrations/Pgsql/20201002180025_MyNewMigration.fs" />
    ...
</ItemGroup>
```

Then, you **run the migrations to update your DB schema** with the already shown command:

```bash
dotnet ef database update --context SqliteContext -v
dotnet ef database update --context PgsqlContext -v
```


## Writing and Running Tests

Tests are located at 'tests/server'. Feel comfortable to write new test cases.

In order to run the tests and try your data access code, please enter the 'tests/server' directory and run (**supposing you're located at 'tests/Server' directory**):

```bash
dotnet run
```

The tests results should appear in your console.


## Acessing PG database

If you need to access the PG database instance used for development, run the following command from `bat_server_1` container:

```bash
psql -h db -U bat
```

When prompted for **password**, just type `bat`.
