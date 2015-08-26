using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaynMaker.Entities.Persistancy
{
    class DatabaseMigrations
    {
        public const int RequiredDatabaseVersion = 10;

        public DatabaseMigrations()
        {
            Migrations = new Dictionary<int, IList<string>>();

            MigrationVersion1();
            MigrationVersion2();
            MigrationVersion3();
            MigrationVersion4();
            MigrationVersion5();
            MigrationVersion6();
            MigrationVersion7();
            MigrationVersion8();
            MigrationVersion9();
            MigrationVersion10();
        }

        public Dictionary<int, IList<string>> Migrations { get; set; }

        private void MigrationVersion1()
        {
            var steps = new List<string>();

            steps.Add( @"
CREATE TABLE SchemaInfoes (
    Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    Version INTEGER NOT NULL
)" );

            steps.Add( @"
CREATE TABLE Companies (
    Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    Name TEXT NOT NULL
)" );

            steps.Add( @"
CREATE TABLE Stocks (
    Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, 
    Isin TEXT NOT NULL,
    Company_id INTEGER NOT NULL,
    FOREIGN KEY(Company_id) REFERENCES Companies(Id)
)" );

            Migrations.Add( 1, steps );
        }

        private void MigrationVersion2()
        {
            var steps = new List<string>();

            steps.Add( @"
CREATE TABLE AnalysisTemplates (
    Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    Name TEXT NOT NULL,
    Template TEXT NOT NULL
)" );

            Migrations.Add( 2, steps );
        }

        private void MigrationVersion3()
        {
            var steps = new List<string>();

            steps.Add( @"
CREATE TABLE Currencies (
    Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    Name TEXT NOT NULL
)" );

            steps.Add( @"
CREATE TABLE Translations (
    Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, 
    Rate DOUBLE NOT NULL,
    Timestamp DATETIME NOT NULL,
    Currency_id INTEGER NOT NULL,
    FOREIGN KEY(Currency_id) REFERENCES Currencies(Id)
)" );

            Migrations.Add( 3, steps );
        }

        private void MigrationVersion4()
        {
            var steps = new List<string>();

            steps.Add( @"DROP TABLE Translations" );

            steps.Add( @"
CREATE TABLE Translations (
    Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, 
    Rate DOUBLE NOT NULL,
    Timestamp DATETIME  NOT NULL,
    SourceId INTEGER NOT NULL,
    TargetId INTEGER NOT NULL,
    FOREIGN KEY(SourceId) REFERENCES Currencies(Id),
    FOREIGN KEY(TargetId) REFERENCES Currencies(Id)
)" );

            Migrations.Add( 4, steps );
        }

        private void MigrationVersion5()
        {
            var steps = new List<string>();

            steps.Add( @"DROP TABLE Companies" );

            steps.Add( @"
CREATE TABLE Companies (
    Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    Name TEXT NOT NULL,
    Homepage TEXT NULL,
    Sector TEXT NULL,
    Country TEXT NULL
)" );

            steps.Add( @"
CREATE TABLE 'References' (
    Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, 
    Url TEXT NOT NULL,
    Company_id INTEGER NOT NULL,
    FOREIGN KEY(Company_id) REFERENCES Companies(Id)
)" );

            Migrations.Add( 5, steps );
        }

        private void MigrationVersion6()
        {
            var steps = new List<string>();

            steps.Add( @"DROP TABLE Companies" );

            steps.Add( @"
CREATE TABLE Companies (
    Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    Name TEXT NOT NULL,
    Homepage TEXT NULL,
    Sector TEXT NULL,
    Country TEXT NULL,
    XdbPath TEXT NULL
)" );

            Migrations.Add( 6, steps );
        }

        private void MigrationVersion7()
        {
            var steps = new List<string>();

            steps.Add( @"
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

            Migrations.Add( 7, steps );
        }

        private void MigrationVersion8()
        {
            var steps = new List<string>();

            steps.Add( @"
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

            Migrations.Add( 8, steps );
        }

        private void MigrationVersion9()
        {
            var steps = new List<string>();

            steps.Add( @"
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

            steps.Add( @"
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

            steps.Add( @"
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

            steps.Add( @"
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

            steps.Add( @"
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

            steps.Add( @"
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

            steps.Add( @"
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

            steps.Add( @"
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

            steps.Add( @"
CREATE TABLE SharesOutstandings (
    Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    Value DOUBLE NOT NULL,
    Source TEXT NOT NULL,
    RawPeriod TEXT NOT NULL,
    Timestamp DATETIME NOT NULL,
    Company_Id INTEGER NOT NULL,
    FOREIGN KEY(Company_Id) REFERENCES Companies(Id)
)" );

            Migrations.Add( 9, steps );
        }

        private void MigrationVersion10()
        {
            var steps = new List<string>();


            steps.Add( @"
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

            Migrations.Add( 10, steps );
        }
    }
}
