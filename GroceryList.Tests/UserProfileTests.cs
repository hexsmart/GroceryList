using GroceryList.Models;

namespace GroceryList.Tests;

public class UserProfileTests
{
    [Fact]
    public void NewUserProfile_HasNonEmptyGuid()
    {
        // Arrange & Act
        var userProfile = new UserProfile();

        // Assert
        Assert.NotEqual(Guid.Empty, userProfile.Id);
    }

    [Fact]
    public void NewUserProfile_DefaultsToEmptyFirstName()
    {
        // Arrange & Act
        var userProfile = new UserProfile();

        // Assert
        Assert.Equal(string.Empty, userProfile.FirstName);
    }

    [Fact]
    public void NewUserProfile_DefaultsToEmptyLastName()
    {
        // Arrange & Act
        var userProfile = new UserProfile();

        // Assert
        Assert.Equal(string.Empty, userProfile.LastName);
    }

    [Fact]
    public void NewUserProfile_DefaultsToEmptyEmail()
    {
        // Arrange & Act
        var userProfile = new UserProfile();

        // Assert
        Assert.Equal(string.Empty, userProfile.Email);
    }

    [Fact]
    public void UserProfile_CanSetFirstName()
    {
        // Arrange
        var userProfile = new UserProfile();
        var expectedFirstName = "John";

        // Act
        userProfile.FirstName = expectedFirstName;

        // Assert
        Assert.Equal(expectedFirstName, userProfile.FirstName);
    }

    [Fact]
    public void UserProfile_CanSetLastName()
    {
        // Arrange
        var userProfile = new UserProfile();
        var expectedLastName = "Doe";

        // Act
        userProfile.LastName = expectedLastName;

        // Assert
        Assert.Equal(expectedLastName, userProfile.LastName);
    }

    [Fact]
    public void UserProfile_CanSetEmail()
    {
        // Arrange
        var userProfile = new UserProfile();
        var expectedEmail = "john.doe@example.com";

        // Act
        userProfile.Email = expectedEmail;

        // Assert
        Assert.Equal(expectedEmail, userProfile.Email);
    }

    [Fact]
    public void UserProfile_CanSetId()
    {
        // Arrange
        var userProfile = new UserProfile();
        var expectedId = Guid.NewGuid();

        // Act
        userProfile.Id = expectedId;

        // Assert
        Assert.Equal(expectedId, userProfile.Id);
    }

    [Fact]
    public void UserProfile_EachInstanceHasUniqueId()
    {
        // Arrange & Act
        var userProfile1 = new UserProfile();
        var userProfile2 = new UserProfile();

        // Assert
        Assert.NotEqual(userProfile1.Id, userProfile2.Id);
    }

    [Fact]
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
        Assert.Equal(expectedFirstName, userProfile.FirstName);
        Assert.Equal(expectedLastName, userProfile.LastName);
        Assert.Equal(expectedEmail, userProfile.Email);
        Assert.NotEqual(Guid.Empty, userProfile.Id);
    }
}
