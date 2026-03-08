namespace GroceryList.Helpers;

public record StoreItem(string Name, string Emoji, string Category);

public static class EmojiHelper
{
    private static readonly List<StoreItem> _items = new()
    {
        // Beverages
        new("Beer", "🍺", "Beverages"),
        new("Coffee", "☕", "Beverages"),
        new("Diet Coke", "🥤", "Beverages"),
        new("Juice", "🧃", "Beverages"),
        new("Soda", "🥤", "Beverages"),
        new("Tea", "🍵", "Beverages"),
        new("Water", "💧", "Beverages"),
        new("Wine", "🍷", "Beverages"),
        // Bread & Grains
        new("Bagel", "🥯", "Bread & Grains"),
        new("Bread", "🍞", "Bread & Grains"),
        new("Cereal", "🥣", "Bread & Grains"),
        new("Crackers", "🍘", "Bread & Grains"),
        new("Croissant", "🥐", "Bread & Grains"),
        new("Noodles", "🍜", "Bread & Grains"),
        new("Oats", "🥣", "Bread & Grains"),
        new("Pasta", "🍝", "Bread & Grains"),
        new("Rice", "🍚", "Bread & Grains"),
        new("Tortillas", "🫓", "Bread & Grains"),
        // Condiments & Pantry
        new("Beans", "🫘", "Condiments & Pantry"),
        new("Ketchup", "🍅", "Condiments & Pantry"),
        new("Mayonnaise", "🫙", "Condiments & Pantry"),
        new("Mustard", "🟡", "Condiments & Pantry"),
        new("Oil", "🫙", "Condiments & Pantry"),
        new("Salt", "🧂", "Condiments & Pantry"),
        new("Sauce", "🫙", "Condiments & Pantry"),
        new("Soup", "🍲", "Condiments & Pantry"),
        new("Sugar", "🍬", "Condiments & Pantry"),
        new("Vinegar", "🫙", "Condiments & Pantry"),
        // Dairy
        new("Butter", "🧈", "Dairy"),
        new("Cheese", "🧀", "Dairy"),
        new("Cream", "🥛", "Dairy"),
        new("Eggs", "🥚", "Dairy"),
        new("Milk", "🥛", "Dairy"),
        new("Yogurt", "🥛", "Dairy"),
        // Frozen
        new("Frozen Foods", "🧊", "Frozen"),
        new("Pizza", "🍕", "Frozen"),
        // Household
        new("Detergent", "🫧", "Household"),
        new("Paper Towels", "🧻", "Household"),
        new("Shampoo", "🧴", "Household"),
        new("Soap", "🧼", "Household"),
        new("Toilet Paper", "🧻", "Household"),
        new("Toothbrush", "🪥", "Household"),
        new("Toothpaste", "🪥", "Household"),
        // Meat & Seafood
        new("Bacon", "🥓", "Meat & Seafood"),
        new("Beef", "🥩", "Meat & Seafood"),
        new("Chicken", "🍗", "Meat & Seafood"),
        new("Crab", "🦀", "Meat & Seafood"),
        new("Fish", "🐟", "Meat & Seafood"),
        new("Ham", "🍖", "Meat & Seafood"),
        new("Lobster", "🦞", "Meat & Seafood"),
        new("Pork", "🥩", "Meat & Seafood"),
        new("Salmon", "🐟", "Meat & Seafood"),
        new("Sausage", "🌭", "Meat & Seafood"),
        new("Shrimp", "🍤", "Meat & Seafood"),
        new("Steak", "🥩", "Meat & Seafood"),
        new("Turkey", "🦃", "Meat & Seafood"),
        // Produce
        new("Apple", "🍎", "Produce"),
        new("Avocado", "🥑", "Produce"),
        new("Banana", "🍌", "Produce"),
        new("Broccoli", "🥦", "Produce"),
        new("Carrot", "🥕", "Produce"),
        new("Cherries", "🍒", "Produce"),
        new("Coconut", "🥥", "Produce"),
        new("Corn", "🌽", "Produce"),
        new("Cucumber", "🥒", "Produce"),
        new("Eggplant", "🍆", "Produce"),
        new("Garlic", "🧄", "Produce"),
        new("Grapes", "🍇", "Produce"),
        new("Lemon", "🍋", "Produce"),
        new("Lettuce", "🥬", "Produce"),
        new("Mango", "🥭", "Produce"),
        new("Mushrooms", "🍄", "Produce"),
        new("Onion", "🧅", "Produce"),
        new("Orange", "🍊", "Produce"),
        new("Peach", "🍑", "Produce"),
        new("Pear", "🍐", "Produce"),
        new("Peppers", "🫑", "Produce"),
        new("Pineapple", "🍍", "Produce"),
        new("Potato", "🥔", "Produce"),
        new("Spinach", "🥬", "Produce"),
        new("Strawberries", "🍓", "Produce"),
        new("Tomato", "🍅", "Produce"),
        new("Watermelon", "🍉", "Produce"),
        // Snacks & Sweets
        new("Cake", "🎂", "Snacks & Sweets"),
        new("Candy", "🍬", "Snacks & Sweets"),
        new("Chips", "🍟", "Snacks & Sweets"),
        new("Chocolate", "🍫", "Snacks & Sweets"),
        new("Cookies", "🍪", "Snacks & Sweets"),
        new("Honey", "🍯", "Snacks & Sweets"),
        new("Ice Cream", "🍦", "Snacks & Sweets"),
        new("Nuts", "🥜", "Snacks & Sweets"),
        new("Popcorn", "🍿", "Snacks & Sweets"),
    };

    public static string GetEmoji(string itemName)
    {
        var lower = itemName.ToLower().Trim();
        var match = _items.FirstOrDefault(i => lower.Contains(i.Name.ToLower()));
        return match?.Emoji ?? "🛒";
    }

    public static List<StoreItem> GetAllItems() =>
        _items.OrderBy(i => i.Name).ToList();
}

