using System.IO;
using System.Windows.Input;
using TaskStart.ViewModel;
using Application = System.Windows.Application;
using DataObject = System.Windows.DataObject;
using DragEventArgs = System.Windows.DragEventArgs;

namespace TaskStart
{
	public partial class MainWindow
	{
		private readonly MainViewModel _model;
		public MainWindow()
		{
			InitializeComponent();

			Closing += MainWindowClosing;
			var locator = new ViewModelLocator();
			_model = locator.Main;
			DataContext = _model;
		}

		void MainWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			Application.Current.Shutdown();
		}

		
		private void CloseCommandHandler(object sender, ExecutedRoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}

		
		private void CanExecuteHandler(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}


		private void WindowDrop(object sender, DragEventArgs e)
		{
			if (e.Data is DataObject && ((DataObject)e.Data).ContainsFileDropList())
			{
				foreach (string filePath in ((DataObject)e.Data).GetFileDropList())
				{
					if (File.Exists(filePath))
						_model.Add(filePath);
				}
			}
		}
	}
}
