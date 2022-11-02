using System;

namespace TreeSize.Model.Exceptions;

public class NegativePercentException : Exception
{
    public NegativePercentException(TreeNode treeNode)
            : base($"Tree node percent {treeNode.Path} is negative")
    {
    }
}
