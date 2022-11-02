using TreeSize.Model;

namespace TreeSize.ViewModel.Interfaces;

public interface ITreeContainer
{
    TreeNode TreeRoot { get;}
    object TreeLock { get; }
}
