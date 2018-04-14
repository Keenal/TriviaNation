using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TriviaNation;

namespace TriviaNationTests
{
    [TestClass]
    public class UserTableTest
    {
        private UserTable UT;
        SqlConnection s_connection;

        [TestInitialize]
        public void Initialize()
        {
            new DataBaseOperations();
            DataBaseOperations.ConnectToDB();
            s_connection = DataBaseOperations.Connection;
            UT = new UserTable();
        }

        [TestMethod]
        public void TestTableExistsMethodToSeeIfAKnownTableExists()
        {
            // Arrange
            var sut = new UserTable();

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
            String sqlString = "SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'UTTestTable1'";
            int count = 0;
            String nameOfTestTable = "UTTestTable1";
            String tableCreationString = "(columnone varchar(4000) not null PRIMARY KEY, columntwo varchar(4000) not null, columnthree varchar(4000) not null, columnfour varchar(4000) not null);";

            // Act
            UT.CreateTable(nameOfTestTable, tableCreationString);
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
            String tableDropCode = ("DROP TABLE IF EXISTS UTTestTable2;");
            SqlCommand deleteTableCommand = new SqlCommand(tableDropCode, s_connection);
            deleteTableCommand.ExecuteNonQuery();
            String tableCreationString = "CREATE TABLE UTTestTable2(username varchar(4000) not null PRIMARY KEY, email varchar(4000) not null, password varchar(4000) not null, score varchar(4000) not null);";
            SqlCommand command = new SqlCommand(tableCreationString, s_connection);
            command.ExecuteNonQuery();
            String retrievedRow = "";
            String TSQLSourceCode = ("SELECT * FROM(Select Row_Number() Over (Order By username) As RowNum, * From UTTestTable2) t2 where RowNum = 1;");

            List<string> userValues = new List<string>();
            userValues.Add("UsernameTest");
            userValues.Add("EmailTest");
            userValues.Add("PasswordTest");
            userValues.Add("ScoreTest");
            Mock<IDataEntry> mockDataEntry = new Mock<IDataEntry>();
            mockDataEntry.Setup(r => r.GetValues()).Returns(userValues);

            // Act
            UT.InsertRowIntoTable("UTTestTable2", mockDataEntry.Object);
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
            Assert.AreEqual(("UsernameTest" + "\n" + "EmailTest" + "\n" + "PasswordTest" + "\n" + "ScoreTest" + "\n"), retrievedRow);
        }

        [TestMethod]
        public void TestRetrieveNumberOfRowsInTableMethodShouldReturnIntNotNUll()
        {
            //Arrange
            var sut = new UserTable();

            //Act
            int numberReturned = sut.RetrieveNumberOfRowsInTable();

            //Assert
            Assert.IsNotNull(numberReturned);
        }

        [TestMethod]
        public void TestRetrieveRowInTableMethodShouldReturnTheRowFromSpecificRowNumber()
        {
            // Arrange
            String tableDropCode = ("DROP TABLE IF EXISTS UTTestTable3;");
            SqlCommand deleteTableCommand = new SqlCommand(tableDropCode, s_connection);
            deleteTableCommand.ExecuteNonQuery();
            String tableCreationString = "CREATE TABLE UTTestTable3(username varchar(4000) not null PRIMARY KEY, email varchar(4000) not null, password varchar(4000) not null, score varchar(4000) not null);";
            SqlCommand createCmd = new SqlCommand(tableCreationString, s_connection);
            createCmd.ExecuteNonQuery();
            String insertString = "INSERT INTO UTTestTable3(username, email, password, score) VALUES ('This is username1', 'This is email1', 'password1', 'score1');";
            SqlCommand insertCmd = new SqlCommand(insertString, s_connection);
            insertCmd.ExecuteNonQuery();

            // Act
            String rowRetrieved = UT.RetrieveTableRow("UTTestTable3", 1);

            // Assert
            Assert.AreEqual("This is username1" + "\n" + "This is email1" + "\n" + "password1" + "\n" + "score1" + "\n", rowRetrieved);
        }

        [TestMethod]
        public void TestRetrieveNumberOfColsInTableMethodShouldReturnIntNotNUll()
        {
            //Arrange
            var sut = new UserTable();

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
            var sut = new UserTable();
            String usernameString = "This is username1";
            String sqlString = "DELETE FROM UserTable WHERE username='" + usernameString + "';";

            // Act
            UT.DeleteRowFromTable(usernameString);
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
            String DropTableSQLCode1 = ("DROP TABLE IF EXISTS UTTestTable1;");
            SqlCommand deleteTableCommand1 = new SqlCommand(DropTableSQLCode1, s_connection);
            deleteTableCommand1.ExecuteNonQuery();
            String DropTableSQLCode2 = ("DROP TABLE IF EXISTS UTTestTable2;");
            SqlCommand deleteTableCommand2 = new SqlCommand(DropTableSQLCode2, s_connection);
            deleteTableCommand2.ExecuteNonQuery();
            String DropTableSQLCode3 = ("DROP TABLE IF EXISTS UTTestTable3;");
            SqlCommand deleteTableCommand3 = new SqlCommand(DropTableSQLCode3, s_connection);
            deleteTableCommand3.ExecuteNonQuery();
        }
    }
}