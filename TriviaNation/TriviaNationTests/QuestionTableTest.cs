using System;
using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TriviaNation;

namespace TriviaNationTests
{
    [TestClass]
    public class QuestionTableTest
    {
        private QuestionTable QT;
        private IDataEntry sut;

        [TestInitialize]
        public void Initialize() {
            QT = new QuestionTable();
            sut = null;
        }
        
        [TestMethod]
        public void TestTableExistsMethodToSeeIfAKnownTableExists()
        {
            // Arrange
            new DataBaseOperations();
            DataBaseOperations.ConnectToDB();
            SqlConnection s_connection = DataBaseOperations.Connection;
            var sut = new QuestionTable();

            // Act
            bool tableExists = sut.TableExists(sut.TableName);
            bool expected = true;
            bool actual = tableExists;

            //Assert
            Assert.AreEqual(expected, actual);
        }        

        [TestMethod]
        public void TestCreateTableMethodTableShouldGetCreated()
        {
            // Arrange
            new DataBaseOperations();
            DataBaseOperations.ConnectToDB();
            SqlConnection s_connection = DataBaseOperations.Connection;
            String sqlString = "SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'QTTestTable2'";
            int count = 0;
            String nameOfTestTable = "QTTestTable2";
            String tableCreationString = "(columnone varchar(4000) not null PRIMARY KEY, columntwo varchar(4000) not null);";

            // Act
            QT.CreateTable(nameOfTestTable, tableCreationString);
            SqlCommand command = new SqlCommand(sqlString, s_connection);
            SqlDataReader myReader = command.ExecuteReader();
            while (myReader.Read())
            {
                count++;
            }

            //Assert
            Assert.AreEqual(1, count);
        }

        /*
        
        [TestMethod]
        public void TestInsertRowIntoTableMethodToSeeIfRowGetsInserted() {
            // Arrange
            Mock<IDataEntry> mockData = new Mock<IDataEntry>();
            mockData.Setup(r => r.GetValues).Returns("This is a test?");
            sut = new QuestionTable();

            // Act
         //   QT.InsertRowIntoTable(mockData);
            List<string> test = (List<String>)sut.GetValues();

            // Assert
            Assert.AreEqual("This is question1" + "\n" + "This is answer1" + "\n", list[0]);

        }
        
    */

        [TestMethod]
        public void TestRetrieveNumberOfRowsInTableMethodShouldReturnIntNotNUll()
        {
            //Arrange
            new DataBaseOperations();
            DataBaseOperations.ConnectToDB();
            SqlConnection s_connection = DataBaseOperations.Connection;
            var sut = new QuestionTable();

            //Act
            int numberReturned = sut.RetrieveNumberOfRowsInTable();

            //Assert
            Assert.IsNotNull(numberReturned);
        }

        [TestMethod]
        public void TestRetrieveRowInTableMethodShouldReturnTheRowFromSpecificRowNumber()
        {
            // Arrange
            new DataBaseOperations();
            DataBaseOperations.ConnectToDB();
            SqlConnection s_connection = DataBaseOperations.Connection;
            var sut = new QuestionTable();
            int rowNumber = 1;

            // Act
            String rowRetrieved = sut.RetrieveTableRow(rowNumber);

            // Assert
            Assert.AreEqual("This is question2" + "\n" + "This is answer2" + "\n", rowRetrieved);
        }



        [TestMethod]
        public void TestRetrieveNumberOfColsInTableMethodShouldReturnIntNotNUll()
        {
            //Arrange
            new DataBaseOperations();
            DataBaseOperations.ConnectToDB();
            SqlConnection s_connection = DataBaseOperations.Connection;
            var sut = new QuestionTable();

            //Act
            int numberReturned = sut.RetriveNumberOfColsInTable();

            //Assert
            Assert.IsNotNull(numberReturned);
        }

        
        [TestMethod]
        public void TestToSeeIfRowIsDeleted()
        {
            // Arrange
            new DataBaseOperations();
            DataBaseOperations.ConnectToDB();
            SqlConnection s_connection = DataBaseOperations.Connection;
            int count = 1;

            var sut = new QuestionTable();
            String questionString = "This is question1";
            String sqlString = "DELETE FROM QuestionTable WHERE question='" + questionString + "';";

            // Act
            QT.DeleteRowFromTable(questionString);
            SqlCommand command = new SqlCommand(sqlString, s_connection);
            SqlDataReader myReader = command.ExecuteReader();
            while (myReader.Read())
            {
                count++;
            }

            // Assert
            Assert.AreEqual(1, count);


        }
        

        public void CleanUpAfterTests()
        {
            new DataBaseOperations();
            DataBaseOperations.ConnectToDB();
            SqlConnection s_connection = DataBaseOperations.Connection;

            String DropTableSQLCode1 = ("DROP TABLE IF EXISTS TestTable1;");
            SqlCommand deleteTableCommand1 = new SqlCommand(DropTableSQLCode1, s_connection);
            deleteTableCommand1.ExecuteNonQuery();
        }

        
    }
}