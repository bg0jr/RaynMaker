using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaynMaker.Entities.Persistancy
{
    class DBMigrationScript
    {
        public DBMigrationScript()
        {
            Migrations = new Dictionary<int, IList<string>>();

            MigrationVersion1();
            MigrationVersion2();
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
    }
}
