using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TriviaNation;

namespace TriviaNationTests
{
    [TestClass]
    public class DataBaseOperationsTest
    {

        [TestMethod]
        public void TestToMakeSureConnectToDBMethodOpensConnectionToTheDataBase()
        {
            //Arrange
            new DataBaseOperations();

            //Act
            DataBaseOperations.ConnectToDB();
            bool ConnectionIsOpen = (DataBaseOperations.Connection.State == ConnectionState.Open);

            //Assert
            Assert.IsTrue(ConnectionIsOpen);
        }

        [TestMethod]
        public void TableExistsMethodTest()
        {
            //Arrange
            new DataBaseOperations();
            DataBaseOperations.ConnectToDB();
            SqlConnection s_connection = DataBaseOperations.Connection;
            String DropTableSQLCode = ("DROP TABLE IF EXISTS TestTable1;");
            SqlCommand deleteTableCommand = new SqlCommand(DropTableSQLCode, s_connection);
            deleteTableCommand.ExecuteNonQuery();
            String TSQLSourceCode = "CREATE TABLE TestTable1(columnone varchar(4000) not null PRIMARY KEY, columntwo varchar(4000) not null);";
            SqlCommand command = new SqlCommand(TSQLSourceCode, s_connection);
            command.ExecuteNonQuery();

            //Act
            bool tableExist = DataBaseOperations.TableExists("TestTable1");

            //Assert
            Assert.AreEqual(true, tableExist);
        }

        [TestMethod]
        public void TestCreateTableMethodToMakeSureItActuallyCreatesATableInTheDataBase()
        {
            //Arrange
            new DataBaseOperations();
            DataBaseOperations.ConnectToDB();
            SqlConnection s_connection = DataBaseOperations.Connection;
            SqlDataReader myReader = null;
            int count = 0;
            String tableName = "TestTable2";
            String sqlString = "SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'TestTable2'";
            String tableCreationString = "(question varchar(4000) not null PRIMARY KEY, answer varchar(4000) not null);";

            //Act
            DataBaseOperations.CreateTable(tableName, tableCreationString);
            SqlCommand command = new SqlCommand(sqlString, s_connection);
            myReader = command.ExecuteReader();
            while (myReader.Read())
            {
                count++;
            }

            //Assert
            Assert.AreNotEqual(0, count);
        }

        [TestMethod]
        public void TestDeleteTableMethodToMakeSureItActuallyDeletesTheTableFromTheDataBase()
        {
            //Arrange
            new DataBaseOperations();
            DataBaseOperations.ConnectToDB();
            SqlConnection s_connection = DataBaseOperations.Connection;
            String DropTableSQLCode3 = ("DROP TABLE IF EXISTS TestTable3;");
            SqlCommand deleteTableCommand3 = new SqlCommand(DropTableSQLCode3, s_connection);
            deleteTableCommand3.ExecuteNonQuery();
            String TSQLSourceCode3 = "CREATE TABLE TestTable3(columnone varchar(4000) not null PRIMARY KEY, columntwo varchar(4000) not null);";
            SqlCommand command3 = new SqlCommand(TSQLSourceCode3, s_connection);
            command3.ExecuteNonQuery();
            SqlDataReader myReader = null;
            int count = 0;
            String sqlString = "SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'TestTable3'";

            //Act
            DataBaseOperations.DeleteTable("TestTable3");
            SqlCommand command = new SqlCommand(sqlString, s_connection);
            myReader = command.ExecuteReader();
            while (myReader.Read())
            {
                count++;
            }

            //Assert
            Assert.AreEqual(0, count);
        }

        [TestMethod]
        public void TestRetrieveNumberOfRowsInTableMethodToMakeSureItRetrievesTheCorrectNumberOfRows()
        {
            //Arrange
            new DataBaseOperations();
            DataBaseOperations.ConnectToDB();
            SqlConnection s_connection = DataBaseOperations.Connection;
            String DropTableSQLCode4 = ("DROP TABLE IF EXISTS TestTable4;");
            SqlCommand deleteTableCommand4 = new SqlCommand(DropTableSQLCode4, s_connection);
            deleteTableCommand4.ExecuteNonQuery();
            String TSQLSourceCode4 = "CREATE TABLE TestTable4(columnone varchar(4000) not null PRIMARY KEY, columntwo varchar(4000) not null);";
            SqlCommand command4 = new SqlCommand(TSQLSourceCode4, s_connection);
            command4.ExecuteNonQuery();
            String insertStringRowOne = "INSERT INTO TestTable4(columnone, columntwo) VALUES ('This is rowone columnone', 'This is rowone columntwo');";
            SqlCommand command = new SqlCommand(insertStringRowOne, s_connection);
            command.ExecuteNonQuery();

            //Act
            int numRowsInTable = DataBaseOperations.RetrieveNumberOfRowsInTable("TestTable4");

            //Assert
            Assert.AreEqual(1, numRowsInTable);
        }

        [TestMethod]
        public void TestRetrieveNumberOfColsInTableMethodToMakeSureItRetrievesTheCorrectNumberOfRows()
        {
            //Arrange
            new DataBaseOperations();
            DataBaseOperations.ConnectToDB();
            SqlConnection s_connection = DataBaseOperations.Connection;
            String DropTableSQLCode5 = ("DROP TABLE IF EXISTS TestTable5;");
            SqlCommand deleteTableCommand5 = new SqlCommand(DropTableSQLCode5, s_connection);
            deleteTableCommand5.ExecuteNonQuery();
            String TSQLSourceCode5 = "CREATE TABLE TestTable5(columnone varchar(4000) not null PRIMARY KEY, columntwo varchar(4000) not null);";
            SqlCommand command5 = new SqlCommand(TSQLSourceCode5, s_connection);
            command5.ExecuteNonQuery();

            //Act
            int numColsInTable = DataBaseOperations.RetrieveNumberOfColsInTable("TestTable5");

            //Assert
            Assert.AreEqual(2, numColsInTable);
        }

        [TestMethod]
        public void TestInsertIntoTableMethodToMakeSureTheRowGetsInsertedProperly()
        {
            //Arrange
            new DataBaseOperations();
            DataBaseOperations.ConnectToDB();
            SqlConnection s_connection = DataBaseOperations.Connection;
            String DropTableSQLCode6 = ("DROP TABLE IF EXISTS TestTable6;");
            SqlCommand deleteTableCommand6 = new SqlCommand(DropTableSQLCode6, s_connection);
            deleteTableCommand6.ExecuteNonQuery();
            String TSQLSourceCode6 = "CREATE TABLE TestTable6(columnone varchar(4000) not null PRIMARY KEY, columntwo varchar(4000) not null);";
            SqlCommand command6 = new SqlCommand(TSQLSourceCode6, s_connection);
            command6.ExecuteNonQuery();
            String insertRowString = "INSERT INTO TestTable6(columnone, columntwo) VALUES ('This is rowone columnone', 'This is rowone columntwo');";
            String retrievedRow = "";
            String TSQLSourceCode = ("SELECT * FROM(Select Row_Number() Over (Order By columnone) As RowNum, * From TestTable6) t2 where RowNum = 1;");

            //Act
            DataBaseOperations.InsertIntoTable(insertRowString);
            using (SqlCommand command = new SqlCommand(TSQLSourceCode, s_connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        retrievedRow += reader.GetString(1) + reader.GetString(2);
                    }
                }
            }
            
            //Assert
            Assert.AreEqual("This is rowone columnoneThis is rowone columntwo", retrievedRow);
        }

        [TestMethod]
        public void TestRetrieveRowFromTableMethodToMakeSureTheRowGetsRetrievedProperly()
        {
            //Arrange
            new DataBaseOperations();
            DataBaseOperations.ConnectToDB();
            SqlConnection s_connection = DataBaseOperations.Connection;
            String DropTableSQLCode7 = ("DROP TABLE IF EXISTS TestTable7;");
            SqlCommand deleteTableCommand7 = new SqlCommand(DropTableSQLCode7, s_connection);
            deleteTableCommand7.ExecuteNonQuery();
            String TSQLSourceCode7 = "CREATE TABLE TestTable7(columnone varchar(4000) not null PRIMARY KEY, columntwo varchar(4000) not null);";
            SqlCommand command7 = new SqlCommand(TSQLSourceCode7, s_connection);
            command7.ExecuteNonQuery();
            String insertString = "INSERT INTO TestTable7(columnone, columntwo) VALUES ('This is rowone columnone', 'This is rowone columntwo');";
            SqlCommand command = new SqlCommand(insertString, s_connection);
            command.ExecuteNonQuery();
            String rowToRetriveSQLCode = ("SELECT * FROM(Select Row_Number() Over (Order By columnone) As RowNum, * From TestTable7) t2 where RowNum = 1;");
            
            //Act
            String retrievedRow = DataBaseOperations.RetrieveRowFromTable(rowToRetriveSQLCode);

            //Assert
            Assert.AreEqual(("This is rowone columnone" + "\n" + "This is rowone columntwo" + "\n"), retrievedRow);
        }

        [TestMethod]
        public void TestDeleteRowFromTableMethodToMakeSureItDeletesARowFromTheDataBase()
        {
            //Arrange
            new DataBaseOperations();
            DataBaseOperations.ConnectToDB();
            SqlConnection s_connection = DataBaseOperations.Connection;
            String DropTableSQLCode8 = ("DROP TABLE IF EXISTS TestTable8;");
            SqlCommand deleteTableCommand8 = new SqlCommand(DropTableSQLCode8, s_connection);
            deleteTableCommand8.ExecuteNonQuery();
            String TSQLSourceCode8 = "CREATE TABLE TestTable8(columnone varchar(4000) not null PRIMARY KEY, columntwo varchar(4000) not null);";
            SqlCommand command8 = new SqlCommand(TSQLSourceCode8, s_connection);
            command8.ExecuteNonQuery();
            String insertStringRowOne = "INSERT INTO TestTable8(columnone, columntwo) VALUES ('This is rowone columnone', 'This is rowone columntwo');";
            SqlCommand command = new SqlCommand(insertStringRowOne, s_connection);
            command.ExecuteNonQuery();
            String rowToDelete = ("DELETE FROM TestTable8 WHERE columnone='This is rowone columnone';");
            int numberOfRowsInTable = 0;
            String NumRowsCountSQLCode = "SELECT COUNT(*) FROM TestTable8;";

            //Act
            DataBaseOperations.DeleteRowFromTable(rowToDelete);
            using (SqlCommand cmd = new SqlCommand(NumRowsCountSQLCode, s_connection))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        numberOfRowsInTable = reader.GetInt32(0);
                    }
                }
            }

            //Assert
            Assert.AreEqual(0, numberOfRowsInTable);
        }

        [TestCleanup]
        public void CleanUpAfterTests()
        {
            new DataBaseOperations();
            DataBaseOperations.ConnectToDB();
            SqlConnection s_connection = DataBaseOperations.Connection;
            String DropTableSQLCode1 = ("DROP TABLE IF EXISTS TestTable1;");
            SqlCommand deleteTableCommand1 = new SqlCommand(DropTableSQLCode1, s_connection);
            deleteTableCommand1.ExecuteNonQuery();
            String DropTableSQLCode2 = ("DROP TABLE IF EXISTS TestTable2;");
            SqlCommand deleteTableCommand2 = new SqlCommand(DropTableSQLCode2, s_connection);
            deleteTableCommand2.ExecuteNonQuery();
            String DropTableSQLCode4 = ("DROP TABLE IF EXISTS TestTable4;");
            SqlCommand deleteTableCommand4 = new SqlCommand(DropTableSQLCode4, s_connection);
            deleteTableCommand4.ExecuteNonQuery();
            String DropTableSQLCode5 = ("DROP TABLE IF EXISTS TestTable5;");
            SqlCommand deleteTableCommand5 = new SqlCommand(DropTableSQLCode5, s_connection);
            deleteTableCommand5.ExecuteNonQuery();
            String DropTableSQLCode6 = ("DROP TABLE IF EXISTS TestTable6;");
            SqlCommand deleteTableCommand6 = new SqlCommand(DropTableSQLCode6, s_connection);
            deleteTableCommand6.ExecuteNonQuery();
            String DropTableSQLCode7 = ("DROP TABLE IF EXISTS TestTable7;");
            SqlCommand deleteTableCommand7 = new SqlCommand(DropTableSQLCode7, s_connection);
            deleteTableCommand7.ExecuteNonQuery();
            String DropTableSQLCode8 = ("DROP TABLE IF EXISTS TestTable8;");
            SqlCommand deleteTableCommand8 = new SqlCommand(DropTableSQLCode8, s_connection);
            deleteTableCommand8.ExecuteNonQuery();
        }
    }
}
