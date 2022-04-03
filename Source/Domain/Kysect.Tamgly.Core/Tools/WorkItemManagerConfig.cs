namespace Kysect.Tamgly.Core.Tools;

public class WorkItemManagerConfig
{
    private const double DefaultAcceptableEstimateDiff = 0.8;

    public double AcceptableEstimateDiff { get; set; }

    public WorkItemManagerConfig()
        : this(DefaultAcceptableEstimateDiff)
    {
    }

    public WorkItemManagerConfig(double acceptableEstimateDiff)
    {
        AcceptableEstimateDiff = acceptableEstimateDiff;
    }
}