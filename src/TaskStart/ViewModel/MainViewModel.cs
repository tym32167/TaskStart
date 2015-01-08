using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using System.Windows.Shell;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using TaskStart.Tasks;
using Application = System.Windows.Application;

namespace TaskStart.ViewModel
{
	public class MainViewModel : ViewModelBase
	{
		/// <summary>
		/// Список переходов
		/// </summary>
		readonly JumpList _jumpList;

		public MainViewModel()
		{
			// Инициализируем список и коллекцию 
			_jumpList = new JumpList();
			JumpList.SetJumpList(Application.Current, _jumpList);

			_tasks = new ObservableCollection<TaskViewModel>();
			
			// Загружаем из файла задачи
			Load();

			// Обновляем список переходов
			Apply();

			// Команда обновления списка переходов
			ApplyCommand = new RelayCommand(Apply);
		}
		
		/// <summary>
		/// Команды
		/// </summary>
		public ICommand ApplyCommand { get; set; }

		/// <summary>
		/// Коллекция задач
		/// </summary>
		private ObservableCollection<TaskViewModel> _tasks;
		public ObservableCollection<TaskViewModel> Tasks
		{
			get { return _tasks; }
			set
			{
				if (_tasks != value)
				{
					_tasks = value;
					RaisePropertyChanged("Tasks");
				}
			}
		}


		/// <summary>
		/// Функция, перезаписывает список переходов.
		/// </summary>
		public void Apply()
		{
			// очистка существующего списка
			_jumpList.JumpItems.Clear();

			// получаем JumpTask
			var jumpTasks =
				(from task in Tasks.Select(x=>x.GetTask())
				 where File.Exists(task.ApplicationPath)
				 orderby task.Title, task.Category 
				 select new JumpTask
				        	{
				        		Title = task.Title ?? string.Empty,
				        		Description = task.Description ?? string.Empty,
				        		ApplicationPath = task.ApplicationPath ?? string.Empty,
				        		IconResourcePath = task.ApplicationPath ?? string.Empty,
				        		WorkingDirectory = Path.GetDirectoryName(task.ApplicationPath),
				        		CustomCategory = task.Category ?? string.Empty,
				        	}).ToList();
			// Шаманим с сортировкой. Этот код вообще не обязателен, просто мне нужен был список в определенном порядке
			jumpTasks.Reverse();
			// добавляем все JumpTask в список jumpTasks
			jumpTasks.ForEach(_jumpList.JumpItems.Add);
			// применяем изменения
			_jumpList.Apply();
			// сохраняем список в файл
			Save();
		}

		// Сохранение списка в файл
		public void Save()
		{
			Settings.Instance.SetTasks(Tasks.Select(x=>x.GetTask()));
		}

		// Загрузка списка из файла
		public void Load()
		{
			try
			{
				var tasks = Settings.Instance.GetTasks();
				foreach (var task in tasks.OrderBy(x=>x.Category).ThenBy(x=>x.Title))
				{
					_tasks.Add(new TaskViewModel(task));
				}
			}
			catch
			{
				_tasks = new ObservableCollection<TaskViewModel>();
			}
		}

		// Добавление нового элемента в список. 
		public void Add(string fileName)
		{
			var task = new Task { ApplicationPath = fileName, Category = string.Empty, Title = Path.GetFileNameWithoutExtension(fileName) };
			_tasks.Add(new TaskViewModel(task));
		}
	}
}