namespace GroceryList.Helpers;

public static class EmojiHelper
{
    private static readonly Dictionary<string, string> _map = new(StringComparer.OrdinalIgnoreCase)
    {
        // Dairy
        { "milk", "🥛" }, { "cheese", "🧀" }, { "butter", "🧈" }, { "yogurt", "🥛" }, { "cream", "🥛" }, { "eggs", "🥚" }, { "egg", "🥚" },
        // Produce
        { "apple", "🍎" }, { "apples", "🍎" }, { "banana", "🍌" }, { "bananas", "🍌" }, { "orange", "🍊" }, { "oranges", "🍊" },
        { "grapes", "🍇" }, { "strawberry", "🍓" }, { "strawberries", "🍓" }, { "watermelon", "🍉" }, { "lemon", "🍋" }, { "lemons", "🍋" },
        { "peach", "🍑" }, { "peaches", "🍑" }, { "pear", "🍐" }, { "pears", "🍐" }, { "cherry", "🍒" }, { "cherries", "🍒" },
        { "mango", "🥭" }, { "pineapple", "🍍" }, { "coconut", "🥥" }, { "avocado", "🥑" }, { "tomato", "🍅" }, { "tomatoes", "🍅" },
        { "carrot", "🥕" }, { "carrots", "🥕" }, { "corn", "🌽" }, { "broccoli", "🥦" }, { "lettuce", "🥬" }, { "salad", "🥬" },{ "spinach", "🥬" },
        { "cucumber", "🥒" }, { "peppers", "🫑" }, { "pepper", "🫑" }, { "onion", "🧅" }, { "onions", "🧅" }, { "garlic", "🧄" },
        { "potato", "🥔" }, { "potatoes", "🥔" }, { "mushroom", "🍄" }, { "mushrooms", "🍄" }, { "eggplant", "🍆" },
        // Meat & Seafood
        { "chicken", "🍗" }, { "beef", "🥩" }, { "steak", "🥩" }, { "pork", "🥩" }, { "bacon", "🥓" }, { "fish", "🐟" },
        { "salmon", "🐟" }, { "shrimp", "🍤" }, { "lobster", "🦞" }, { "crab", "🦀" }, { "turkey", "🦃" }, { "ham", "🍖" }, { "sausage", "🌭" },
        // Bread & Grains
        { "bread", "🍞" }, { "rice", "🍚" }, { "pasta", "🍝" }, { "noodles", "🍜" }, { "cereal", "🥣" }, { "oats", "🥣" },
        { "bagel", "🥯" }, { "croissant", "🥐" }, { "tortilla", "🫓" }, { "tortillas", "🫓" }, { "crackers", "🍘" },
        // Beverages
        { "water", "💧" }, { "juice", "🧃" }, { "coffee", "☕" }, { "tea", "🍵" }, { "beer", "🍺" }, { "wine", "🍷" },
        { "soda", "🥤" }, { "cola", "🥤" }, { "smoothie", "🥤" },
        // Snacks & Sweets
        { "chocolate", "🍫" }, { "candy", "🍬" }, { "cookies", "🍪" }, { "cake", "🎂" }, { "ice cream", "🍦" }, { "icecream", "🍦" },
        { "chips", "🍟" }, { "popcorn", "🍿" }, { "nuts", "🥜" }, { "peanuts", "🥜" }, { "honey", "🍯" },
        // Condiments & Pantry
        { "salt", "🧂" }, { "sugar", "🍬" }, { "oil", "🫙" }, { "vinegar", "🫙" }, { "ketchup", "🍅" }, { "mustard", "🟡" },
        { "mayo", "🫙" }, { "mayonnaise", "🫙" }, { "sauce", "🫙" }, { "soup", "🍲" }, { "beans", "🫘" },
        // Frozen
        { "pizza", "🍕" }, { "frozen", "🧊" },
        // Household
        { "soap", "🧼" }, { "shampoo", "🧴" }, { "toilet paper", "🧻" }, { "paper towels", "🧻" }, { "detergent", "🫧" },
        { "toothpaste", "🪥" }, { "toothbrush", "🪥" },
    };

    public static string GetEmoji(string itemName)
    {
        var lower = itemName.ToLower().Trim();
        foreach (var kvp in _map)
        {
            if (lower.Contains(kvp.Key))
                return kvp.Value;
        }
        return "🛒";
    }
}
