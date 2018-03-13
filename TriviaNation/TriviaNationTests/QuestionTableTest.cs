using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TriviaNation;

namespace TriviaNationTests
{
    [TestClass]
    public class QuestionTableTest
    {

        private String tableName;
        private String tableCreationString;
        private QuestionTable sut;

        [TestInitialize]
        public void Initialize() {
            tableName = "QuestionTable";
            tableCreationString = "(question varchar(4000) not null PRIMARY KEY, answer varchar(4000) not null);";
            sut = null;
        }
        [TestMethod]
        public void TestToSeeIfTableExists()
        {
            
            // Arrange
            Mock<DataBaseOperations> mockDataBaseOperations = new Mock<DataBaseOperations>();
            mockDataBaseOperations.Setup(r => r.TableExists).Returns(true);
            sut = new QuestionTable();

            // Act
            bool expected = true;
            bool actual = sut.TableExists(); 


            // Assert
            Assert.AreEqual(expected, actual);

        }
        [TestMethod]
        public void TestToSeeIfTableIsCreated() {
            

        }
    }
}
