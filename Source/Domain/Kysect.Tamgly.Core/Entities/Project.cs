using Kysect.Tamgly.Core.Tools;

namespace Kysect.Tamgly.Core.Entities;

public class Project : IEquatable<Project>
{
    public Guid Id { get; }
    public string Title { get; }
    public ICollection<WorkItem> Items { get; }

    public static Project Create(string title)
    {
        return new Project(Guid.NewGuid(), title, new List<WorkItem>());
    }

    public Project(Guid id, string title, ICollection<WorkItem> items)
    {
        Id = id;
        Title = title;
        Items = items;
    }

    public void AddItem(WorkItem item)
    {
        Items.Add(item);
    }

    public void RemoveItem(WorkItem item)
    {
        if (!Items.Remove(item))
            throw new TamglyException($"Work item with id {item.Id} was not found.");
    }

    public bool Equals(Project? other)
    {
        if (ReferenceEquals(null, other))
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return Id.Equals(other.Id);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (obj is Project project)
            return Equals(project);
        return false;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}