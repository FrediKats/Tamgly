using Tamgly.Integration.MicrosoftGraphAdapter.Todo;

namespace Tamgly.Playground;

public class TamglyConfig
{
    public string ClockifyApiKey { get; set; }

    public TamglyMicrosoftTodoConfig MicrosoftTodo { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public TamglyConfig()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {

    }
}