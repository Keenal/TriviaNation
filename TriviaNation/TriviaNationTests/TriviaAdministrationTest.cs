using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace TriviaNation
{
    [TestClass]
    public class TriviaAdministrationTest
    {
        private IQuestion question;
        private IDataBaseTable QT;
        private ITriviaAdministration admin;
        private ITriviaAdministration sut;

        [TestInitialize]
        public void Initialize()
        {
            question = new Questions();
            QT = new QuestionTable();
            admin = new TriviaAdministration(question, QT);
            sut = null;
        }

        [TestMethod]
        public void AddingQuestionsToAListThroughUseOfObjectAccessorsShouldReturnStringValueOfQuestion()
        {
            // Arrange
            Mock<IQuestion> mockQuestion = new Mock<IQuestion>();
            mockQuestion.Setup(r => r.Question).Returns("This is a test?");
            sut = new TriviaAdministration(mockQuestion.Object, QT);

            // Act
            List<string> test = (List<String>)sut.GetValues();
      
            // Assert
            Assert.AreEqual("This is a test?", test[0]);
        }

        [TestMethod]
        public void AddingAnswersToAListThroughUseOfObjectAccessorsShouldReturnStringValueOfAnswer()
        {
            // Arrange
            Mock<IQuestion> mockAnswer = new Mock<IQuestion>();
            mockAnswer.Setup(r => r.Answer).Returns("This is the answer.");
            sut = new TriviaAdministration(mockAnswer.Object, QT);

            // Act
            List<string> test = (List<string>)sut.GetValues();

            // Assert
            Assert.AreEqual("This is the answer.", test[1]);
        }

        [TestMethod]

        public void IfTheDatabaseHasANumberOfQuestionsInTheTableThenListingTheQuestionsShouldFillAListWithTheCorrectNumberOfQuestions()
        {
            // Arrange
            Mock<IDataBaseTable> mockDatabase = new Mock<IDataBaseTable>();
            mockDatabase.Setup(r => r.RetrieveNumberOfRowsInTable()).Returns(1);
            mockDatabase.Setup(r => r.TableName).Returns("Table Name");
            mockDatabase.Setup(r => r.RetrieveTableRow("Table Name", 1)).Returns("This is the question?\nThis is the answer\nThis is the question type");
            sut = new TriviaAdministration(question, mockDatabase.Object);

            // Act
            List<IQuestion> test = (List<IQuestion>)sut.ListQuestions();

            // Assert
            Assert.AreEqual("This is the question?", test[0].Question);
            try
            {
                string t = test[1].Question;
                Assert.Fail(); // raises AssertionException
            }
            catch (Exception)
            {
                // Catches the assertion exception, and the test passes
            }
        }


        [TestMethod]
        public void ListingTheQuestionsInTheDatabaseShouldListThemAllAndShouldListTheirProperObjectStringValuesInOrder()
        {
            // Arrange
            Mock<IDataBaseTable> mockDatabase = new Mock<IDataBaseTable>();
            mockDatabase.Setup(r => r.RetrieveNumberOfRowsInTable()).Returns(4);
            mockDatabase.Setup(r => r.TableName).Returns("Table Name");
            mockDatabase.Setup(r => r.RetrieveTableRow("Table Name", 1)).Returns("Testing row One\n WithAnswer1\ntype");
            mockDatabase.Setup(r => r.RetrieveTableRow("Table Name", 2)).Returns("Testing row Two\nWithAnswer2\ntype");
            mockDatabase.Setup(r => r.RetrieveTableRow("Table Name", 3)).Returns("Testing row Three\nWithAnswer3\ntype");
            mockDatabase.Setup(r => r.RetrieveTableRow("Table Name", 4)).Returns("Testing row Four\nWithAnswer4\ntype");
            sut = new TriviaAdministration(question, mockDatabase.Object);

            // Act
            List<IQuestion> test = (List<IQuestion>) sut.ListQuestions();

            // Assert
            Assert.AreEqual("Testing row One WithAnswer1Testing row TwoWithAnswer2Testing row ThreeWithAnswer3Testing row FourWithAnswer4", test[0].Question + test[0].Answer + test[1].Question + test[1].Answer + test[2].Question + test[2].Answer + test[3].Question + test[3].Answer);
        }

        [TestMethod]
        public void MethodForDeletingAQuestionShouldFirstRetrieveARowFromDatabaseThroughPrivateMethodThenAccessOnlyTheQuestionColumnStringInOrderToEnterItAsTheArgumentForMethodDeleteRowFromTable()
        {
            // Arrange
            string query = null;
            Mock<IDataBaseTable> mockDatabase = new Mock<IDataBaseTable>();
            mockDatabase.Setup(r => r.TableName).Returns("Table Name");
            mockDatabase.Setup(r => r.RetrieveTableRow("Table Name", 1)).Returns("This is the question? \n This is the answer.\ntype");
            mockDatabase.Setup(r => r.DeleteRowFromTable(It.IsAny<string>())).Callback<string>((s1) => 
            {
                query = s1;
            });
            sut = new TriviaAdministration(question, mockDatabase.Object);

            // Act
            sut.DeleteQuestion(1);

            //Assert
            Assert.AreEqual("This is the question? ", query);
        }

        [TestMethod]
        public void MethodAddQuestionShouldModifyTheFieldsOfTheClassItIsInAndThenPassItsOwnClassObjectAsAnArgumentToMethodInsertARowIntoTable()
        {
            // Arrange
            IDataEntry test = null;
            Mock<IDataBaseTable> mockDatabase = new Mock<IDataBaseTable>();
            mockDatabase.Setup(r => r.TableName).Returns("Table Name");
            mockDatabase.Setup(r => r.InsertRowIntoTable("Table Name", It.IsAny<IDataEntry>())).Callback<string, IDataEntry>((s1, s2) =>
            {
                test = s2;
            });
            sut = new TriviaAdministration(question, mockDatabase.Object);

            // Act
            sut.AddQuestion("Question", "Answer", "Question Type");

            // Assert
            Assert.AreSame(sut, test);
            Assert.AreEqual(sut, test);
        }

        [TestMethod]
        public void RetrievingAQuestionForEditingShouldDeleteTheQuestionFromTheDatabaseAndReturnTheDeletedQuestionObject()
        {
            // Arrange
            string query = null;
            Mock<IDataBaseTable> mockDatabase = new Mock<IDataBaseTable>();
            mockDatabase.Setup(r => r.TableName).Returns("Table Name");
            mockDatabase.Setup(r => r.RetrieveTableRow("Table Name", 1)).Returns("This is the question? \n This is the answer.\ntype");
            mockDatabase.Setup(r => r.DeleteRowFromTable(It.IsAny<string>())).Callback<string>((s1) =>
            {
                query = s1;
            });
            sut = new TriviaAdministration(question, mockDatabase.Object);

            // Act
            IQuestion test = sut.GetEditableQuestion(1);

            //Assert
            Assert.AreEqual("This is the question? ", query);
            Assert.AreEqual("This is the question? ", test.Question);
        }

        // [Ignore]
        [TestMethod]
        // This is an integration test 
        public void InsertingAnEditedQuestionShouldOverwriteQuestionObjectWithNewInsertedQuestionStringAndReturnTheEditedQuestionInIntegrationTesting()
        {
            // Arrange
            Mock<IDataBaseTable> mockDatabase = new Mock<IDataBaseTable>();
            mockDatabase.Setup(r => r.TableName).Returns("Table Name");
            mockDatabase.Setup(r => r.RetrieveTableRow("Table Name", 1)).Returns("Question?\n This is the answer.\ntype");
            mockDatabase.Setup(r => r.DeleteRowFromTable(It.IsAny<string>()));
            mockDatabase.Setup(r => r.InsertRowIntoTable("Table Name", It.IsAny<IDataEntry>())).Verifiable(); ;
            IQuestion q = new Questions
            {
                Question = "Question?",
                Answer = "Answer",
                QuestionType = "T/F"
            };
            sut = new TriviaAdministration(question, mockDatabase.Object);

            // Act
            sut.AddQuestion("original question", "answer", "type");
            sut.InsertEditedQuestion(q);

            // Assert
            Assert.AreEqual("Question?", sut.GetEditableQuestion(1).Question);
        }

        [Ignore]
        [TestMethod]
        // This is an integration test
        public void AddingAQuestionToDatabaseTableShouldReturnThatQuestionFromTableInIntegrationTesting()
        {
            // Arrange
            new DataBaseOperations();
            DataBaseOperations.ConnectToDB();
            QT.CreateTable(QT.TableName, QT.TableCreationString);
            Console.WriteLine(QT.TableExists(QT.TableName));

            // Act
            admin.AddQuestion("This is a question", "This is an answer", "Question Type");

            // Assert
            Assert.AreEqual("This is a question\nThis is an answer\nQuestion Type\n", QT.RetrieveTableRow(QT.TableName, 1));
        }

        [Ignore]
        [TestMethod]
        // This is an integration test
        public void DeletingAQuestionFromDatabaseTableShouldRemoveThatQuestionFromTableInIntegrationTesting()
        {
            // Arrange
            new DataBaseOperations();
            DataBaseOperations.ConnectToDB();
            QT.CreateTable(QT.TableName, QT.TableCreationString);
            Console.WriteLine(QT.TableExists(QT.TableName));
            admin.AddQuestion("This is a question", "This is an answer", "Question Type");
            List<IQuestion> test = (List<IQuestion>)admin.ListQuestions();

            // Act
            admin.DeleteQuestion(1);

            // Assert
            if ("This is a question\nThis is an answer\nQuestion Type\n".Equals(QT.RetrieveTableRow(QT.TableName, 1)))
            {
                Assert.Fail();
            }
            Assert.AreNotEqual("This is a question\nThis is an answer\nQuestion Type\n", QT.RetrieveTableRow(QT.TableName, 1));
            Assert.AreNotSame(admin.ListQuestions(), test);
        }
    }
}
