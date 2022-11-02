using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using TreeSize.Model.Exceptions;

namespace TreeSize.Model;

public class TreeNode : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public string Path { get; set; }
    public string Name { get; set; }
    public FsType Type { get; set; }
    private long _size;
    private float _percent;
    private ICollection<TreeNode> _subNodes;
    private TreeNode _perent;

    public long Size
    {
        get => _size;
        set
        {
            if(value < 0)
            {
                throw new NegativeSizeException(this);
            }

            _size = value;

            if (Type == FsType.File)
            {
                AddToPerents(_size);
            }

            CalculatePercent();

            OnPropertyChanged();
        }
    }

    public float Percent
    {
        get => _percent;
        set
        {
            if (value < 0)
            {
                throw new NegativePercentException(this);
            }
            _percent = value;
            OnPropertyChanged();
        }
    }

    public TreeNode Perent
    {
        get => _perent;
        set => _perent = value;
    }

    public ICollection<TreeNode> SubNodes
    {
        get => _subNodes;
        set
        {
            _subNodes = value;
            OnPropertyChanged();
        }
    }

    public TreeNode(DirectoryInfo dir, TreeNode perent) : this(dir.Name, perent)
    {
        Path = dir.FullName;
        Type = FsType.Directory;
    }

    public TreeNode(FileInfo file, TreeNode perent) : this(file.Name, perent)
    {
        Path = file.FullName;
        Type = FsType.File;
        Size = file.Length;
    }

    private TreeNode(string name, TreeNode perent)
    {
        Name = name;
        SubNodes = new ObservableCollection<TreeNode>();
        Perent = perent;
    }

    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    private void AddToPerents(long size)
    {
        if (Perent == null)
        {
            return;
        }
        Perent.Size += size;
        Perent.AddToPerents(size);
    }

    private void CalculatePercent()
    {
        if (SubNodes.Count == 0 || Size <= 0)
        {
            return;
        }

        foreach(var node in SubNodes)
        {
            node.Percent = (float)node.Size / Size;
        }
    }
}


