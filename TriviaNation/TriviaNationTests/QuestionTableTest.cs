using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TriviaNation;

namespace TriviaNationTests
{
    [TestClass]
    public class QuestionTableTest
    {
        private string tableName;
        private String tableCreationString;
        private String insertString;

        [TestInitialize]
        public void Initialize() {
            tableName = "QuestionTable";
            tableCreationString = "exTableCreationString";
            insertString = "exInsertString";
        }

        [TestMethod]
        public void TestToSeeIfATableExists() {
            /* does not work with DBO
             * 
            // Arrange
            Mock<DataBaseOperations> mockDatabase = new Mock<DataBaseOperations>();
            mockDatabase.Setup(r => r.TableExists(tableName)).Returns(true);
            */

            // Arrange
            Mock<IDataBaseTable> mockDatabase = new Mock<IDataBaseTable>();
            mockDatabase.Setup(r => r.TableExists()).Returns(true);

            // Act
            bool expected = true;
            bool actual = DataBaseOperations.TableExists(tableName);

            //Assert
            Assert.AreEqual(expected, actual);

        }

        [TestMethod]
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

        [TestMethod]
        public void TestToSeeIfNumOfRowsIsRetrieved()
        {
            // Arrange
            Mock<DataBaseOperations> dbo = new Mock<DataBaseOperations>();
            dbo.Setup(r => r.RetrieveNumberOfRowsInTable(tableName).Returns(3);

            // Act
            DataBaseOperations obj = new DataBaseOperations();

            // Assert
            Assert.AreEqual(obj.RetrieveNumberOfRowsInTable(tableName), 3);

        }

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

    }
}
