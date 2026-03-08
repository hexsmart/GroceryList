# 🤖 AI Prompts

This document tracks all AI-assisted changes made to the Grocery List app using NEXA/CoPilot.

← [Back to README](../README.md)

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
* �� Auto-capitalize first letter of item names on add
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
