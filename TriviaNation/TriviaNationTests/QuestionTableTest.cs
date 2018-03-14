using System;
using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TriviaNation;

namespace TriviaNationTests
{
    [TestClass]
    public class QuestionTableTest
    {
        private QuestionTable QT;

        [TestInitialize]
        public void Initialize() {
            QT = new QuestionTable();
        }
        
        [TestMethod]
        public void TestTableExistsMethodToSeeIfAKnownTableExists()
        {
            // Arrange
            new DataBaseOperations();
            DataBaseOperations.ConnectToDB();
            SqlConnection s_connection = DataBaseOperations.Connection;
            var sut = new QuestionTable();

            // Act
            bool tableExists = sut.TableExists(sut.TableName);

            //Assert
            Assert.AreEqual(true, tableExists);
        }        

        [TestMethod]
        public void TestCreateTableMethodTableShouldGetCreated()
        {
            // Arrange
            new DataBaseOperations();
            DataBaseOperations.ConnectToDB();
            SqlConnection s_connection = DataBaseOperations.Connection;
            String sqlString = "SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'QTTestTable2'";
            int count = 0;
            String nameOfTestTable = "QTTestTable2";
            String tableCreationString = "(columnone varchar(4000) not null PRIMARY KEY, columntwo varchar(4000) not null);";

            // Act
            QT.CreateTable(nameOfTestTable, tableCreationString);
            SqlCommand command = new SqlCommand(sqlString, s_connection);
            SqlDataReader myReader = command.ExecuteReader();
            while (myReader.Read())
            {
                count++;
            }

            //Assert
            Assert.AreEqual(1, count);
        }

        /*
        [TestMethod]
        public void TestInsertRowIntoTableMethodToSeeIfRowGetsInserted() {
            // Arrange
            Mock<DataBaseOperations> dbo = new Mock<DataBaseOperations>();
            dbo.Setup(r => r.InsertIntoTable(insertString)).Returns("Insertion complete!");

            // Act
            DataBaseOperations obj = new DataBaseOperations();

            // Assert
            Assert.AreEqual(obj.InsertIntoTable(dbo.Object), "Insertion complete!");
            
        }
        */

        [TestMethod]
        public void TestRetrieveNumberOfRowsInTableMethodShouldReturnIntNotNUll()
        {
            //Arrange
            new DataBaseOperations();
            DataBaseOperations.ConnectToDB();
            SqlConnection s_connection = DataBaseOperations.Connection;
            var sut = new QuestionTable();

            //Act
            int numberReturned = sut.RetrieveNumberOfRowsInTable();

            //Assert
            Assert.IsNotNull(numberReturned);
        }

        /*
        [TestMethod]
        public void TestToSeeIfRowIsRetrieved()
        {

        }
        */

        [TestMethod]
        public void TestRetrieveNumberOfColsInTableMethodShouldReturnIntNotNUll()
        {
            //Arrange
            new DataBaseOperations();
            DataBaseOperations.ConnectToDB();
            SqlConnection s_connection = DataBaseOperations.Connection;
            var sut = new QuestionTable();

            //Act
            int numberReturned = sut.RetriveNumberOfColsInTable();

            //Assert
            Assert.IsNotNull(numberReturned);
        }

        /*
        [TestMethod]
        public void TestToSeeIfRowIsDeleted()
        {

        }
        */

        public void CleanUpAfterTests()
        {
            new DataBaseOperations();
            DataBaseOperations.ConnectToDB();
            SqlConnection s_connection = DataBaseOperations.Connection;

            String DropTableSQLCode1 = ("DROP TABLE IF EXISTS TestTable1;");
            SqlCommand deleteTableCommand1 = new SqlCommand(DropTableSQLCode1, s_connection);
            deleteTableCommand1.ExecuteNonQuery();
        }
    }
}