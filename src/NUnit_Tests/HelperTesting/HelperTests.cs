using NUnit.Framework;
using SteamProject.DAL.Abstract;
using SteamProject.DAL.Concrete;
using SteamProject.Models;
using SteamProject.Helpers;

namespace NUnit_Tests.HelperTesting;

[TestFixture]
public class HelperTests
{

    [Test]
    public void StripJunkFromString_WithProperString_Returns_True()
    {
        // Arrange
        var userString = HelperMethods.StripJunkFromString("This string is here");

        // Act

        // Assert
        Assert.True(userString != null);
    }

    [Test]
    public void StripJunkFromString_WithNullString_Returns_False()
    {
        // Arrange
        var userString = HelperMethods.StripJunkFromString(null);

        // Act

        // Assert
        Assert.True(userString == null);
    }

    [Test]
    public void StripJunkFromString_HtmlTags_RemovesTags()
    {
        // Arrange
        string input = "<h1>Hello</h1> <p>World!</p>";

        // Act
        string result = HelperMethods.StripJunkFromString(input);

        // Assert
        Assert.AreEqual("Hello World!", result);
    }

    [Test]
    public void StripJunkFromString_BracketTags_RemovesTags()
    {
        // Arrange
        string input = "Hello [tag]World![/tag]";

        // Act
        string result = HelperMethods.StripJunkFromString(input);

        // Assert
        Assert.AreEqual("Hello World!", result);
    }

    [Test]
    public void StripJunkFromString_Hyperlinks_RemovesLinks()
    {
        // Arrange
        string input = "Visit https://example.com and www.example2.com for more information.";

        // Act
        string result = HelperMethods.StripJunkFromString(input);

        // Assert
        Assert.AreEqual("Visit  and  for more information.", result);
    }

    [Test]
    public void StripJunkFromString_OtherJunk_RemovesJunk()
    {
        // Arrange
        string input = "Hello World!&quot;{STEAM_CLAN_IMAGE}/abc123/def456.jpg";

        // Act
        string result = HelperMethods.StripJunkFromString(input);

        // Assert
        Assert.AreEqual("Hello World!", result);
    }

    [Test]
    public void UnixTimeStampToDateTime_Zero_ReturnsEpochTime()
    {
        // Arrange
        int unixTimeStamp = 0;

        // Act
        DateTime result = HelperMethods.UnixTimeStampToDateTime(unixTimeStamp);

        // Assert
        Assert.AreEqual(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).ToLocalTime(), result);
    }

    [Test]
    public void UnixTimeStampToDateTime_Negative_ReturnsBeforeEpochTime()
    {
        // Arrange
        int unixTimeStamp = -86400;

        // Act
        DateTime result = HelperMethods.UnixTimeStampToDateTime(unixTimeStamp);

        // Assert
        Assert.AreEqual(new DateTime(1969, 12, 31, 0, 0, 0, DateTimeKind.Utc).ToLocalTime(), result);
    }

    [Test]
    public void UnixTimeStampToDateTime_Positive_ReturnsAfterEpochTime()
    {
        // Arrange
        int unixTimeStamp = 86400;

        // Act
        DateTime result = HelperMethods.UnixTimeStampToDateTime(unixTimeStamp);

        // Assert
        Assert.AreEqual(new DateTime(1970, 1, 2, 0, 0, 0, DateTimeKind.Utc).ToLocalTime(), result);
    }

    [Test]
    public void UnixTimeStampToDateTime_MaxValue_ReturnsMaxDateTime()
    {
        // Arrange
        int unixTimeStamp = int.MaxValue;

        // Act
        DateTime result = HelperMethods.UnixTimeStampToDateTime(unixTimeStamp);

        // Assert
        Assert.AreEqual(new DateTime(2038, 1, 19, 3, 14, 7, DateTimeKind.Utc).ToLocalTime(), result);
    }
}