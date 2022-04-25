using System.Text;
using Kysect.Tamgly.Common;
using Kysect.Tamgly.Graphs;

namespace Kysect.Tamgly.Core;

public class GraphBuildResultPrinter
{
    public string GenerateTree(GraphBuildResult<WorkItem> tree)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Tree:");
        foreach (GraphNode<WorkItem> graphNode in tree.Roots)
            GenerateTreeInternal(graphNode, 0, sb);
        return sb.ToString();
    }

    public void GenerateTreeInternal(GraphNode<WorkItem> graphNode, int level, StringBuilder stringBuilder)
    {
        var space = StringExtensions.FromChar('\t', level);
        stringBuilder.Append(space).Append(graphNode.Value.ToShortString()).AppendLine();
        foreach (GraphNode<WorkItem> child in graphNode.DirectChildren)
        {
            GenerateTreeInternal(child, level + 1, stringBuilder);
        }
    }
}