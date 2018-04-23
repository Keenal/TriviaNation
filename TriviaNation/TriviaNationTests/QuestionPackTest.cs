using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TriviaNation;
using TriviaNation.Models;
using TriviaNation.Models.Abstract;

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

        /*
        
        */

     //   [Ignore]
        [TestMethod]
        public void AddingQuestion()
        {
            
            // Arrange
            IDataEntry test = null;
            Mock<IQuestion> mockDatabase = new Mock<IQuestion>();
            mockDatabase.Setup(r => r.Question).Returns("Question 1");
            mockDatabase.Setup(r => r.PointValue).Returns(4);

            IQuestionPack sut = new QuestionPack(mockDatabase.Object.QuestionPack, mockDatabase.Object.PointValue);

            // Act
            sut.AddQuestion("Question 1", "Answer 1", "Question Type");

            // Assert 
            Assert.AreSame(sut, test);
            Assert.AreEqual(sut, test);
            
        }

        /*
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
            

    */
    
    
    }
}
