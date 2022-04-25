using Kysect.Tamgly.Common.Extensions;
using Kysect.Tamgly.Core.Entities.RepetitiveWorkItems;
using Kysect.Tamgly.Core.Tools;
using Kysect.Tamgly.Core.ValueObjects;

namespace Kysect.Tamgly.Core.Entities;

public class Project : IEquatable<Project>
{
    public Guid Id { get; }
    public string Title { get; }
    public ICollection<WorkItem> Items { get; }
    public ICollection<RepetitiveParentWorkItem> RepetitiveItems { get; }
    public WorkingHours WorkingHours { get; }

    public static Project Create(string title, WorkingHours? workingHours = null)
    {
        ArgumentNullException.ThrowIfNull(title);

        return new Project(Guid.NewGuid(), title, new List<WorkItem>(), new List<RepetitiveParentWorkItem>(), workingHours ?? WorkingHours.Empty);
    }

    public Project(Guid id, string title, ICollection<WorkItem> items, ICollection<RepetitiveParentWorkItem> repetitiveItems, WorkingHours workingHours)
    {
        Id = id;
        Title = title;
        Items = items;
        RepetitiveItems = repetitiveItems;
        WorkingHours = workingHours;
    }

    public void AddItem(WorkItem item)
    {
        ArgumentNullException.ThrowIfNull(item);

        Items.Add(item);
    }

    public void AddItem(RepetitiveParentWorkItem item)
    {
        ArgumentNullException.ThrowIfNull(item);

        RepetitiveItems.Add(item);
    }

    public void RemoveItem(WorkItem item)
    {
        ArgumentNullException.ThrowIfNull(item);

        if (!Items.Remove(item))
            throw new TamglyException($"Work item with id {item.Id} was not found.");
    }

    public IReadOnlyCollection<WorkItem> GetAllWorkItemWithRepetitive()
    {
        List<WorkItem> result = new List<WorkItem>();
        result.AddRange(Items);
        result.AddRange(RepetitiveItems.SelectMany(r => r.GetChildWorkItems()));
        return result;
    }

    public string ToShortString()
    {
        return $"{Title} ({Id.ToShortString()})";
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