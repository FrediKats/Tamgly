using Kysect.CommonLib.BaseTypes.Extensions;
using Kysect.CommonLib.Graphs;
using System.Text;
using Tamgly.Core.WorkItems;

namespace Tamgly.Core.Tools;

public class GraphBuildResultPrinter
{
    public string GenerateTree(GraphBuildResult<int, WorkItem> tree)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Tree:");
        foreach (GraphNode<int, WorkItem> graphNode in tree.Roots)
            GenerateTreeInternal(graphNode, 0, sb);
        return sb.ToString();
    }

    public void GenerateTreeInternal(GraphNode<int, WorkItem> graphNode, int level, StringBuilder stringBuilder)
    {
        string space = StringExtensions.FromChar('\t', level);
        stringBuilder.Append(space).Append(graphNode.Value.ToShortString()).AppendLine();
        foreach (GraphNode<int, WorkItem> child in graphNode.DirectChildren)
        {
            GenerateTreeInternal(child, level + 1, stringBuilder);
        }
    }
}