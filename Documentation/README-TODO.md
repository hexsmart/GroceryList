# 📋 TODO

← [Back to README](../README.md)

- [x] [Display the categories on the home page, breaking out the items](https://github.com/hexsmart/GroceryList/issues/1)
  * [x] Categories should be collapsible
  * [x] Categories can be moved around
  * [x] Categories can be toggled on or off
- [x] [Allow separate grocery list files based on a user ID](https://github.com/hexsmart/GroceryList/issues/2)
- [ ] [Allow multiple users to share the same list](https://github.com/hexsmart/GroceryList/issues/3)
- [ ] [Investigate SignalR for real-time updates](https://github.com/hexsmart/GroceryList/issues/4)
- [ ] [Allow the user to update their theme](https://github.com/hexsmart/GroceryList/issues/5)
- [ ] [Preserve cart selection state across all views](https://github.com/hexsmart/GroceryList/issues/6)
  * [ ] Might need to preserve this in a file
- [ ] [Support multiple stores with custom item lists](https://github.com/hexsmart/GroceryList/issues/7)
  * [ ] Create a Store model (name, description, category/item list)
  * [ ] Add a default grocery store (current behavior)
  * [ ] Allow creating custom stores (e.g. Home Depot, Liquor Store)
  * [ ] Each store has its own item catalog (user-defined items with category and emoji)
  * [ ] Each store gets its own separate grocery/shopping list
  * [ ] Add a store switcher to the nav or home page
  * [ ] Store definitions saved per user
  * [ ] Pre-seed example stores as starting templates
- [ ] [Color-code grocery list items by category (Staple vs Other)](https://github.com/hexsmart/GroceryList/issues/8)
  * [ ] Assign a distinct background color to Staple items
  * [ ] Assign a distinct background color to Other items
  * [ ] Allow the user to customize the colors per category
  * [ ] Colors saved per user in settings
  * [ ] Apply in both flat and category views
- [ ] [Add an About page with credits and backstory](https://github.com/hexsmart/GroceryList/issues/9)
  * [ ] Backstory — why this app was built (AI-assisted dev exercise with GitHub Copilot)
  * [ ] Credits (developer, tools used)
  * [ ] Link to the GitHub repo
  * [ ] Add About link to the navbar
- [ ] [Add Import/Export for user data (grocery list and settings)](https://github.com/hexsmart/GroceryList/issues/10)
  * [ ] Export grocery list to JSON or CSV
  * [ ] Export settings (category order, colors, etc.) to JSON
  * [ ] Import grocery list from file
  * [ ] Import settings from file
  * [ ] Add Import/Export UI to a settings or profile page
- [ ] [Background process to clean up inactive user files](https://github.com/hexsmart/GroceryList/issues/11)
  * [ ] Track last-active timestamp per user
  * [ ] Scheduled background task (runs once daily)
  * [ ] Delete grocery list, settings, and user files after 30 days of inactivity
  * [ ] Log cleanup activity