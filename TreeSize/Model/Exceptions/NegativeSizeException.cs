using System;

namespace TreeSize.Model.Exceptions;

public class NegativeSizeException : Exception
{
    public NegativeSizeException(TreeNode treeNode)
            : base($"Tree node size {treeNode.Path} is negative")
    {
    }
}
