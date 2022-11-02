using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Windows.Input;
using TreeSize.ViewModel.Interfaces;
using TreeNode = TreeSize.Model.TreeNode;

namespace TreeSize.ViewModel;

partial class TreeViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public ICommand OpenSelectDirectoryDialog { get; }

    private TreeSizeInitiator _initiator;
    private ITreeContainer _treeContainer;

    public ITreeContainer TreeContainer
    {
        get 
        {
            return _treeContainer;
        }
        set 
        { 
            _treeContainer = value; 
            OnPropertyChanged();
        }
    }

    public TreeViewModel()
    {
        OpenSelectDirectoryDialog = new RelayCommand(OpenSelectDirectory);
    }

    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    private void OpenSelectDirectory()
    {
        using (var dialog = new FolderBrowserDialog())
        {
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var folder = dialog.SelectedPath;
                var dir = new DirectoryInfo(folder);
                BuildSelected(dir);
            }
        }
    }

    private void BuildSelected(DirectoryInfo dir)
    {
        TreeContainer?.TreeRoot.SubNodes.Clear();
        TreeContainer = new TreeContainer(new TreeNode(dir, null));
        _initiator = new TreeSizeInitiator(TreeContainer);
        _initiator.Start();
    }
}