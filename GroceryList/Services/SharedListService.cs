using System.Text.Json;
using GroceryList.Models;

namespace GroceryList.Services;

public class SharedListService
{
    private readonly string _contentRoot;

    public SharedListService(IWebHostEnvironment env)
    {
        _contentRoot = env.ContentRootPath;
    }

    private string ListFilePath(Guid listId) =>
        Path.Combine(_contentRoot, $"shared-list-{listId}.json");

    private string UserSharedListsFilePath(string userId) =>
        Path.Combine(_contentRoot, $"user-shared-lists-{userId}.json");

    private string AllSharedListsFilePath() =>
        Path.Combine(_contentRoot, "all-shared-lists.json");

    // Get all shared lists that a user has access to
    public List<SharedList> GetUserSharedLists(string userId)
    {
        var allLists = GetAllSharedLists();
        return allLists.Where(l => l.OwnerId.ToString() == userId || l.MemberIds.Any(m => m.ToString() == userId))
            .OrderBy(l => l.Name, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    // Get all shared lists in the system
    public List<SharedList> GetAllSharedLists()
    {
        var path = AllSharedListsFilePath();
        if (!File.Exists(path)) return new List<SharedList>();
        var json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<List<SharedList>>(json) ?? new List<SharedList>();
    }

    // Save all shared lists to master file
    private void SaveAllSharedLists(List<SharedList> lists)
    {
        File.WriteAllText(AllSharedListsFilePath(), JsonSerializer.Serialize(lists, new JsonSerializerOptions { WriteIndented = true }));
    }

    // Create a new shared list
    public SharedList CreateSharedList(string userId, string name)
    {
        var sharedList = new SharedList
        {
            Name = name,
            OwnerId = Guid.Parse(userId)
        };

        var allLists = GetAllSharedLists();
        allLists.Add(sharedList);
        SaveAllSharedLists(allLists);

        // Initialize empty grocery list for this shared list
        var groceryService = new GroceryService(new WebHostEnvironmentWrapper(_contentRoot));
        groceryService.Save(sharedList.Id.ToString(), new List<GroceryItem>());

        return sharedList;
    }

    // Get a specific shared list
    public SharedList? GetSharedList(Guid listId)
    {
        return GetAllSharedLists().FirstOrDefault(l => l.Id == listId);
    }

    // Add a member to a shared list by email
    public bool AddMember(Guid listId, string userId, string memberEmail, UserService userService)
    {
        var sharedList = GetSharedList(listId);
        if (sharedList == null) return false;

        // Only the owner can add members
        if (sharedList.OwnerId.ToString() != userId) return false;

        var member = userService.FindByEmail(memberEmail);
        if (member == null) return false;

        // Don't add if already a member or owner
        if (sharedList.MemberIds.Contains(member.Id) || sharedList.OwnerId == member.Id)
            return false;

        sharedList.MemberIds.Add(member.Id);

        var allLists = GetAllSharedLists();
        var index = allLists.FindIndex(l => l.Id == listId);
        if (index >= 0)
        {
            allLists[index] = sharedList;
            SaveAllSharedLists(allLists);
        }

        return true;
    }

    // Remove a member from a shared list
    public bool RemoveMember(Guid listId, string userId, Guid memberIdToRemove)
    {
        var sharedList = GetSharedList(listId);
        if (sharedList == null) return false;

        // Only the owner can remove members
        if (sharedList.OwnerId.ToString() != userId) return false;

        if (!sharedList.MemberIds.Contains(memberIdToRemove)) return false;

        sharedList.MemberIds.Remove(memberIdToRemove);

        var allLists = GetAllSharedLists();
        var index = allLists.FindIndex(l => l.Id == listId);
        if (index >= 0)
        {
            allLists[index] = sharedList;
            SaveAllSharedLists(allLists);
        }

        return true;
    }

    // Leave a shared list (for non-owners)
    public bool LeaveSharedList(Guid listId, string userId)
    {
        var sharedList = GetSharedList(listId);
        if (sharedList == null) return false;

        // Can't leave if you're the owner
        if (sharedList.OwnerId.ToString() == userId) return false;

        var memberGuid = Guid.Parse(userId);
        if (!sharedList.MemberIds.Contains(memberGuid)) return false;

        sharedList.MemberIds.Remove(memberGuid);

        var allLists = GetAllSharedLists();
        var index = allLists.FindIndex(l => l.Id == listId);
        if (index >= 0)
        {
            allLists[index] = sharedList;
            SaveAllSharedLists(allLists);
        }

        return true;
    }

    // Delete a shared list (only owner)
    public bool DeleteSharedList(Guid listId, string userId)
    {
        var sharedList = GetSharedList(listId);
        if (sharedList == null) return false;

        // Only the owner can delete
        if (sharedList.OwnerId.ToString() != userId) return false;

        var allLists = GetAllSharedLists();
        allLists.RemoveAll(l => l.Id == listId);
        SaveAllSharedLists(allLists);

        // Delete the grocery list file
        var listFile = ListFilePath(listId);
        if (File.Exists(listFile))
        {
            File.Delete(listFile);
        }

        return true;
    }

    // Check if a user has access to a shared list
    public bool HasAccess(Guid listId, string userId)
    {
        var sharedList = GetSharedList(listId);
        if (sharedList == null) return false;

        return sharedList.OwnerId.ToString() == userId ||
               sharedList.MemberIds.Any(m => m.ToString() == userId);
    }

    // Rename a shared list (only owner)
    public bool RenameSharedList(Guid listId, string userId, string newName)
    {
        var sharedList = GetSharedList(listId);
        if (sharedList == null) return false;

        // Only the owner can rename
        if (sharedList.OwnerId.ToString() != userId) return false;

        sharedList.Name = newName;

        var allLists = GetAllSharedLists();
        var index = allLists.FindIndex(l => l.Id == listId);
        if (index >= 0)
        {
            allLists[index] = sharedList;
            SaveAllSharedLists(allLists);
        }

        return true;
    }

    // Helper class to wrap IWebHostEnvironment
    private class WebHostEnvironmentWrapper : IWebHostEnvironment
    {
        private readonly string _contentRoot;

        public WebHostEnvironmentWrapper(string contentRoot)
        {
            _contentRoot = contentRoot;
        }

        public string WebRootPath { get; set; } = string.Empty;
        public string ContentRootPath
        {
            get => _contentRoot;
            set { }
        }
        public string ApplicationName { get; set; } = string.Empty;
        public string EnvironmentName { get; set; } = string.Empty;
        public Microsoft.Extensions.FileProviders.IFileProvider WebRootFileProvider { get; set; } = null!;
        public Microsoft.Extensions.FileProviders.IFileProvider ContentRootFileProvider { get; set; } = null!;
    }
}
