using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaynMaker.Entities.Persistancy
{
    class DatabaseMigrations
    {
        public const int RequiredDatabaseVersion = 12;

        public DatabaseMigrations()
        {
            Migrations = new Dictionary<int, Action<AssetsContext>>();

            Migrations.Add( 1, ToVersion1 );
            Migrations.Add( 2, ToVersion2 );
            Migrations.Add( 3, ToVersion3 );
            Migrations.Add( 4, ToVersion4 );
            Migrations.Add( 5, ToVersion5 );
            Migrations.Add( 6, ToVersion6 );
            Migrations.Add( 7, ToVersion7 );
            Migrations.Add( 8, ToVersion8 );
            Migrations.Add( 9, ToVersion9 );
            Migrations.Add( 10, ToVersion10 );
            Migrations.Add( 11, ToVersion11 );
            Migrations.Add( 12, ToVersion12 );
        }

        public Dictionary<int, Action<AssetsContext>> Migrations { get; set; }

        private void ToVersion1( AssetsContext ctx )
        {
            ctx.Database.ExecuteSqlCommand( @"
CREATE TABLE SchemaInfoes (
    Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    Version INTEGER NOT NULL
)" );

            ctx.Database.ExecuteSqlCommand( @"
CREATE TABLE Companies (
    Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    Name TEXT NOT NULL
)" );

            ctx.Database.ExecuteSqlCommand( @"
CREATE TABLE Stocks (
    Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, 
    Isin TEXT NOT NULL,
    Company_id INTEGER NOT NULL,
    FOREIGN KEY(Company_id) REFERENCES Companies(Id)
)" );
        }

        private void ToVersion2( AssetsContext ctx )
        {
            ctx.Database.ExecuteSqlCommand( @"
CREATE TABLE AnalysisTemplates (
    Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    Name TEXT NOT NULL,
    Template TEXT NOT NULL
)" );
        }

        private void ToVersion3( AssetsContext ctx )
        {
            ctx.Database.ExecuteSqlCommand( @"
CREATE TABLE Currencies (
    Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    Name TEXT NOT NULL
)" );

            ctx.Database.ExecuteSqlCommand( @"
CREATE TABLE Translations (
    Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, 
    Rate DOUBLE NOT NULL,
    Timestamp DATETIME NOT NULL,
    Currency_id INTEGER NOT NULL,
    FOREIGN KEY(Currency_id) REFERENCES Currencies(Id)
)" );
        }

        private void ToVersion4( AssetsContext ctx )
        {
            ctx.Database.ExecuteSqlCommand( @"DROP TABLE Translations" );

            ctx.Database.ExecuteSqlCommand( @"
CREATE TABLE Translations (
    Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, 
    Rate DOUBLE NOT NULL,
    Timestamp DATETIME  NOT NULL,
    SourceId INTEGER NOT NULL,
    TargetId INTEGER NOT NULL,
    FOREIGN KEY(SourceId) REFERENCES Currencies(Id),
    FOREIGN KEY(TargetId) REFERENCES Currencies(Id)
)" );
        }

        private void ToVersion5( AssetsContext ctx )
        {
            ctx.Database.ExecuteSqlCommand( @"DROP TABLE Companies" );

            ctx.Database.ExecuteSqlCommand( @"
CREATE TABLE Companies (
    Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    Name TEXT NOT NULL,
    Homepage TEXT NULL,
    Sector TEXT NULL,
    Country TEXT NULL
)" );

            ctx.Database.ExecuteSqlCommand( @"
CREATE TABLE 'References' (
    Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, 
    Url TEXT NOT NULL,
    Company_id INTEGER NOT NULL,
    FOREIGN KEY(Company_id) REFERENCES Companies(Id)
)" );
        }

        private void ToVersion6( AssetsContext ctx )
        {
            ctx.Database.ExecuteSqlCommand( @"DROP TABLE Companies" );

            ctx.Database.ExecuteSqlCommand( @"
CREATE TABLE Companies (
    Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    Name TEXT NOT NULL,
    Homepage TEXT NULL,
    Sector TEXT NULL,
    Country TEXT NULL,
    XdbPath TEXT NULL
)" );
        }

        private void ToVersion7( AssetsContext ctx )
        {
            ctx.Database.ExecuteSqlCommand( @"
CREATE TABLE Prices (
    Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    Value DOUBLE NOT NULL,
    Source TEXT NOT NULL,
    RawPeriod TEXT NOT NULL,
    Timestamp DATETIME NOT NULL,
    Currency_Id INTEGER NOT NULL,
    Stock_Id INTEGER NOT NULL,
    FOREIGN KEY(Currency_Id) REFERENCES Currencies(Id)
    FOREIGN KEY(Stock_Id) REFERENCES Stocks(Id)
)" );
        }

        private void ToVersion8( AssetsContext ctx )
        {
            ctx.Database.ExecuteSqlCommand( @"
CREATE TABLE Assets (
    Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    Value DOUBLE NOT NULL,
    Source TEXT NOT NULL,
    RawPeriod TEXT NOT NULL,
    Timestamp DATETIME NOT NULL,
    Currency_Id INTEGER NOT NULL,
    Company_Id INTEGER NOT NULL,
    FOREIGN KEY(Currency_Id) REFERENCES Currencies(Id)
    FOREIGN KEY(Company_Id) REFERENCES Companies(Id)
)" );
        }

        private void ToVersion9( AssetsContext ctx )
        {
            ctx.Database.ExecuteSqlCommand( @"
CREATE TABLE Debts (
    Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    Value DOUBLE NOT NULL,
    Source TEXT NOT NULL,
    RawPeriod TEXT NOT NULL,
    Timestamp DATETIME NOT NULL,
    Currency_Id INTEGER NOT NULL,
    Company_Id INTEGER NOT NULL,
    FOREIGN KEY(Currency_Id) REFERENCES Currencies(Id)
    FOREIGN KEY(Company_Id) REFERENCES Companies(Id)
)" );

            ctx.Database.ExecuteSqlCommand( @"
CREATE TABLE Dividends (
    Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    Value DOUBLE NOT NULL,
    Source TEXT NOT NULL,
    RawPeriod TEXT NOT NULL,
    Timestamp DATETIME NOT NULL,
    Currency_Id INTEGER NOT NULL,
    Company_Id INTEGER NOT NULL,
    FOREIGN KEY(Currency_Id) REFERENCES Currencies(Id)
    FOREIGN KEY(Company_Id) REFERENCES Companies(Id)
)" );

            ctx.Database.ExecuteSqlCommand( @"
CREATE TABLE EBITs (
    Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    Value DOUBLE NOT NULL,
    Source TEXT NOT NULL,
    RawPeriod TEXT NOT NULL,
    Timestamp DATETIME NOT NULL,
    Currency_Id INTEGER NOT NULL,
    Company_Id INTEGER NOT NULL,
    FOREIGN KEY(Currency_Id) REFERENCES Currencies(Id)
    FOREIGN KEY(Company_Id) REFERENCES Companies(Id)
)" );

            ctx.Database.ExecuteSqlCommand( @"
CREATE TABLE Equities (
    Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    Value DOUBLE NOT NULL,
    Source TEXT NOT NULL,
    RawPeriod TEXT NOT NULL,
    Timestamp DATETIME NOT NULL,
    Currency_Id INTEGER NOT NULL,
    Company_Id INTEGER NOT NULL,
    FOREIGN KEY(Currency_Id) REFERENCES Currencies(Id)
    FOREIGN KEY(Company_Id) REFERENCES Companies(Id)
)" );

            ctx.Database.ExecuteSqlCommand( @"
CREATE TABLE InterestExpenses (
    Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    Value DOUBLE NOT NULL,
    Source TEXT NOT NULL,
    RawPeriod TEXT NOT NULL,
    Timestamp DATETIME NOT NULL,
    Currency_Id INTEGER NOT NULL,
    Company_Id INTEGER NOT NULL,
    FOREIGN KEY(Currency_Id) REFERENCES Currencies(Id)
    FOREIGN KEY(Company_Id) REFERENCES Companies(Id)
)" );

            ctx.Database.ExecuteSqlCommand( @"
CREATE TABLE Liabilities (
    Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    Value DOUBLE NOT NULL,
    Source TEXT NOT NULL,
    RawPeriod TEXT NOT NULL,
    Timestamp DATETIME NOT NULL,
    Currency_Id INTEGER NOT NULL,
    Company_Id INTEGER NOT NULL,
    FOREIGN KEY(Currency_Id) REFERENCES Currencies(Id)
    FOREIGN KEY(Company_Id) REFERENCES Companies(Id)
)" );

            ctx.Database.ExecuteSqlCommand( @"
CREATE TABLE NetIncomes (
    Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    Value DOUBLE NOT NULL,
    Source TEXT NOT NULL,
    RawPeriod TEXT NOT NULL,
    Timestamp DATETIME NOT NULL,
    Currency_Id INTEGER NOT NULL,
    Company_Id INTEGER NOT NULL,
    FOREIGN KEY(Currency_Id) REFERENCES Currencies(Id)
    FOREIGN KEY(Company_Id) REFERENCES Companies(Id)
)" );

            ctx.Database.ExecuteSqlCommand( @"
CREATE TABLE Revenues (
    Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    Value DOUBLE NOT NULL,
    Source TEXT NOT NULL,
    RawPeriod TEXT NOT NULL,
    Timestamp DATETIME NOT NULL,
    Currency_Id INTEGER NOT NULL,
    Company_Id INTEGER NOT NULL,
    FOREIGN KEY(Currency_Id) REFERENCES Currencies(Id)
    FOREIGN KEY(Company_Id) REFERENCES Companies(Id)
)" );

            ctx.Database.ExecuteSqlCommand( @"
CREATE TABLE SharesOutstandings (
    Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    Value DOUBLE NOT NULL,
    Source TEXT NOT NULL,
    RawPeriod TEXT NOT NULL,
    Timestamp DATETIME NOT NULL,
    Company_Id INTEGER NOT NULL,
    FOREIGN KEY(Company_Id) REFERENCES Companies(Id)
)" );
        }

        private void ToVersion10( AssetsContext ctx )
        {
            ctx.Database.ExecuteSqlCommand( @"
COMMIT;

PRAGMA foreign_keys = false;

BEGIN TRANSACTION;
CREATE TABLE Companies_new (
    Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    Name TEXT NOT NULL,
    Homepage TEXT NULL,
    Sector TEXT NULL,
    Country TEXT NULL
);

INSERT INTO Companies_new SELECT Id, Name, Homepage, Sector, Country FROM Companies;
DROP TABLE Companies;
ALTER TABLE Companies_new RENAME TO Companies;
COMMIT;

PRAGMA foreign_keys = true;

BEGIN TRANSACTION;
" );
        }

        private void ToVersion11( AssetsContext ctx )
        {
            ctx.Database.ExecuteSqlCommand( @"ALTER TABLE Stocks ADD COLUMN Wpkn TEXT;" );
            ctx.Database.ExecuteSqlCommand( @"ALTER TABLE Stocks ADD COLUMN Symbol TEXT;" );
        }

        private void ToVersion12( AssetsContext ctx )
        {
            ctx.Database.ExecuteSqlCommand( @"ALTER TABLE Companies ADD COLUMN Guid TEXT;" );
            ctx.Database.ExecuteSqlCommand( @"ALTER TABLE Stocks ADD COLUMN Guid TEXT;" );

            foreach( var company in ctx.Companies )
            {
                company.Guid = Guid.NewGuid().ToString();
            }

            foreach( var stock in ctx.Stocks )
            {
                stock.Guid = Guid.NewGuid().ToString();
            }

            ctx.SaveChanges();

            ctx.Database.ExecuteSqlCommand( @"
COMMIT;

PRAGMA foreign_keys = false;

BEGIN TRANSACTION;
CREATE TABLE Companies_new (
    Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    Guid TEXT NOT NULL,
    Name TEXT NOT NULL,
    Homepage TEXT NULL,
    Sector TEXT NULL,
    Country TEXT NULL
);

INSERT INTO Companies_new SELECT Id, Guid, Name, Homepage, Sector, Country FROM Companies;
DROP TABLE Companies;
ALTER TABLE Companies_new RENAME TO Companies;
COMMIT;

PRAGMA foreign_keys = true;

BEGIN TRANSACTION;
" );

            ctx.Database.ExecuteSqlCommand( @"
COMMIT;

PRAGMA foreign_keys = false;

BEGIN TRANSACTION;
CREATE TABLE Stocks_new (
    Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, 
    Guid TEXT NOT NULL,
    Isin TEXT NOT NULL,
    Wpkn TEXT,
    Symbol TEXT,
    Company_id INTEGER NOT NULL,
    FOREIGN KEY(Company_id) REFERENCES Companies(Id)
);

INSERT INTO Stocks_new SELECT Id, Guid, Isin, Wpkn, Symbol, Company_Id FROM Stocks;
DROP TABLE Stocks;
ALTER TABLE Stocks_new RENAME TO Stocks;
COMMIT;

PRAGMA foreign_keys = true;

BEGIN TRANSACTION;
" );
        }
    }
}
