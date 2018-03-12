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
            String TSQLSourceCode = "CREATE TABLE TestTable(columnone varchar(4000) not null PRIMARY KEY, columntwo varchar(4000) not null);"
            Mock<SqlCommand> mockCommand = new Mock<SqlCommand>(TSQLSourceCode, s_connection);
            mockCommand.Setup(c => c.ExecuteNonQuery()).Returns(-1);
            

            //Act
            bool tableExist = DataBaseOperations.TableExists("TestTable");

            //Assert
            Assert.AreEqual(true, tableExist);
        }

        /*
        [TestMethod]
        public void TableExistsMethodTest()
        {
            //Arrange
            new DataBaseOperations();
            DataBaseOperations.ConnectToDB();
            SqlConnection s_connection = DataBaseOperations.Connection;
            String TSQLSourceCode = "CREATE TABLE QuestionTable(question varchar(4000) not null PRIMARY KEY, answer varchar(4000) not null);"
            Mock<SqlCommand> mockCommand = new Mock<SqlCommand>(TSQLSourceCode, s_connection);
            mockCommand.Setup(c => c.ExecuteNonQuery()).Returns(-1);


            //Act
            bool tableExist = DataBaseOperations.TableExists("TestTable");

            //Assert
            Assert.AreEqual(true, tableExist);
        }
        */
    }
}
