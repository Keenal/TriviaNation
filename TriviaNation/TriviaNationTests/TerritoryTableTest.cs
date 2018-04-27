using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TriviaNation;

namespace TriviaNationTests
{
    [TestClass]
    public class TerritoryTableTest
    {
        private TerritoryTable TT;
        SqlConnection s_connection;

        [TestInitialize]
        public void Initialize()
        {
            new DataBaseOperations();
            DataBaseOperations.ConnectToDB();
            s_connection = DataBaseOperations.Connection;
            TT = new TerritoryTable();
        }

        [TestMethod]
        public void TestTableExistsMethodToSeeIfAKnownTableExists()
        {
            // Arrange
            var sut = new TerritoryTable();

            // Act
            bool tableExists = sut.TableExists(sut.TableName);
            bool expected = true;
            bool actual = tableExists;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestCreateTableMethodTableShouldGetCreated()
        {
            // Arrange
            String sqlString = "SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'TTTestTable1'";
            int count = 0;
            String nameOfTestTable = "TTTestTable1";
            String tableCreationString = "(columnone varchar(4000) not null PRIMARY KEY, columntwo varchar(4000) not null, columnthree varchar(4000) not null);";

            // Act
            TT.CreateTable(nameOfTestTable, tableCreationString);
            SqlCommand command = new SqlCommand(sqlString, s_connection);
            SqlDataReader myReader = command.ExecuteReader();
            while (myReader.Read())
            {
                count++;
            }

            //Assert
            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void TestInsertRowIntoTableMethodToSeeIfRowGetsInserted()
        {
            // Arrange
            String tableDropCode = ("DROP TABLE IF EXISTS TTTestTable2;");
            SqlCommand deleteTableCommand = new SqlCommand(tableDropCode, s_connection);
            deleteTableCommand.ExecuteNonQuery();
            String tableCreationString = "CREATE TABLE TTTestTable2(territoryIndex varchar(50) not null PRIMARY KEY, username varchar(4000), color varchar(50), playersTurn varchar(5));";
            SqlCommand command = new SqlCommand(tableCreationString, s_connection);
            command.ExecuteNonQuery();
            String retrievedRow = "";
            String TSQLSourceCode = ("SELECT * FROM(Select Row_Number() Over (Order By territoryIndex) As RowNum, * From TTTestTable2) t2 where RowNum = 1;");

            List<string> userValues = new List<string>();
            userValues.Add("territoryIndexTest");
            userValues.Add("usernameTest");
            userValues.Add("colorTest");
            userValues.Add("0");
            Mock<IDataEntry> mockDataEntry = new Mock<IDataEntry>();
            mockDataEntry.Setup(r => r.GetValues()).Returns(userValues);

            // Act
            TT.InsertRowIntoTable("TTTestTable2", mockDataEntry.Object);
            using (SqlCommand cmd = new SqlCommand(TSQLSourceCode, s_connection))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        for (int i = 1; i < reader.FieldCount; i++)
                        {
                            retrievedRow += (reader.GetString(i) + "\n");
                        }
                    }
                }
            }

            // Assert
            Assert.AreEqual(("territoryIndexTest" + "\n" + "usernameTest" + "\n" + "colorTest" + "\n" + "0" + "\n"), retrievedRow);
        }

        [TestMethod]
        public void TestRetrieveNumberOfRowsInTableMethodShouldReturnIntNotNUll()
        {
            //Arrange
            var sut = new TerritoryTable();

            //Act
            int numberReturned = sut.RetrieveNumberOfRowsInTable();

            //Assert
            Assert.IsNotNull(numberReturned);
        }

        [TestMethod]
        public void TestRetrieveRowInTableMethodShouldReturnTheRowFromSpecificRowNumber()
        {
            // Arrange
            String tableDropCode = ("DROP TABLE IF EXISTS TTTestTable3;");
            SqlCommand deleteTableCommand = new SqlCommand(tableDropCode, s_connection);
            deleteTableCommand.ExecuteNonQuery();
            String tableCreationString = "CREATE TABLE TTTestTable3(territoryIndex varchar(50) not null PRIMARY KEY, username varchar(4000), color varchar(50));";
            SqlCommand createCmd = new SqlCommand(tableCreationString, s_connection);
            createCmd.ExecuteNonQuery();
            String insertString = "INSERT INTO TTTestTable3(territoryIndex, username, color) VALUES ('This is territoryIndex1', 'This is username1', 'color1');";
            SqlCommand insertCmd = new SqlCommand(insertString, s_connection);
            insertCmd.ExecuteNonQuery();

            // Act
            String rowRetrieved = TT.RetrieveTableRow("TTTestTable3", 1);

            // Assert
            Assert.AreEqual("This is territoryIndex1" + "\n" + "This is username1" + "\n" + "color1" + "\n", rowRetrieved);
        }

        [TestMethod]
        public void TestRetrieveNumberOfColsInTableMethodShouldReturnIntNotNUll()
        {
            //Arrange
            var sut = new TerritoryTable();

            //Act
            int numberReturned = sut.RetriveNumberOfColsInTable();

            //Assert
            Assert.IsNotNull(numberReturned);
        }

        [TestMethod]
        public void TestToSeeIfRowIsDeleted()
        {
            // Arrange
            int count = 1;
            var sut = new TerritoryTable();
            String territoryIndexString = "This is territoryIndex1";
            String sqlString = "DELETE FROM Territories WHERE territoryIndex='" + territoryIndexString + "';";

            // Act
            TT.DeleteRowFromTable(territoryIndexString);
            SqlCommand command = new SqlCommand(sqlString, s_connection);
            SqlDataReader myReader = command.ExecuteReader();
            while (myReader.Read())
            {
                count++;
            }

            // Assert
            Assert.AreEqual(1, count);
        }

        public void CleanUpAfterTests()
        {
            String DropTableSQLCode1 = ("DROP TABLE IF EXISTS TTTestTable1;");
            SqlCommand deleteTableCommand1 = new SqlCommand(DropTableSQLCode1, s_connection);
            deleteTableCommand1.ExecuteNonQuery();
            String DropTableSQLCode2 = ("DROP TABLE IF EXISTS TTTestTable2;");
            SqlCommand deleteTableCommand2 = new SqlCommand(DropTableSQLCode2, s_connection);
            deleteTableCommand2.ExecuteNonQuery();
            String DropTableSQLCode3 = ("DROP TABLE IF EXISTS TTTestTable3;");
            SqlCommand deleteTableCommand3 = new SqlCommand(DropTableSQLCode3, s_connection);
            deleteTableCommand3.ExecuteNonQuery();
        }
    }
}