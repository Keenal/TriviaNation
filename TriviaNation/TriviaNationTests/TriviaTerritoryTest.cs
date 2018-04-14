using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TriviaNation
{
    [TestClass]
    public class TriviaTerritoryTest
    { 
        [TestMethod]
        public void SettingATestTerritoryUserNameShouldReturnTerritoryUserName()
        {
            // Arrange
            ITriviaTerritory terr = new TriviaTerritory();
            {
                // Act
                terr.userName = "Josh";
            };
            // Assert
            Assert.AreEqual("Josh", terr.userName);
        }

        [TestMethod]
        public void SettingATestColorShouldReturnColor()
        {
            // Arrange
            ITriviaTerritory terr = new TriviaTerritory();
            {
                // Act
                terr.color = "red";
            };
            // Assert
            Assert.AreEqual("red", terr.color);
        }

        [TestMethod]
        public void SettingATerritoryIndexShouldReturnIndex()
        {
            // Arrange
            ITriviaTerritory terr = new TriviaTerritory();
            {
                // Act 
                terr.territoryIndex = "5F";
            };
            // Assert
            Assert.AreEqual("5F", terr.territoryIndex);
        }
    
    }
}
