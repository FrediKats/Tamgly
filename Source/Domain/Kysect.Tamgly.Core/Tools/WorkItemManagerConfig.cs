namespace Kysect.Tamgly.Core.Tools;

public class WorkItemManagerConfig
{
    public double AcceptableEstimateDiff { get; set; }

    public WorkItemManagerConfig()
        : this(0.8)
    {
    }

    public WorkItemManagerConfig(double acceptableEstimateDiff)
    {
        AcceptableEstimateDiff = acceptableEstimateDiff;
    }
}