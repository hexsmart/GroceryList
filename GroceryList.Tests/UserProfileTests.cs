using GroceryList.Models;

namespace GroceryList.Tests;

[TestClass]
public class UserProfileTests
{
    [TestMethod]
    public void UserProfile_Email_AcceptsEmptyString()
    {
        // Arrange
        var userProfile = new UserProfile();

        // Act & Assert - empty/whitespace should not throw
        userProfile.Email = "";
        Assert.AreEqual(string.Empty, userProfile.Email);
    }

    [TestMethod]
    public void UserProfile_Email_AcceptsValidFormat()
    {
        // Arrange
        var userProfile = new UserProfile();

        // Act & Assert - should not throw
        userProfile.Email = "valid.email@example.com";
        Assert.AreEqual("valid.email@example.com", userProfile.Email);
    }

    [TestMethod]
    public void UserProfile_Email_CanSetValidValue()
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
    public void UserProfile_Email_ConvertsNullToEmpty()
    {
        // Arrange
        var userProfile = new UserProfile { Email = "test@example.com" };

        // Act
        userProfile.Email = null!;

        // Assert
        Assert.AreEqual(string.Empty, userProfile.Email);
    }

    [TestMethod]
    public void UserProfile_Email_DefaultsToEmpty()
    {
        // Arrange & Act
        var userProfile = new UserProfile();

        // Assert
        Assert.AreEqual(string.Empty, userProfile.Email);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void UserProfile_Email_ThrowsOnInvalidFormat()
    {
        // Arrange
        var userProfile = new UserProfile();

        // Act - should throw ArgumentException
        userProfile.Email = "invalid-email";
    }

    [TestMethod]
    public void UserProfile_FirstName_CanSetValue()
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
    public void UserProfile_FirstName_ConvertsNullToEmpty()
    {
        // Arrange
        var userProfile = new UserProfile { FirstName = "John" };

        // Act
        userProfile.FirstName = null!;

        // Assert
        Assert.AreEqual(string.Empty, userProfile.FirstName);
    }

    [TestMethod]
    public void UserProfile_FirstName_DefaultsToEmpty()
    {
        // Arrange & Act
        var userProfile = new UserProfile();

        // Assert
        Assert.AreEqual(string.Empty, userProfile.FirstName);
    }

    [TestMethod]
    public void UserProfile_Id_CanSetValue()
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
    public void UserProfile_Id_IsNonEmptyGuid()
    {
        // Arrange & Act
        var userProfile = new UserProfile();

        // Assert
        Assert.AreNotEqual(Guid.Empty, userProfile.Id);
    }

    [TestMethod]
    public void UserProfile_Id_IsUniquePerInstance()
    {
        // Arrange & Act
        var userProfile1 = new UserProfile();
        var userProfile2 = new UserProfile();

        // Assert
        Assert.AreNotEqual(userProfile1.Id, userProfile2.Id);
    }

    [TestMethod]
    public void UserProfile_LastName_CanSetValue()
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
    public void UserProfile_LastName_ConvertsNullToEmpty()
    {
        // Arrange
        var userProfile = new UserProfile { LastName = "Doe" };

        // Act
        userProfile.LastName = null!;

        // Assert
        Assert.AreEqual(string.Empty, userProfile.LastName);
    }

    [TestMethod]
    public void UserProfile_LastName_DefaultsToEmpty()
    {
        // Arrange & Act
        var userProfile = new UserProfile();

        // Assert
        Assert.AreEqual(string.Empty, userProfile.LastName);
    }

    [TestMethod]
    public void UserProfile_ObjectInitializer_SetsAllProperties()
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
