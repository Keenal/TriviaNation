using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace TriviaNation
{
    [TestClass]
    public class TerritoryAdministrationTest
    {
        private ITriviaTerritory territory;
        private IDataBaseTable database;
        private ITerritoryAdministration admin;

        [TestInitialize]
        public void Initialize()
        {
            territory = new TriviaTerritory();
            database = new TerritoryTable();
            admin = new TerritoryAdministration(territory, database);
        }

        [TestMethod]
        public void MethodAddTerritoryShouldModifyTheFieldsOfTheClassItIsInAndThenPassItsOwnClassObjectAsAnArgumentToMethodInsertRowIntoTable()
        {
            // Arrange
            IDataEntry test = null;
            Mock<IDataBaseTable> mockDatabase = new Mock<IDataBaseTable>();
            mockDatabase.Setup(r => r.TableName).Returns("Table Name");
            mockDatabase.Setup(r => r.InsertRowIntoTable("Table Name", It.IsAny<IDataEntry>())).Callback<string, IDataEntry>((s1, s2) =>
            {
                test = s2;
            });
            ITerritoryAdministration sut = new TerritoryAdministration(territory, mockDatabase.Object);

            // Act
            sut.AddTerritory("5F", "Billy-Bob", "Purple");

            // Assert
            Assert.AreSame(sut, test);
            Assert.AreEqual(sut, test);
        }

        [TestMethod]
        public void MethodForDeletingATerritoryShouldFirstRetrieveARowFromDatabaseThenAccessOnlyTheTerritoryColumnStringInOrderToEnterItAsTheArgumentForMethodDeleteRowFromTable()
        {
            // Arrange
            string query = null;
            Mock<IDataBaseTable> mockDatabase = new Mock<IDataBaseTable>();
            mockDatabase.Setup(r => r.DeleteRowFromTable(It.IsAny<string>())).Callback<string>((s1) =>
            {
                query = s1;
            });
            ITerritoryAdministration sut = new TerritoryAdministration(territory, mockDatabase.Object);

            // Act
            sut.DeleteTerritory("TEST");

            //Assert
            Assert.AreEqual("TEST", query);
        }

        [TestMethod]
        public void ListingTheTerritoryAttributesInTheDatabaseShouldListThemAllAndShouldReturnTheListAsAgenericList()
        {
            // Arrange
            Mock<IDataBaseTable> mockDatabase = new Mock<IDataBaseTable>();
            mockDatabase.Setup(r => r.RetrieveNumberOfRowsInTable()).Returns(1);
            mockDatabase.Setup(r => r.TableName).Returns("Table Name");
            mockDatabase.Setup(r => r.RetrieveTableRow("Table Name", 1)).Returns("5\nBilly\nRed");
            ITerritoryAdministration sut = new TerritoryAdministration(territory, mockDatabase.Object);
            List<TriviaTerritory> test = new List<TriviaTerritory>();

            // Act
            test = sut.ListTerritories();
            string index = test[0].territoryIndex;
            string name = test[0].userName;
            string color = test[0].color;

            // Assert
            Assert.AreEqual("5", index);
            Assert.AreEqual("Billy", name);
            Assert.AreEqual("Red", color);
        }

        [TestMethod]
        public void AddingTerritoriesToAListThroughUseOfObjectAccessorsShouldReturnStringValueOfTerritory()
        {
            // Arrange
            Mock<ITriviaTerritory> mockUser = new Mock<ITriviaTerritory>();
            mockUser.Setup(r => r.territoryIndex).Returns("5");
            ITerritoryAdministration sut = new TerritoryAdministration(mockUser.Object, database);

            // Act
            List<string> test = (List<String>)sut.GetValues();

            // Assert
            Assert.AreEqual("5", test[0]);
        }
    }
}
