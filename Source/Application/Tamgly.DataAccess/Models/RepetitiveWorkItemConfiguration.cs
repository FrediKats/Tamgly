using System.ComponentModel.DataAnnotations;
using Tamgly.RepetitiveEvents.Models;

namespace Tamgly.DataAccess.Models;

public class RepetitiveWorkItemConfigurationDatabaseRecord
{
    [Key]
    public int ParentWorkItemId { get; init; }
    public RepetitiveEventPatternType Type { get; init; }
    public string SerializedConfiguration { get; init; }

    public RepetitiveWorkItemConfigurationDatabaseRecord(int parentWorkItemId, RepetitiveEventPatternType type, string serializedConfiguration)
        : this()
    {
        ParentWorkItemId = parentWorkItemId;
        Type = type;
        SerializedConfiguration = serializedConfiguration;
    }

    protected RepetitiveWorkItemConfigurationDatabaseRecord()
    {
    }
}