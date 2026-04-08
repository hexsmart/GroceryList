using GroceryList.Helpers;

namespace GroceryList.Tests;

[TestClass]
public class EmojiHelperTests
{
    [TestMethod]
    public void EmojiHelper_GetAllItems_EachItemHasNameEmojiAndCategory()
    {
        foreach (var item in EmojiHelper.GetAllItems())
        {
            Assert.IsFalse(string.IsNullOrWhiteSpace(item.Name));
            Assert.IsFalse(string.IsNullOrWhiteSpace(item.Emoji));
            Assert.IsFalse(string.IsNullOrWhiteSpace(item.Category));
        }
    }

    [TestMethod]
    public void EmojiHelper_GetAllItems_ReturnsAlphabetizedList()
    {
        var names = EmojiHelper.GetAllItems().Select(i => i.Name).ToList();
        CollectionAssert.AreEqual(names.OrderBy(n => n).ToList(), names);
    }

    [TestMethod]
    public void EmojiHelper_GetAllItems_ReturnsNonEmptyList()
    {
        Assert.IsTrue(EmojiHelper.GetAllItems().Any());
    }

    [TestMethod]
    public void EmojiHelper_GetCategory_IsCaseInsensitive()
    {
        Assert.AreEqual(EmojiHelper.GetCategory("milk"), EmojiHelper.GetCategory("MILK"));
    }

    [TestMethod]
    public void EmojiHelper_GetCategory_ReturnsCorrectCategoryForKnownItem()
    {
        Assert.AreEqual("Dairy", EmojiHelper.GetCategory("Milk"));
    }

    [TestMethod]
    public void EmojiHelper_GetCategory_ReturnsOtherForUnknownItem()
    {
        Assert.AreEqual("Other", EmojiHelper.GetCategory("Xyz123"));
    }

    [TestMethod]
    public void EmojiHelper_GetEmoji_IsCaseInsensitive()
    {
        Assert.AreEqual(EmojiHelper.GetEmoji("milk"), EmojiHelper.GetEmoji("MILK"));
    }

    [TestMethod]
    public void EmojiHelper_GetEmoji_ReturnsCorrectEmojiForKnownItem()
    {
        Assert.AreEqual("🥛", EmojiHelper.GetEmoji("Milk"));
    }

    [TestMethod]
    public void EmojiHelper_GetEmoji_ReturnsTrolleyForUnknownItem()
    {
        Assert.AreEqual("🛒", EmojiHelper.GetEmoji("Xyz123"));
    }
}
