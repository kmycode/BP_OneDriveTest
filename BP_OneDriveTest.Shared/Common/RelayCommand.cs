using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace BP_OneDriveTest.Shared.Common
{
	/// <summary>
	/// RelayCommand
	/// </summary>
	public class RelayCommand : ICommand
	{
		private readonly Action action;
		public event EventHandler CanExecuteChanged;

		public RelayCommand(Action action)
		{
			this.action = action;
		}

		public bool CanExecute(object parameter) => true;

		public void Execute(object parameter)
		{
			this.action();
		}
	}
}
