using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TriviaNation.Models;

namespace TriviaNationTests
{
    [TestClass]
    public class QuestionPackTest
    {
        [TestMethod]
        public void SettingAQuestionPackShouldReturnQuestionPackName()
        {
            // Arrange
            QuestionPack questionPackName = new QuestionPack();

            // Act
            questionPackName.QuestionPackName = "Testing working?";

            // Assert
            Assert.AreEqual("Testing working?", questionPackName.QuestionPackName);
        }

        [TestMethod]
        public void SettingAPointValueShouldReturnPointValue()
        {
            // Arrange
            QuestionPack pointValue = new QuestionPack
            {
                // Act 
                PointValue = 121
            };
            // Assert
            Assert.AreEqual(121, pointValue.PointValue);
        }
    }
}
