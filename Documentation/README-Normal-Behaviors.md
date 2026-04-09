# 🤖 Normal Behaviors for AI Assistant

This document tracks established patterns and preferences for AI-assisted work on the Grocery List project.

← [Back to README](../README.md)

## 📋 Git & Documentation

### Always Update AI Prompts Documentation
* **When**: Before every `git push`
* **What**: Update `Documentation/README-AI-Prompts.md` with a summary of changes
* **How**: Add a new bullet point under the current date section describing the work completed
* **Include**: This documentation update in the same commit as the code changes

### Commit Messages
* Use clear, descriptive commit messages
* Format: Brief summary of what changed (e.g., "Upgrade from .NET 9 to .NET 10")

### Markdown Style
* **Bullet Points**: Always use `*` for bullet points (not `-`)

## 🧪 Testing Standards

### Test Framework
* **Framework**: MSTest (not xUnit)
* **Reason**: User preference - more familiar with MSTest

### Test Naming Convention
* **Pattern**: `ClassName_PropertyOrMethod_Description`
* **Examples**:
  * `UserProfile_Email_ConvertsNullToEmpty`
  * `GroceryItem_Name_CanSetValue`
  * `EmojiHelper_GetEmoji_ReturnsCorrectEmojiForKnownItem`
* **Structure**:
  1. Class name being tested
  2. Underscore (`_`)
  3. Property or method name being tested
  4. Underscore (`_`)
  5. What aspect is being tested (convert underscores to camelCase, no spaces)

### Test Organization
* **Alphabetize**: All test methods must be alphabetically ordered within each test class
* **Groups**: Tests naturally group by property/method name when alphabetized

## 🔄 Consistency Patterns

### Apply Changes Universally
* When updating one model (e.g., UserProfile), apply the same pattern to similar models (e.g., GroceryItem)
* When updating one test class, apply the same updates to all applicable test classes
* Maintain consistency across the entire codebase

### Code Style
* Use private backing fields with null-safety patterns
* Convert null values to sensible defaults (empty string, default constants, etc.)

## 🎓 Project Context

### Learning Environment
* User is taking a Udemy Copilot course
* GroceryList project is the practice/demonstration project
* Focus on practical, real-world patterns and best practices

## 📝 Notes

* This document should be updated as new patterns and preferences are established
* When in doubt, ask for clarification rather than assuming