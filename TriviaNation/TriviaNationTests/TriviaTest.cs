using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace TriviaNation
{
    [TestClass]
    public class TriviaTest
    {
        private IQuestion question;
        private IDataBaseTable database;
        private ITrivia sut;

        [TestInitialize]
        public void Initialize()
        {
            question = new Questions();
            database = new QuestionTable();
            sut = null;
        }

        [TestMethod]
        public void AquiringARandomIntegerShouldReturnNumbersBetweenOneAndTenInclusiveWhenTheNumberOfRowsInATableAreTen()
        {
            // Arrange
            Mock<IDataBaseTable> mockDatabase = new Mock<IDataBaseTable>();
            mockDatabase.Setup(r => r.RetrieveNumberOfRowsInTable()).Returns(10);
            sut = new Trivia(mockDatabase.Object, question);
            int i = 0;
            Boolean flag;
            
            // Act
            while (i < 1000)
            {
                int test = sut.RandomGenerator();

                // Assert
                Assert.AreNotEqual(0, test);
                Assert.AreNotEqual(11, test);

                if (test <= 0 || test >= 11)                
                    Assert.Fail();
                if (test >= 1 && test <= 10)
                { 
                    flag = true;
                    Assert.IsTrue(flag);
                }

                i++;
            }
        }

        [TestMethod]
        public void AquiringARandomQuestionShouldReturnARandomTableRowFromDatabaseAndExtractOnlyTheQuestionFromTheRow()
        {
            // Arrange
            Mock<IDataBaseTable> mockDatabase = new Mock<IDataBaseTable>();
            mockDatabase.Setup(r => r.RetrieveNumberOfRowsInTable()).Returns(5);
            mockDatabase.Setup(r => r.TableName).Returns("Table Name");
            mockDatabase.Setup(r => r.RetrieveTableRow("Table Name", 1)).Returns("This is the first row.\n And this is its answer\n Question Type");
            mockDatabase.Setup(r => r.RetrieveTableRow("Table Name", 2)).Returns("This is the second row.\n And this is its answer\n Question Type");
            mockDatabase.Setup(r => r.RetrieveTableRow("Table Name", 3)).Returns("This is the third row.\n And this is its answer\n Question Type");
            mockDatabase.Setup(r => r.RetrieveTableRow("Table Name", 4)).Returns("This is the fourth row.\n And this is its answer\n Question Type");
            mockDatabase.Setup(r => r.RetrieveTableRow("Table Name", 5)).Returns("This is the fifth row.\n And this is its answer\n Question Type");
            sut = new Trivia(mockDatabase.Object, question);
            int i = 0;

            // Act
            while (i <= 5)
            {
                IQuestion test = sut.GetRandomQuestion();
                // Assert
                if (test.Question.Equals("This is the first row."))
                    Assert.AreEqual("This is the first row.", test.Question);
                else if (test.Question.Equals("This is the second row."))
                    Assert.AreEqual("This is the second row.", test.Question);
                else if (test.Question.Equals("This is the third row."))
                    Assert.AreEqual("This is the third row.", test.Question);
                else if (test.Question.Equals("This is the fourth row."))
                    Assert.AreEqual("This is the fourth row.", test.Question);
                else if (test.Question.Equals("This is the fifth row."))
                    Assert.AreEqual("This is the fifth row.", test.Question);
                else
                    Assert.Fail();
                i++;
            }

        }

        [TestMethod]
        public void AnswerEvaluationShouldReturnTrueIfTheAnswerGivenMatchesTheActualAnswersStringValue()
        {
            // Arrange
            string answerGiven = "False";
            Mock<IQuestion> mockAnswer = new Mock<IQuestion>();
            mockAnswer.Setup(r => r.Answer).Returns("False");
            sut = new Trivia(database, mockAnswer.Object);

            // Act
            Boolean test = sut.EvaluateAnswer(answerGiven);

            // Assert
            Assert.IsTrue(test);
        }

        [TestMethod]
        public void AnswerEvaluationShouldReturnFalseIfTheAnswerGivenDoesNotMatchTheActualAnswersStringValue()
        {
            // Arrange
            string answerGiven = "True";
            Mock<IQuestion> mockAnswer = new Mock<IQuestion>();
            mockAnswer.Setup(r => r.Answer).Returns("False");
            sut = new Trivia(database, mockAnswer.Object);

            // Act
            Boolean test = sut.EvaluateAnswer(answerGiven);

            // Assert
            Assert.IsFalse(test);
        }

        [TestMethod]
        public void AnswerEvaluationShouldMatchTheActualAnswerEvenIfTheAnswerGivenHasADifferentCaseOrHasExtraWhiteSpace()
        {
            // Arrange
            string answerGiven = "  fAlSe  ";
            Mock<IQuestion> mockAnswer = new Mock<IQuestion>();
            mockAnswer.Setup(r => r.Answer).Returns("False");
            sut = new Trivia(database, mockAnswer.Object);

            // Act
            Boolean test = sut.EvaluateAnswer(answerGiven);

            // Assert
            Assert.IsTrue(test);
        }
    }
}
