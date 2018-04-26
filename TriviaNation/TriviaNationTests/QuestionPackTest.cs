using System;
using System.Collections.Generic;
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


        [Ignore]
        [TestMethod]
        public void AddingQuestion()
        {
            
            // Arrange
            IDataEntry test = null;
            Mock<IDataBaseTable> mockDatabase = new Mock<IDataBaseTable>();
            Mock<IQuestion> mockQuestion = new Mock<IQuestion>();
            mockQuestion.Setup(r => r.Question).Returns("Question 1");
            mockQuestion.Setup(r => r.PointValue).Returns(4);
            mockDatabase.Setup(r => r.InsertRowIntoTable("Table Name", It.IsAny<IDataEntry>())).Callback<string, IDataEntry>((s1, s2) =>
            {
                test = s2;
            });

            IQuestionPack sut = new QuestionPack(mockQuestion.Object.QuestionPack, mockQuestion.Object.PointValue);

            // Act
            sut.AddQuestion("Question 1", "Answer 1", "Question Type");

            // Assert 
            Assert.AreSame(sut, test);
            Assert.AreEqual(sut, test);   
        }

        [TestMethod]
        public void DeletingAQuestionFromAQuestionPackShouldDeleteThatQuestionObjectFromTheList()
        {
            // Arrange
            string query = null;
            IQuestion question1 = new Questions();
            IQuestion question2 = new Questions();
            question1.Question = "Sally sells seashells?";
            question1.Question = "Jeremy jogged and jumped?";
            List<IQuestion> questions = new List<IQuestion>();
            questions.Add(question1);
            questions.Add(question2);
            QuestionPack q = new QuestionPack();
            q.QuestionPackQuestions = questions;
            Mock<IDataBaseTable> mockDatabase = new Mock<IDataBaseTable>();
            mockDatabase.Setup(r => r.DeleteRowFromTable(It.IsAny<string>())).Callback<string>((s1) =>
            {
                query = s1;
            });
            q.Database = mockDatabase.Object;
            String test = q.QuestionPackQuestions[0].Question;


            // Act
            q.DeleteQuestion(0);

            // Assert
            Assert.AreEqual("Jeremy jogged and jumped?", query);
            Assert.AreEqual("Jeremy jogged and jumped?", test);
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
