# HealthERP
a Health Insurance Enterprise Resource Planning (ERP) system. This system is responsible for managing health insurance policies, processing claims, and interacting with Hospital Electronic Health Records (EHRs) for efficient claim processing

# Features
1. Policy Holders can submit a claim
2. Administrators can approve or decline a claim
3. Integration tests with AutoFixture, Nunit

# Getting Started
These instructions will help you get a copy of the project up and running on your local machine for development and testing purposes.

# Prerequisites
1. .NET SDK (version 7)
2. Visual Studio or Visual Studio Code
3. SQlite Database Engine(As Sqlite is a file-based database, hence it will be easier to get it up and running)
4. Db browser for Sqlite (for database explorer)

# Installing

git clone https://github.com/davido776/HealthERP.git

cd <project-folder>

once you clone, you can open with Visual studio and clean, build then run the project. Your browser should open a localhost:51320.
Append "/swagger" to open the swagger documentation. if you are opening with Visual studio code run below commands

run cmd : dotnet restore

run cmd : dotnet run

# Built With

1. .NET : .NET is a free, cross-platform, open-source developer platform for building many different types of applications.
2. SQLite : SQLite is a C-language library that implements a small, fast, self-contained, high-reliability, full-featured, SQL database engine.
