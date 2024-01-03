using Kysect.CommonLib.Exceptions;
using System.Collections.Generic;
using System.Linq;
using Tamgly.Core.RepetitiveWorkItems;
using Tamgly.Core.TimeIntervals;
using Tamgly.Core.WorkItems;

namespace Tamgly.Core.Projects;

public class ProjectWorkHoursValidator
{
    public IReadOnlyCollection<WorkingHoursWarning> Validate(Project project, IRepetitiveWorkItemResolver repetitiveWorkItemResolver)
    {
        var result = new List<WorkingHoursWarning>();

        IReadOnlyCollection<WorkItem> workItems = repetitiveWorkItemResolver.CreateWorkItemsFromRepetitive(project);
        IEnumerable<IGrouping<ITimeInterval, WorkItem>> workItemGroupedByDeadline = workItems.GroupBy(wi => wi.Deadline.TimeInterval);
        foreach (IGrouping<ITimeInterval?, WorkItem> items in workItemGroupedByDeadline)
        {
            ITimeInterval? timeInterval = items.Key;
            switch (timeInterval)
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
                    throw SwitchDefaultExceptions.OnUnexpectedType(timeInterval);
            }
        }

        return result;
    }
}