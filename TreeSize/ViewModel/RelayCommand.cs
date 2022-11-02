using System.Windows.Input;
using System;

namespace TreeSize.ViewModel;

class RelayCommand : ICommand
{
    public event EventHandler CanExecuteChanged;
    private readonly Action _execute;

    public RelayCommand(Action execute)
    {
        _execute = execute;
    }

    public bool CanExecute(object parameter)
    {
        return true;
    }

    public void Execute(object parameter)
    {
        _execute();  
    }
}