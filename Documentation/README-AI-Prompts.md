# 🤖 AI Prompts

This document tracks all AI-assisted changes made to the Grocery List app using NEXA/CoPilot.  _(The text may or may not be what was said exactly - just the general gist of the ask.)_

← [Back to README](../README.md)

## Contents
- [2026-03-07](#2026-03-07)
- [2026-03-08](#2026-03-08)
- [2026-04-08](#2026-04-08)

## 2026-03-07

* 🆕 Created GitHub Repo
  * It needed an access token because `gh auth` didn't work.
* 🏗️ Created ASP.NET Core MVC Project
  * Grocery list website
  * Add comma-separated items
  * Persists to JSON file
  * Add, Remove, and Clear All functionality
* 🏷️ Added Category field to each item
  * Defaults to "Staple"
  * Editable inline per item
* 😄 Auto-detect emojis based on item name
  * 80+ grocery keyword mappings
  * Defaults to 🛒 if no match found
* 🔤 Alphabetized the grocery list
* 🧹 Removed Privacy nav link, footer link, and view
* ✅ Click/tap row to select items
  * Checkbox reflects selected state
  * Green highlight on selected rows
* 📝 Added `.gitignore` to exclude build artifacts and `groceries.json`
* 🧺 Added "Shop" view with cart functionality
  * Selected items stored in `localStorage` to persist across views
  * Shop view shows cart with per-item Remove button
  * Removing from cart deselects the row on the Grocery List
* 🗑️ Removed "Clear All" button (too dangerous!)
* 🔠 Auto-capitalize first letter of item names on add
* 🛒 Renamed nav item to "Grocery List" with cart emoji
* ☁️ Published to Azure App Service
  * Required quota increase to create App Service
  * Live at: https://smartgrocerylist-bwa8bkgha7eqacaz.eastus-01.azurewebsites.net/
* 📵 Hide category, Save, and Remove buttons on mobile (< 576px)
* 🔤 Alphabetized EmojiHelper entries within each category
* 🏪 Added Store view grouped by category
  * Refactored EmojiHelper to use `StoreItem` records with Name, Emoji, Category
  * Items already on list show ✅ Added
* ⭐ "Add Staples" button selects all Staple items at once
* 💾🗑️ Save and Remove buttons use emoji icons
* 🚫 Prevent duplicate items from being added
* 🗂️ Category field changed to Staple / Other dropdown
* 🧹 Removed redundant Grocery List nav item
* 🛒 Deleting an item from Home also removes it from the Shop cart
* ✂️ Removed strikethrough on selected Home list items
* 🩶 Shop list rows turn grey when tapped (toggle on/off)

## 2026-03-08

* 📄 Split AI Prompts into separate `Documentation/README-AI-Prompts.md`; linked from README
* 📋 Created `Documentation/README-TODO.md` with GitHub issues linked
* 📅 Created `Documentation/README-ActivityLog.md`
* 🔍 Replaced hand emojis with magnifying glass emojis in README links
* 🧪 Added `GroceryList.Tests` xUnit project with 21 tests
  * `GroceryServiceTests` — add, capitalize, deduplicate, alphabetize, remove, clear, persist
  * `EmojiHelperTests` — known emoji, unknown fallback, case-insensitive, alphabetized
  * `GroceryItemTests` — default GUID, category, and name
  * Each test class in its own file
* 📁 Moved documentation files into `Documentation/` folder
* 🗂️ Category view on Home page (Issue #1)
  * Collapsible sections with SortableJS drag-to-reorder
  * Category order persisted to `settings-{userId}.json` (per user)
  * Flat/Category view toggle
  * ✖ Clear Selection button
* 👤 User login & registration (Issue #2)
  * Login and Register pages
  * Per-user grocery list files (`groceries-{userId}.json`)
  * Session-based auth (30-day idle timeout)
  * Navbar shows user first name + Sign Out
* 🗂️ Category view added to Shop and Store pages
  * Collapsible sections, drag-to-reorder, saves to same settings file
* 📌 Sticky navbar (`fixed-top`) so menu stays visible while scrolling
* 🏪 Store page improvements
  * AJAX add — stays on Store page, button becomes ✅ Added
  * 🔽 Collapse All / 🔼 Expand All button
* 💸 Shop nav item uses money-flying emoji
* 🧺 Shop page improvements
  * Checkboxes on items (like Home page)
  * Category badge shows total; switches to selected/total when items checked
  * Badge updates live on toggle
  * Ampersand fix for category IDs (Bread & Grains, Condiments & Pantry)
  * Selection preserved when switching between flat and category view
  * Category badges refresh when switching from flat to category view
  * Flat list always alphabetized
* 🔄 Home page button order: Add Staples → Flat View → Clear Selection

## 2026-04-08

* 🧪 **Converted test framework from xUnit to MSTest**
  * All 49 tests passing with MSTest framework
  * Added `UserProfileTests.cs` with 10 new tests
  * Fixed duplicate property definitions in `UserProfile.cs`
* 🛡️ **Added null-safety to models**
  * `UserProfile.cs`: Private backing fields with null-to-empty-string conversion for all properties
  * `GroceryItem.cs`: Private backing fields with null-safety (Name → empty, Category → "Staple")
* 🧪 **Expanded and standardized all test classes** (62 total tests)
  * `UserProfileTests.cs`: Expanded to 16 tests (added email validation and null-safety tests)
  * `GroceryItemTests.cs`: Expanded to 10 tests (added setter, null-safety, and object initializer tests)
  * All test methods renamed to `ClassName_PropertyOrMethod_Description` pattern
  * All test methods alphabetized within each class
  * Applied to: `EmojiHelperTests`, `UserProfileTests`, `GroceryItemTests`, `UserServiceTests`, `SettingsServiceTests`, `GroceryServiceTests`
* ⬆️ **Upgraded from .NET 9 to .NET 10**
  * Updated `TargetFramework` in both project files
  * All 62 tests pass on .NET 10
* 📋 **Created `README-Normal-Behaviors.md`**
  * Documents established patterns and preferences for AI-assisted work
  * Includes git workflows, testing standards, and consistency patterns
* ✏️ **Standardized markdown formatting in `README-Normal-Behaviors.md`**
  * Changed all bullet points from `-` (hyphens) to `*` (asterisks)
  * Added "Markdown Style" section documenting this preference
* 🤝 **Implemented shared grocery lists feature (Issue #3)**
  * Created `SharedList` model, `SharedListService`, and `SharedListController`
  * Added member management (invite by email, add/remove members, leave lists)
  * Updated `HomeController` with list switcher between personal and shared lists
  * Owner-only operations: add/remove members, rename, delete; Members: view/edit, leave
  * Created 20 tests for `SharedListService`, all 82 tests passing
* 🧹 **Repository cleanup**
  * Removed empty `docs/` folder
  * Updated `.gitignore` to exclude user-generated JSON data files
* 🔒 **Restricted category reordering to Store page only**
  * Removed drag handles from Index (Home) and Shop pages
  * Category order can now only be changed via the Store page
  * Prevents confusion from accidental category reordering on other pages
  * Fixed category order sync by updating localStorage when reordering on Store page
* ⚡ **Improved category dropdown UX on Home page**
  * Removed save buttons (💾) next to category dropdowns
  * Category changes now auto-save when dropdown value changes
  * Provides immediate feedback and streamlines the workflow
  * Changed to AJAX submission to prevent page scroll on category update
  * Fixed "Add Staples" button to respect category changes (updates data-category attribute after AJAX)
* 📋 **Added Copy List button on Shop page**
  * New "Copy List" button copies all cart items to clipboard
  * Each item appears on its own line with emoji included
  * Respects current view mode:
    * Flat view: Items sorted alphabetically
    * Category view: Items grouped by category with headers, indented items
  * Designed for texting shopping list to others
  * Shows brief success confirmation after copying
  * Fixed category count badge to update when deleting items

## 2026-04-09

* 🗂️ **Refactored JavaScript from Home views to external files**
  * Created three new JavaScript files in `wwwroot/js/`:
    * `index.js` - Cart management, view toggle, category collapse, staple selection, AJAX category updates
    * `shop.js` - Cart display, item removal, copy to clipboard, view mode handling
    * `store.js` - Item selection, category reordering with Sortable
  * Updated `Index.cshtml`, `Shop.cshtml`, `Store.cshtml`:
    * Removed embedded `<script>` blocks containing view-specific JavaScript
    * Kept SortableJS CDN references
    * Added references to new external JS files with `asp-append-version` for cache-busting
  * Benefits:
    * Better code organization (JavaScript separated from markup)
    * Improved caching (external JS files cached independently)
    * Easier maintenance (changes don't require editing view files)
    * Better separation of concerns (Razor views focus on markup, JS files focus on behavior)
  * Each JS file is self-contained with no shared functions between views
  * All functionality preserved and tested successfully
* 🧹 **Fixed normal behavior compliance issues**
  * Alphabetized test methods in `SharedListServiceTests.cs`
  * Changed all markdown bullet points from `-` to `*` in `README-TODO.md` and `README-ActivityLog.md`

