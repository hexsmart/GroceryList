# 🛒 Grocery List

A general grocery list application.

## 📖 Overview

This is an exercise in using NEXA/CoPilot to create a project from scratch.  I'm **hoping** that I'm able to use prompts for 99% of this.

NOTE: That **CoPilot** will add its name to the changesets.

NEXA is adding to the prompts below after major milestones _(even minor ones)_

## 🤖 AI Prompts

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
  * Green highlight + strikethrough on selected rows
* 📝 Added `.gitignore` to exclude build artifacts and `groceries.json`