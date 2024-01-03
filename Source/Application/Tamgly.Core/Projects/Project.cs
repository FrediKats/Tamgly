using Kysect.CommonLib.BaseTypes.Extensions;
using System;
using System.Collections.Generic;
using Tamgly.Common.Exceptions;
using Tamgly.Core.WorkItems;

namespace Tamgly.Core.Projects;

public class Project : IEquatable<Project>
{
    public Guid Id { get; }
    public string Title { get; }
    public ICollection<WorkItem> Items { get; }
    public WorkingHours WorkingHours { get; }

    public static Project Create(string title, WorkingHours? workingHours = null)
    {
        ArgumentNullException.ThrowIfNull(title);

        return new Project(Guid.NewGuid(), title, new List<WorkItem>(), workingHours ?? WorkingHours.Empty);
    }

    public Project(Guid id, string title, ICollection<WorkItem> items, WorkingHours workingHours)
    {
        Id = id;
        Title = title;
        Items = items;
        WorkingHours = workingHours;
    }

    public void AddItem(WorkItem item)
    {
        ArgumentNullException.ThrowIfNull(item);

        Items.Add(item);
    }

    public void RemoveItem(WorkItem item)
    {
        ArgumentNullException.ThrowIfNull(item);

        if (!Items.Remove(item))
            throw new TamglyException($"Work item with id {item.Id} was not found.");
    }

    public string ToShortString()
    {
        return $"{Title} ({Id.ToShortString()})";
    }

    public bool Equals(Project? other)
    {
        if (other is null)
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return Id.Equals(other.Id);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
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