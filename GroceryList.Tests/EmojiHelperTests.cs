using GroceryList.Helpers;

namespace GroceryList.Tests;

[TestClass]
public class EmojiHelperTests
{
    [TestMethod]
    public void GetEmoji_ReturnsCorrectEmoji_ForKnownItem()
    {
        Assert.AreEqual("🥛", EmojiHelper.GetEmoji("Milk"));
    }

    [TestMethod]
    public void GetEmoji_ReturnsTrolley_ForUnknownItem()
    {
        Assert.AreEqual("🛒", EmojiHelper.GetEmoji("Xyz123"));
    }

    [TestMethod]
    public void GetEmoji_IsCaseInsensitive()
    {
        Assert.AreEqual(EmojiHelper.GetEmoji("milk"), EmojiHelper.GetEmoji("MILK"));
    }

    [TestMethod]
    public void GetCategory_ReturnsCorrectCategory_ForKnownItem()
    {
        Assert.AreEqual("Dairy", EmojiHelper.GetCategory("Milk"));
    }

    [TestMethod]
    public void GetCategory_ReturnsOther_ForUnknownItem()
    {
        Assert.AreEqual("Other", EmojiHelper.GetCategory("Xyz123"));
    }

    [TestMethod]
    public void GetCategory_IsCaseInsensitive()
    {
        Assert.AreEqual(EmojiHelper.GetCategory("milk"), EmojiHelper.GetCategory("MILK"));
    }

    [TestMethod]
    public void GetAllItems_ReturnsNonEmptyList()
    {
        Assert.IsTrue(EmojiHelper.GetAllItems().Any());
    }

    [TestMethod]
    public void GetAllItems_ReturnsAlphabetizedList()
    {
        var names = EmojiHelper.GetAllItems().Select(i => i.Name).ToList();
        CollectionAssert.AreEqual(names.OrderBy(n => n).ToList(), names);
    }

    [TestMethod]
    public void GetAllItems_EachItemHasNameEmojiAndCategory()
    {
        foreach (var item in EmojiHelper.GetAllItems())
        {
            Assert.IsFalse(string.IsNullOrWhiteSpace(item.Name));
            Assert.IsFalse(string.IsNullOrWhiteSpace(item.Emoji));
            Assert.IsFalse(string.IsNullOrWhiteSpace(item.Category));
        }
    }
}
