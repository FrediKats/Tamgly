namespace Kysect.Tamgly.Core.Tools;

public class WorkItemManagerConfig
{
    private const double DefaultAcceptableEstimatePercentDiff = 0.8;

    public double AcceptableEstimateDiff { get; set; }

    public WorkItemManagerConfig()
        : this(DefaultAcceptableEstimatePercentDiff)
    {
    }

    public WorkItemManagerConfig(double acceptableEstimateDiff)
    {
        AcceptableEstimateDiff = acceptableEstimateDiff;
    }
}