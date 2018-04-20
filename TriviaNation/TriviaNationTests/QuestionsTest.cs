using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TriviaNation
{
    [TestClass]
    public class QuestionsTest
    {
        [TestMethod]
        public void SettingATestQuestionShouldReturnQuestion()
        {
            // Arrange
            Questions question = new Questions();

            // Act
            question.Question = "Testing Working?";
            
            // Assert
            Assert.AreEqual("Testing Working?", question.Question);
        }

        [TestMethod]
        public void SettingATestAnswerShouldReturnAnswer()
        {
            // Arrange
            Questions answer = new Questions();

            // Act
            answer.Answer = "Testing is working.";
            
            // Assert
            Assert.AreEqual("Testing is working.", answer.Answer);
        }

        [TestMethod]
        public void SettingAPointValueShouldReturnPointValue()
        {
            // Arrange
            Questions pointValue = new Questions
            {
                // Act 
                PointValue = 121
            };
            // Assert
            Assert.AreEqual(121, pointValue.PointValue);
        }
    }
}
