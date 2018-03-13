using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TriviaNation;

namespace TriviaNationTests
{
    [TestClass]
    public class QuestionTableTest
    {
        private IDataBaseTable sut;
        private string tableName;

        [TestInitialize]
        public void Initialize() {
            sut = null;
            tableName = "QuestionTable";
        }

        [TestMethod]
        public void TestToSeeIfATableIsCreated() {

            // Arrange
            Mock<IDataBaseTable> mockDatabase = new Mock<IDataBaseTable>();
            mockDatabase.Setup(r => r.TableExists()).Returns(true);

            // Act
            bool expected = true;
            bool actual = DataBaseOperations.TableExists(tableName);

            //Assert
            Assert.AreEqual(expected, actual);

        }
    }
}
