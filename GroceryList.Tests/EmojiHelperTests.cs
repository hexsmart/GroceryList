using GroceryList.Helpers;

namespace GroceryList.Tests;

public class EmojiHelperTests
{
    [Fact]
    public void GetEmoji_ReturnsCorrectEmoji_ForKnownItem()
    {
        Assert.Equal("🥛", EmojiHelper.GetEmoji("Milk"));
    }

    [Fact]
    public void GetEmoji_ReturnsTrolley_ForUnknownItem()
    {
        Assert.Equal("🛒", EmojiHelper.GetEmoji("Xyz123"));
    }

    [Fact]
    public void GetEmoji_IsCaseInsensitive()
    {
        Assert.Equal(EmojiHelper.GetEmoji("milk"), EmojiHelper.GetEmoji("MILK"));
    }

    [Fact]
    public void GetCategory_ReturnsCorrectCategory_ForKnownItem()
    {
        Assert.Equal("Dairy", EmojiHelper.GetCategory("Milk"));
    }

    [Fact]
    public void GetCategory_ReturnsOther_ForUnknownItem()
    {
        Assert.Equal("Other", EmojiHelper.GetCategory("Xyz123"));
    }

    [Fact]
    public void GetCategory_IsCaseInsensitive()
    {
        Assert.Equal(EmojiHelper.GetCategory("milk"), EmojiHelper.GetCategory("MILK"));
    }

    [Fact]
    public void GetAllItems_ReturnsNonEmptyList()
    {
        Assert.NotEmpty(EmojiHelper.GetAllItems());
    }

    [Fact]
    public void GetAllItems_ReturnsAlphabetizedList()
    {
        var names = EmojiHelper.GetAllItems().Select(i => i.Name).ToList();
        Assert.Equal(names.OrderBy(n => n).ToList(), names);
    }

    [Fact]
    public void GetAllItems_EachItemHasNameEmojiAndCategory()
    {
        foreach (var item in EmojiHelper.GetAllItems())
        {
            Assert.False(string.IsNullOrWhiteSpace(item.Name));
            Assert.False(string.IsNullOrWhiteSpace(item.Emoji));
            Assert.False(string.IsNullOrWhiteSpace(item.Category));
        }
    }
}
