using System.Windows.Data;
using TreeSize.Model;
using TreeSize.ViewModel.Interfaces;

namespace TreeSize.ViewModel;

public class TreeContainer : ITreeContainer
{
    public object TreeLock { get; } = new object();
    private TreeNode _treeRoot;

    public TreeNode TreeRoot
    { 
        get => _treeRoot;
        set
        {
            _treeRoot = value;
        }
    }

    public TreeContainer(TreeNode root)
    {
        TreeRoot = root;
        BindingOperations.EnableCollectionSynchronization(TreeRoot.SubNodes, TreeLock);
    }
}
