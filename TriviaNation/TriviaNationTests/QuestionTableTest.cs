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
        private string tableName;
        private String tableCreationString;
        private String insertString;

        [TestInitialize]
        public void Initialize() {
            QT = new QuestionTable();
            tableName = "QuestionTable";
            tableCreationString = "exTableCreationString";
            insertString = "exInsertString";
        }
        /*
        [TestMethod]
        public void TestToSeeIfATableExists() {
            /* does not work with DBO
             * 
            // Arrange
            Mock<DataBaseOperations> mockDatabase = new Mock<DataBaseOperations>();
            mockDatabase.Setup(r => r.TableExists(tableName)).Returns(true);
            

            // Arrange
            Mock<IDataBaseTable> mockDatabase = new Mock<IDataBaseTable>();
            mockDatabase.Setup(r => r.TableExists()).Returns(true);

            // Act
            bool expected = true;
            bool actual = DataBaseOperations.TableExists(tableName);

            //Assert
            Assert.AreEqual(expected, actual);

        }
    */



        [TestMethod]
        public void TestToSeeIfATableIsCreated() {
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
        public void TestToSeeIfRowIsInserted() {
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
        public void TestRetrieveNumberOfRowsInTableMethodToSeeIfCorrectNumOfRowsIsRetrieved()
        {
            // Arrange
            //Mock<IDataBaseOperations> dbo = new Mock<IDataBaseOperations>();
            //dbo.Setup(s => s.RetrieveNumberOfRowsInTable("TestTable")).Returns(3);


            // Act
           // DataBaseOperations obj = new DataBaseOperations();

            // Assert
           // Assert.AreEqual(obj.RetrieveNumberOfRowsInTable(tableName), 3);

        }

        /*
        [TestMethod]
        public void TestToSeeIfRowIsRetrieved()
        {

        }

        [TestMethod]
        public void TestToSeeIfNumOfColsIsRetrieved()
        {
            // Arrange
            Mock<DataBaseOperations> dbo = new Mock<DataBaseOperations>();
            dbo.Setup(r => r.RetrieveNumberOfColsInTable(tableName).Returns(2);

            // Act
            DataBaseOperations obj = new DataBaseOperations();

            // Assert
            Assert.AreEqual(obj.RetrieveNumberOfColsInTable(tableName), 2);
        }

        [TestMethod]
        public void TestToSeeIfRowIsDeleted()
        {

        }
        */
    }
}
/*
 *[TestMethod]
        public void TestToSeeIfATableIsCreated() {
            // Arrange
            Mock<IDataBaseTable> mockDatabase = new Mock<IDataBaseTable>();
            mockDatabase.Setup(r => r.CreateTable());

            // Act
            void expected = "Creation of " + tableName + "complete!";
            void actual = DataBaseOperations.CreateTable(tableName, tableCreationString);

            //Assert
            Assert.AreEqual(expected, actual);
        }
*/