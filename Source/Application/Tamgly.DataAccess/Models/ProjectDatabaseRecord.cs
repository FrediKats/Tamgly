using Kysect.CommonLib.DateAndTime;
using System;

namespace Tamgly.DataAccess.Models;

public class ProjectDatabaseRecord
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public TimeSpan? PerDay { get; }
    public SelectedDayOfWeek? SelectedDays { get; }
    public TimeSpan? PerWeek { get; }
    public TimeSpan? PerMonth { get; }

    public ProjectDatabaseRecord(Guid id, string title, TimeSpan? perDay, SelectedDayOfWeek? selectedDays, TimeSpan? perWeek, TimeSpan? perMonth)
        : this()
    {
        Id = id;
        Title = title;
        PerDay = perDay;
        SelectedDays = selectedDays;
        PerWeek = perWeek;
        PerMonth = perMonth;
    }

    public ProjectDatabaseRecord()
    {
    }
}