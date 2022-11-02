using System.Threading.Tasks;
using TreeSize.ViewModel.Interfaces;

namespace TreeSize.ViewModel;

public class TreeSizeInitiator
{
    private readonly ITreeContainer _treeContainer;

    public TreeSizeInitiator(ITreeContainer treeContainer) 
    {
        _treeContainer = treeContainer;
    }

    public void Start()
    {
        Task.Run(() => StartAsync());
    }
   
    private async Task StartAsync()
    {
        var builder = new TreeBuilder(_treeContainer);

        builder.Build();
    }
}
