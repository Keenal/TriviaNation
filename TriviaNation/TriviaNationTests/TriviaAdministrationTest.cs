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
        public void IfTheDatabaseHasANumberOfQuestionsInTheTableThenListingTheQuestionsShouldOutputTheCorrectNumberOfQuestionsToString()
        {
            // Arrange
            Mock<IDataBaseTable> mockDatabase = new Mock<IDataBaseTable>();
            mockDatabase.Setup(r => r.RetrieveNumberOfRowsInTable()).Returns(9);
            sut = new TriviaAdministration(question, mockDatabase.Object);

            // Act
            string test = sut.ListQuestions();

            // Assert
            Assert.AreEqual("1. 2. 3. 4. 5. 6. 7. 8. 9. ", test);
        }

        [TestMethod]
        public void ListingTheQuestionsInTheDatabaseShouldListThemAllAndShouldListTheirProperStringValuesInOrder()
        {
            // Arrange
            Mock<IDataBaseTable> mockDatabase = new Mock<IDataBaseTable>();
            mockDatabase.Setup(r => r.RetrieveNumberOfRowsInTable()).Returns(4);
            mockDatabase.Setup(r => r.TableName).Returns("Table Name");
            mockDatabase.Setup(r => r.RetrieveTableRow("Table Name", "1")).Returns("Testing row One ");
            mockDatabase.Setup(r => r.RetrieveTableRow("Table Name", "2")).Returns("Testing row Two ");
            mockDatabase.Setup(r => r.RetrieveTableRow("Table Name", "3")).Returns("Testing row Three ");
            mockDatabase.Setup(r => r.RetrieveTableRow("Table Name", "4")).Returns("Testing row Four");
            sut = new TriviaAdministration(question, mockDatabase.Object);

            // Act
            string testy = sut.ListQuestions();

            // Assert
            Assert.AreEqual("1. Testing row One 2. Testing row Two 3. Testing row Three 4. Testing row Four", testy);
        }

        [TestMethod]
        public void MethodForDeletingAQuestionShouldFirstRetrieveARowFromDatabaseThenAccessOnlyTheQuestionColumnStringInOrderToEnterItAsTheArgumentForMethodDeleteRowFromTable()
        {
            // Arrange
            string query = null;
            Mock<IDataBaseTable> mockDatabase = new Mock<IDataBaseTable>();
            mockDatabase.Setup(r => r.TableName).Returns("Table Name");
            mockDatabase.Setup(r => r.RetrieveTableRow("Table Name", "1")).Returns("This is the question? \n This is the answer.");
            mockDatabase.Setup(r => r.DeleteRowFromTable(It.IsAny<string>())).Callback<string>((s1) => 
            {
                query = s1;
            });
            sut = new TriviaAdministration(question, mockDatabase.Object);

            // Act
            sut.DeleteQuestion("1");

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
            Assert.AreEqual("This is a question\nThis is an answer\nQuestion Type\n", QT.RetrieveTableRow(QT.TableName, "1"));
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
            string test = admin.ListQuestions();

            // Act
            admin.DeleteQuestion("1");

            // Assert
            Assert.AreNotSame(admin.ListQuestions(), test);
            Assert.AreNotEqual(admin.ListQuestions(), test);
        }
    }
}
