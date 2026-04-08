using GroceryList.Models;

namespace GroceryList.Tests;

[TestClass]
public class UserProfileTests
{
    [TestMethod]
    public void NewUserProfile_HasNonEmptyGuid()
    {
        // Arrange & Act
        var userProfile = new UserProfile();

        // Assert
        Assert.AreNotEqual(Guid.Empty, userProfile.Id);
    }

    [TestMethod]
    public void NewUserProfile_DefaultsToEmptyFirstName()
    {
        // Arrange & Act
        var userProfile = new UserProfile();

        // Assert
        Assert.AreEqual(string.Empty, userProfile.FirstName);
    }

    [TestMethod]
    public void NewUserProfile_DefaultsToEmptyLastName()
    {
        // Arrange & Act
        var userProfile = new UserProfile();

        // Assert
        Assert.AreEqual(string.Empty, userProfile.LastName);
    }

    [TestMethod]
    public void NewUserProfile_DefaultsToEmptyEmail()
    {
        // Arrange & Act
        var userProfile = new UserProfile();

        // Assert
        Assert.AreEqual(string.Empty, userProfile.Email);
    }

    [TestMethod]
    public void UserProfile_CanSetFirstName()
    {
        // Arrange
        var userProfile = new UserProfile();
        var expectedFirstName = "John";

        // Act
        userProfile.FirstName = expectedFirstName;

        // Assert
        Assert.AreEqual(expectedFirstName, userProfile.FirstName);
    }

    [TestMethod]
    public void UserProfile_CanSetLastName()
    {
        // Arrange
        var userProfile = new UserProfile();
        var expectedLastName = "Doe";

        // Act
        userProfile.LastName = expectedLastName;

        // Assert
        Assert.AreEqual(expectedLastName, userProfile.LastName);
    }

    [TestMethod]
    public void UserProfile_CanSetEmail()
    {
        // Arrange
        var userProfile = new UserProfile();
        var expectedEmail = "john.doe@example.com";

        // Act
        userProfile.Email = expectedEmail;

        // Assert
        Assert.AreEqual(expectedEmail, userProfile.Email);
    }

    [TestMethod]
    public void UserProfile_CanSetId()
    {
        // Arrange
        var userProfile = new UserProfile();
        var expectedId = Guid.NewGuid();

        // Act
        userProfile.Id = expectedId;

        // Assert
        Assert.AreEqual(expectedId, userProfile.Id);
    }

    [TestMethod]
    public void UserProfile_EachInstanceHasUniqueId()
    {
        // Arrange & Act
        var userProfile1 = new UserProfile();
        var userProfile2 = new UserProfile();

        // Assert
        Assert.AreNotEqual(userProfile1.Id, userProfile2.Id);
    }

    [TestMethod]
    public void UserProfile_CanCreateWithObjectInitializer()
    {
        // Arrange
        var expectedFirstName = "Jane";
        var expectedLastName = "Smith";
        var expectedEmail = "jane.smith@example.com";

        // Act
        var userProfile = new UserProfile
        {
            FirstName = expectedFirstName,
            LastName = expectedLastName,
            Email = expectedEmail
        };

        // Assert
        Assert.AreEqual(expectedFirstName, userProfile.FirstName);
        Assert.AreEqual(expectedLastName, userProfile.LastName);
        Assert.AreEqual(expectedEmail, userProfile.Email);
        Assert.AreNotEqual(Guid.Empty, userProfile.Id);
    }
}
