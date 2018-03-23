using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace TriviaNation
{
    [TestClass]
    public class UserAdministrationTest
    {
        private IUser user;
        private IDataBaseTable database;
        private IUserAdministration admin;

        [TestInitialize]
        public void Initialize()
        {
            user = new User();
            database = new UserTable();
            admin = new UserAdministration(user, database);
        }

        [TestMethod]
        public void MethodAddUserShouldModifyTheFieldsOfTheClassItIsInAndThenPassItsOwnClassObjectAsAnArgumentToMethodInsertRowIntoTable()
        {
            // Arrange
            IDataEntry test = null;
            Mock<IDataBaseTable> mockDatabase = new Mock<IDataBaseTable>();
            mockDatabase.Setup(r => r.TableName).Returns("Table Name");
            mockDatabase.Setup(r => r.InsertRowIntoTable("Table Name", It.IsAny<IDataEntry>())).Callback<string, IDataEntry>((s1, s2) =>
            {
                test = s2;
            });
            IUserAdministration sut = new UserAdministration(user, mockDatabase.Object);

            // Act
            sut.AddUser("Bob", "lock@try.edu", "password", "password", "score");

            // Assert
            Assert.AreSame(sut, test);
            Assert.AreEqual(sut, test);
        }

        [TestMethod]
        public void MethodAddUserShouldReturnTrueIfPasswordAndConfirmPasswordMatch()
        {
            // Arrange
            Mock<IDataBaseTable> mockDatabase = new Mock<IDataBaseTable>();
            mockDatabase.Setup(r => r.TableName).Returns("Table Name");
            mockDatabase.Setup(r => r.InsertRowIntoTable("Table Name", It.IsAny<IDataEntry>()));
            IUserAdministration sut = new UserAdministration(user, mockDatabase.Object);

            // Act
            Boolean test = sut.AddUser("Bob", "lock@try.edu", "password", "password", "score");

            // Assert 
            Assert.AreEqual(true, test);
        }

        [TestMethod]
        public void MethodForDeletingAUserShouldFirstRetrieveARowFromDatabaseThenAccessOnlyTheUserColumnStringInOrderToEnterItAsTheArgumentForMethodDeleteRowFromTable()
        {
            // Arrange
            string query = null;
            Mock<IDataBaseTable> mockDatabase = new Mock<IDataBaseTable>();
            mockDatabase.Setup(r => r.TableName).Returns("Table Name");
            mockDatabase.Setup(r => r.RetrieveTableRow("Table Name", 1)).Returns("Huey \n jump@uwf.edu");
            mockDatabase.Setup(r => r.DeleteRowFromTable(It.IsAny<string>())).Callback<string>((s1) =>
            {
                query = s1;
            });
            IUserAdministration sut = new UserAdministration(user, mockDatabase.Object);

            // Act
            sut.DeleteUser(1);

            //Assert
            Assert.AreEqual("Huey ", query);
        }

        [TestMethod]
        public void ListingTheUsersInTheDatabaseShouldListThemAllAndShouldListTheirProperStringValuesInOrder()
        {
            // Arrange
            Mock<IDataBaseTable> mockDatabase = new Mock<IDataBaseTable>();
            mockDatabase.Setup(r => r.RetrieveNumberOfRowsInTable()).Returns(4);
            mockDatabase.Setup(r => r.TableName).Returns("Table Name");
            mockDatabase.Setup(r => r.RetrieveTableRow("Table Name", 1)).Returns("Billy ");
            mockDatabase.Setup(r => r.RetrieveTableRow("Table Name", 2)).Returns("TomTom ");
            mockDatabase.Setup(r => r.RetrieveTableRow("Table Name", 3)).Returns("Eernest ");
            mockDatabase.Setup(r => r.RetrieveTableRow("Table Name", 4)).Returns("Thor");
            IUserAdministration sut = new UserAdministration(user, mockDatabase.Object);

            // Act
            string test = sut.ListUsers();

            // Assert
            Assert.AreEqual("1. Billy 2. TomTom 3. Eernest 4. Thor", test);
        }

        [TestMethod]
        public void AddingUsersToAListThroughUseOfObjectAccessorsShouldReturnStringValueOfUser()
        {
            // Arrange
            Mock<IUser> mockUser = new Mock<IUser>();
            mockUser.Setup(r => r.UserName).Returns("Chillin");
            IUserAdministration sut = new UserAdministration(mockUser.Object, database);

            // Act
            List<string> test = (List<String>)sut.GetValues();

            // Assert
            Assert.AreEqual("Chillin", test[0]);
        }
    }
}
