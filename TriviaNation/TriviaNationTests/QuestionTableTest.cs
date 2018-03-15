using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TriviaNation;
using Moq;
using System.Data.SqlClient;

namespace TriviaNationTests
{
    [TestClass]
    public class QuestionTableTest
    {
        [TestMethod]
        public void RetrieveNumberOfRowsInTable_ShouldReturnTheNumberOfRows()
        {
            new DataBaseOperations();
            DataBaseOperations.ConnectToDB();
            SqlConnection s_connection = DataBaseOperations.Connection;

            var sut = new QuestionTable();

            int numberReturned = sut.RetrieveNumberOfRowsInTable();

            Assert.IsNotNull(numberReturned);
        }
    }
}
