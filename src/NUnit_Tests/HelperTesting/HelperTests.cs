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

    [Test]
    public void IsolateEmailURL_ForLocal_ReturnsExpected()
    {
        // Arrange
        string passedURL = "http://localhost:5105/Identity/Account/ConfirmEmail?userId=63279ee0-86ae-4d92-8d6f-c8d5605d11d9&amp;code=Q2ZESjhMMGhjY1pxem1sS2w0akhrZEtJeTBkUk1NeTBCbkpWQndENzV3QWNEME1aQVBRaTg5cDg4c25PWUhSNFRXY3NJWUltc2c2OFJTK2lobUNhZ2VzMDFVQUZVNUQyWmJ6RlBRalZJTUxsaVhOeWVDL1VobnI4cFRMcWFaU3Q0SUJCbW8vL1k0cW1kSE5VZFMvazVrRlJQTFJpZFR3cE53aUc4V05xN093NUUzbEtNWHpKa2hmVDNwbWtSREY2dDUzWkx3MkhYdXcvM0VBWnpHT1lHcUFkZ1RNeU8zRDNoV0U4bDBFRzVzSEg3Nnk1bTZqbmxjOVduOVZRYW1CaVVRQTV2dz09&amp;returnUrl=%2FIdentity%2FAccount%2FManage";
        string expectedURL = "http://localhost:5105/Identity/Account/ConfirmEmail?userId=63279ee0-86ae-4d92-8d6f-c8d5605d11d9&code=Q2ZESjhMMGhjY1pxem1sS2w0akhrZEtJeTBkUk1NeTBCbkpWQndENzV3QWNEME1aQVBRaTg5cDg4c25PWUhSNFRXY3NJWUltc2c2OFJTK2lobUNhZ2VzMDFVQUZVNUQyWmJ6RlBRalZJTUxsaVhOeWVDL1VobnI4cFRMcWFaU3Q0SUJCbW8vL1k0cW1kSE5VZFMvazVrRlJQTFJpZFR3cE53aUc4V05xN093NUUzbEtNWHpKa2hmVDNwbWtSREY2dDUzWkx3MkhYdXcvM0VBWnpHT1lHcUFkZ1RNeU8zRDNoV0U4bDBFRzVzSEg3Nnk1bTZqbmxjOVduOVZRYW1CaVVRQTV2dz09&returnUrl=%2FIdentity%2FAccount%2FManage";
        // Act
        passedURL = HelperMethods.FixEmailUrl(passedURL);
        // Assert
        Assert.AreEqual(expectedURL, passedURL);
    }
    [Test]
    public void IsolateEmailURL_ForDeployed_ReturnsExpected()
    {
        // Arrange
        string passedURL = "http://steamnexus.azurewebsites.net/Identity/Account/ConfirmEmail?userId=63279ee0-86ae-4d92-8d6f-c8d5605d11d9&amp;code=Q2ZESjhMMGhjY1pxem1sS2w0akhrZEtJeTBkUk1NeTBCbkpWQndENzV3QWNEME1aQVBRaTg5cDg4c25PWUhSNFRXY3NJWUltc2c2OFJTK2lobUNhZ2VzMDFVQUZVNUQyWmJ6RlBRalZJTUxsaVhOeWVDL1VobnI4cFRMcWFaU3Q0SUJCbW8vL1k0cW1kSE5VZFMvazVrRlJQTFJpZFR3cE53aUc4V05xN093NUUzbEtNWHpKa2hmVDNwbWtSREY2dDUzWkx3MkhYdXcvM0VBWnpHT1lHcUFkZ1RNeU8zRDNoV0U4bDBFRzVzSEg3Nnk1bTZqbmxjOVduOVZRYW1CaVVRQTV2dz09&amp;returnUrl=%2FIdentity%2FAccount%2FManage";
        string expectedURL = "http://steamnexus.azurewebsites.net/Identity/Account/ConfirmEmail?userId=63279ee0-86ae-4d92-8d6f-c8d5605d11d9&code=Q2ZESjhMMGhjY1pxem1sS2w0akhrZEtJeTBkUk1NeTBCbkpWQndENzV3QWNEME1aQVBRaTg5cDg4c25PWUhSNFRXY3NJWUltc2c2OFJTK2lobUNhZ2VzMDFVQUZVNUQyWmJ6RlBRalZJTUxsaVhOeWVDL1VobnI4cFRMcWFaU3Q0SUJCbW8vL1k0cW1kSE5VZFMvazVrRlJQTFJpZFR3cE53aUc4V05xN093NUUzbEtNWHpKa2hmVDNwbWtSREY2dDUzWkx3MkhYdXcvM0VBWnpHT1lHcUFkZ1RNeU8zRDNoV0U4bDBFRzVzSEg3Nnk1bTZqbmxjOVduOVZRYW1CaVVRQTV2dz09&returnUrl=%2FIdentity%2FAccount%2FManage";
        // Act
        passedURL = HelperMethods.FixEmailUrl(passedURL);
        // Assert
        Assert.AreEqual(expectedURL, passedURL);
    }

    [Test]
    public void IsolatePasswordResetHTMLContent_ForLocal_ReturnsExpected()
    {
        // Arrange
        string passedURL = "Please reset your password by <a href='http://localhost:5105/Identity/Account/ResetPassword?code=Q2ZESjhMMGhjY1pxem1sS2w0akhrZEtJeTBlak1hQ3dsb2doYWRYaXkxMmV0ZHRKZE1sY0RCQXp2Ynh4TDJ3dWQyK2FVWFlGN0d6UVJtcFhCVWEvUDcwQ3Zncm8xMy9QQ1h2ZHQvWHlxYmU3QzNWaDFhSThMa0dkRlFJN3FEdFJQMnVGWWhkelVFYnFINGVSTUtITmErdHJncXBFaFF3WXZlV0IxWWNTQlcra2NlbXhMcWg5SHJlTC9iZm9ZVEtMMDVQZFlKekJudCtVejFBWGpjOWZjQm11cURRUHBCbDlZOGJVRUNXbFMrYTlFV01K'>clicking here</a>.";
        string expectedURL = "http://localhost:5105/Identity/Account/ResetPassword?code=Q2ZESjhMMGhjY1pxem1sS2w0akhrZEtJeTBlak1hQ3dsb2doYWRYaXkxMmV0ZHRKZE1sY0RCQXp2Ynh4TDJ3dWQyK2FVWFlGN0d6UVJtcFhCVWEvUDcwQ3Zncm8xMy9QQ1h2ZHQvWHlxYmU3QzNWaDFhSThMa0dkRlFJN3FEdFJQMnVGWWhkelVFYnFINGVSTUtITmErdHJncXBFaFF3WXZlV0IxWWNTQlcra2NlbXhMcWg5SHJlTC9iZm9ZVEtMMDVQZFlKekJudCtVejFBWGpjOWZjQm11cURRUHBCbDlZOGJVRUNXbFMrYTlFV01K";
        // Act
        passedURL = HelperMethods.FixResetUrl(passedURL);
        // Assert
        Assert.AreEqual(expectedURL, passedURL);
    }
    [Test]
    public void IsolatePasswordResetHTMLContent_ForDeployed_ReturnsExpected()
    {
        // Arrange
        string passedURL = "Please reset your password by <a href='http://steamnexus.azurewebsites.net/Identity/Account/ResetPassword?code=Q2ZESjhMMGhjY1pxem1sS2w0akhrZEtJeTBlak1hQ3dsb2doYWRYaXkxMmV0ZHRKZE1sY0RCQXp2Ynh4TDJ3dWQyK2FVWFlGN0d6UVJtcFhCVWEvUDcwQ3Zncm8xMy9QQ1h2ZHQvWHlxYmU3QzNWaDFhSThMa0dkRlFJN3FEdFJQMnVGWWhkelVFYnFINGVSTUtITmErdHJncXBFaFF3WXZlV0IxWWNTQlcra2NlbXhMcWg5SHJlTC9iZm9ZVEtMMDVQZFlKekJudCtVejFBWGpjOWZjQm11cURRUHBCbDlZOGJVRUNXbFMrYTlFV01K'>clicking here</a>.";
        string expectedURL = "http://steamnexus.azurewebsites.net/Identity/Account/ResetPassword?code=Q2ZESjhMMGhjY1pxem1sS2w0akhrZEtJeTBlak1hQ3dsb2doYWRYaXkxMmV0ZHRKZE1sY0RCQXp2Ynh4TDJ3dWQyK2FVWFlGN0d6UVJtcFhCVWEvUDcwQ3Zncm8xMy9QQ1h2ZHQvWHlxYmU3QzNWaDFhSThMa0dkRlFJN3FEdFJQMnVGWWhkelVFYnFINGVSTUtITmErdHJncXBFaFF3WXZlV0IxWWNTQlcra2NlbXhMcWg5SHJlTC9iZm9ZVEtMMDVQZFlKekJudCtVejFBWGpjOWZjQm11cURRUHBCbDlZOGJVRUNXbFMrYTlFV01K";
        // Act
        passedURL = HelperMethods.FixResetUrl(passedURL);
        // Assert
        Assert.AreEqual(expectedURL, passedURL);
    }
}