namespace Kysect.Tamgly.Core;

public class ProjectWorkHoursValidator
{
    public IReadOnlyCollection<WorkingHoursWarning> Validate(Project project)
    {
        var result = new List<WorkingHoursWarning>();

        IEnumerable<IGrouping<ITimeInterval?, WorkItem>> workItemGroupedByDeadline = project.GetAllWorkItemWithRepetitive().GroupBy(wi => wi.Deadline.TimeInterval);
        foreach (IGrouping<ITimeInterval?, WorkItem> items in workItemGroupedByDeadline)
        {
            switch (items.Key)
            {
                case null:
                    break;
                case TamglyDay tamglyDay:
                    WorkingHoursWarning? validateDailyLimit = project.WorkingHours.ValidateDailyLimit(tamglyDay, items.ToList());
                    if (validateDailyLimit is not null)
                        result.Add(validateDailyLimit.Value);
                    break;

                case TamglyMonth tamglyMonth:
                    WorkingHoursWarning? validateMonthlyLimit = project.WorkingHours.ValidateMonthlyLimit(tamglyMonth, items.ToList());
                    if (validateMonthlyLimit is not null)
                        result.Add(validateMonthlyLimit.Value);
                    break;

                case TamglyWeek tamglyWeek:
                    WorkingHoursWarning? validateWeeklyLimit = project.WorkingHours.ValidateWeeklyLimit(tamglyWeek, items.ToList());
                    if (validateWeeklyLimit is not null)
                        result.Add(validateWeeklyLimit.Value);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        return result;
    }
}