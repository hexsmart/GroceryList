namespace GroceryList.Helpers;

public static class EmojiHelper
{
    private static readonly Dictionary<string, string> _map = new(StringComparer.OrdinalIgnoreCase)
    {
        // Beverages
        { "beer", "🍺" }, { "coffee", "☕" }, { "coke", "🥤" }, { "cola", "🥤" }, { "diet coke", "🥤" },
        { "juice", "🧃" }, { "smoothie", "🥤" }, { "soda", "🥤" }, { "tea", "🍵" }, { "water", "💧" }, { "wine", "🍷" },
        // Bread & Grains
        { "bagel", "🥯" }, { "bread", "🍞" }, { "cereal", "🥣" }, { "crackers", "🍘" }, { "croissant", "🥐" },
        { "noodles", "🍜" }, { "oats", "🥣" }, { "pasta", "🍝" }, { "rice", "🍚" }, { "tortilla", "🫓" }, { "tortillas", "🫓" },
        // Condiments & Pantry
        { "beans", "🫘" }, { "ketchup", "🍅" }, { "mayo", "🫙" }, { "mayonnaise", "🫙" }, { "mustard", "🟡" },
        { "oil", "🫙" }, { "salt", "🧂" }, { "sauce", "🫙" }, { "soup", "🍲" }, { "sugar", "🍬" }, { "vinegar", "🫙" },
        // Dairy
        { "butter", "🧈" }, { "cheese", "🧀" }, { "cream", "🥛" }, { "egg", "🥚" }, { "eggs", "🥚" }, { "milk", "🥛" }, { "yogurt", "🥛" },
        // Frozen
        { "frozen", "🧊" }, { "pizza", "🍕" },
        // Household
        { "detergent", "🫧" }, { "paper towels", "🧻" }, { "shampoo", "🧴" }, { "soap", "🧼" },
        { "toilet paper", "🧻" }, { "toothbrush", "🪥" }, { "toothpaste", "🪥" },
        // Meat & Seafood
        { "bacon", "🥓" }, { "beef", "🥩" }, { "chicken", "🍗" }, { "crab", "🦀" }, { "fish", "🐟" },
        { "ham", "🍖" }, { "lobster", "🦞" }, { "pork", "🥩" }, { "salmon", "🐟" }, { "sausage", "🌭" },
        { "shrimp", "🍤" }, { "steak", "🥩" }, { "turkey", "🦃" },
        // Produce
        { "apple", "🍎" }, { "apples", "🍎" }, { "avocado", "🥑" }, { "banana", "🍌" }, { "bananas", "🍌" },
        { "broccoli", "🥦" }, { "carrot", "🥕" }, { "carrots", "🥕" }, { "cherries", "🍒" }, { "cherry", "🍒" },
        { "coconut", "🥥" }, { "corn", "🌽" }, { "cucumber", "🥒" }, { "eggplant", "🍆" }, { "garlic", "🧄" },
        { "grapes", "🍇" }, { "lemon", "🍋" }, { "lemons", "🍋" }, { "lettuce", "🥬" }, { "mango", "🥭" },
        { "mushroom", "🍄" }, { "mushrooms", "🍄" }, { "onion", "🧅" }, { "onions", "🧅" }, { "orange", "🍊" },
        { "oranges", "🍊" }, { "peach", "🍑" }, { "peaches", "🍑" }, { "pear", "🍐" }, { "pears", "🍐" },
        { "pepper", "🫑" }, { "peppers", "🫑" }, { "pineapple", "🍍" }, { "potato", "🥔" }, { "potatoes", "🥔" },
        { "salad", "🥬" }, { "spinach", "🥬" }, { "strawberries", "🍓" }, { "strawberry", "🍓" },
        { "tomato", "🍅" }, { "tomatoes", "🍅" }, { "watermelon", "🍉" },
        // Snacks & Sweets
        { "cake", "🎂" }, { "candy", "🍬" }, { "chips", "🍟" }, { "chocolate", "🍫" }, { "cookies", "🍪" },
        { "honey", "🍯" }, { "ice cream", "🍦" }, { "icecream", "🍦" }, { "nuts", "🥜" }, { "peanuts", "🥜" }, { "popcorn", "🍿" },
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
