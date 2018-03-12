using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
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
        }

    }
}
