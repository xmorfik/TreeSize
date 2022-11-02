using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using TreeSize.ViewModel.Interfaces;
using TreeNode = TreeSize.Model.TreeNode;

namespace TreeSize.ViewModel;

public class TreeBuilder
{
    private readonly ITreeContainer _treeContainer;
    private readonly EnumerationOptions _options;

    public TreeBuilder(ITreeContainer treeContainer)
    {
        _treeContainer = treeContainer;
        _options = new EnumerationOptions()
        {
            IgnoreInaccessible = true
        };
    }

    public void Build()
    {
        var dir = new DirectoryInfo(_treeContainer.TreeRoot.Path);

        foreach (var subDir in dir.EnumerateDirectories("*", _options))
        {
            _treeContainer.TreeRoot.SubNodes.Add(new TreeNode(subDir, _treeContainer.TreeRoot));
        }

        Parallel.ForEach(_treeContainer.TreeRoot.SubNodes, dir =>
        {
            Traverse(new DirectoryInfo(dir.Path), dir);
        });

        foreach (var file in dir.EnumerateFiles("*", _options))
        {
            _treeContainer.TreeRoot.SubNodes.Add(new TreeNode(file, _treeContainer.TreeRoot));
        }
    }

    private void Traverse(DirectoryInfo dir, TreeNode root)
    {
        var treeSubNodes = new ObservableCollection<TreeNode>();
        
        foreach (var subDir in dir.EnumerateDirectories("*", _options))
        {
            var dirNode = new TreeNode(subDir, root);
            treeSubNodes.Add(dirNode);
            Traverse(new DirectoryInfo(dirNode.Path), dirNode);
        }
   
        foreach (var file in dir.EnumerateFiles("*", _options))
        {
            treeSubNodes.Add(new TreeNode(file,root));
        }

        root.SubNodes = treeSubNodes;

        CalculatePercent(root);
    }
    
    void CalculatePercent(TreeNode root)
    {
        foreach(var node in root.SubNodes)
        {
            node.Percent = (float)node.Size / root.Size;
        }
    }
}
