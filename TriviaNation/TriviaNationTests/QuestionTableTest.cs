using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TriviaNationTests
{
    [TestClass]
    public class QuestionTableTest
    {

        private String tableName;
        private String tableCreationString;

        [TestInitialize]
        public void Initialize() {
            tableName = "QuestionTable";
            tableCreationString = "(question varchar(4000) not null PRIMARY KEY, answer varchar(4000) not null);";
        }
        [TestMethod]
        public void TestToSeeIfTableExists()
        {


        }
    }
}
