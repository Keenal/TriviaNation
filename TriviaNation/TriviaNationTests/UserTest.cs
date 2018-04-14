using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TriviaNation
{
    [TestClass]
    public class UserTest
    {
        [TestMethod]
        public void SettingATestUserShouldReturnUser()
        {
            // Arrange
            IUser user = new User();
            {
                // Act
                user.UserName = "Testing Working?";
            };
            // Assert
            Assert.AreEqual("Testing Working?", user.UserName);
        }

        [TestMethod]
        public void SettingATestEmailShouldReturnEmail()
        {
            // Arrange
            IUser user = new User();
            {
                // Act
                user.Email = "working@uwf.edu";
            };
            // Assert
            Assert.AreEqual("working@uwf.edu", user.Email);
        }

        [TestMethod]
        public void SettingAPasswordShouldReturnPassword()
        {
            // Arrange
            IUser user = new User();
            {
                // Act 
                user.Password = "password";
            };
            // Assert
            Assert.AreEqual("password", user.Password);
        }

        [TestMethod]
        public void SettingAScoreShouldReturnScore()
        {
            // Arrange
            IUser user = new User();
            {
                // Act 
                user.Score = "104";
            };
            // Assert
            Assert.AreEqual("104", user.Score);
        }
    }

}
