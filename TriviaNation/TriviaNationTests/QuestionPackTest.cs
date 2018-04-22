using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TriviaNation;
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

        [Ignore]
        [TestMethod]
        public void SettingADatabaseShouldReturnDatabase()
        {
            // Arrange
            QuestionPack database = new QuestionPack
            {
                // Act
                Database = 134
            };

            // Assert
            Assert.AreEqual(134, database.Database);
        }

        [Ignore]
        [TestMethod]
        public void AddingQuestion()
        {
            // Arrange
            Mock<IQuestion> mockQuestion = new Mock<IQuestion>();
            mockQuestion.Setup(r => r.Question).Returns("This is a test?");
            //   sut = new TriviaAdministration(mockQuestion.Object, QT);

            // Act
            //   List<string> test = (List<String>)sut.GetValues();

            // Assert
            //      Assert.AreEqual("This is a test?", test[0]);
        }

        [Ignore]
        [TestMethod]
        public void MethodForDeletingAQuestionShouldFirstRetrieveARowFromDatabaseThroughPrivateMethodThenAccessOnlyTheQuestionColumnStringInOrderToEnterItAsTheArgumentForMethodDeleteRowFromTable()
        {
            // Arrange
            string query = null;
            Mock<IQuestion> mockDatabase = new Mock<IQuestion>();
            mockDatabase.Setup(r => r.TableName).Returns("Table Name");
            mockDatabase.Setup(r => r.RetrieveTableRow("Table Name", 1)).Returns("This is the question? \n This is the answer.\ntype");
            mockDatabase.Setup(r => r.DeleteRowFromTable(It.IsAny<string>())).Callback<string>((s1) =>
            {
                query = s1;
            });
            //   sut = new TriviaAdministration(question, mockDatabase.Object);

            // Act
            //   sut.DeleteQuestion(1);

            //Assert
            Assert.AreEqual("This is the question? ", query);
        }
    }
}
