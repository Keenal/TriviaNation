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

        [TestInitialize]
        public void Initialize() {
            QT = new QuestionTable();
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
            Mock<DataBaseOperations> dbo = new Mock<DataBaseOperations>();
            dbo.Setup(r => r.InsertIntoTable(insertString)).Returns("Insertion complete!");

            // Act
            DataBaseOperations obj = new DataBaseOperations();

            // Assert
            Assert.AreEqual(obj.InsertIntoTable(dbo.Object), "Insertion complete!");
            
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

        /*
        [TestMethod]
        public void TestToSeeIfRowIsRetrieved()
        {
            // Arrange
            new DataBaseOperations();
            DataBaseOperations.ConnectToDB();
            SqlConnection s_connection = DataBaseOperations.Connection;
            String sqlString = ("SELECT * " +
                "FROM(SELECT Row_Number()" +
                "OVER(ORDER BY question)" +
                "AS RowNum, *" +
                "FROM QuestionTable) t2" +
                "WHERE RowNum = 1;");
            int rowNumber = 1;
          

            // Act
            String retrievedRow = QT.RetrieveTableRow(rowNumber);
            
            using (SqlCommand command = new SqlCommand(sqlString, s_connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        retrievedRow += reader.GetString(1) + "\n" + reader.GetString(2) + "\n";
                    }
                }
            }
            

            // Assert
            Assert.AreEqual(("This is question2" + "\n" + "This is answer2"), retrievedRow);



        }
        */
        
       
        

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

        /*
        [TestMethod]
        public void TestToSeeIfRowIsDeleted()
        {

        }
        */

        public void CleanUpAfterTests()
        {
            new DataBaseOperations();
            DataBaseOperations.ConnectToDB();
            SqlConnection s_connection = DataBaseOperations.Connection;

            String DropTableSQLCode1 = ("DROP TABLE IF EXISTS TestTable1;");
            SqlCommand deleteTableCommand1 = new SqlCommand(DropTableSQLCode1, s_connection);
            deleteTableCommand1.ExecuteNonQuery();
        }

        [TestMethod]
        public void TestRetrieveRow() {
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
    }
}