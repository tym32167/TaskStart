using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using GalaSoft.MvvmLight.Command;
using TaskStart.Tasks;

namespace TaskStart.ViewModel
{
	public class TaskViewModel
	{
		private readonly Task _task;

		/// <summary>
		/// Конструктор. Принимает Task и заполняет нужные поля данными. Также инициализирует команду запуска приложения.
		/// </summary>
		/// <param name="task"></param>
		public TaskViewModel( Task task)
		{
			_task = task;
			
			ApplicationPath = _task.ApplicationPath;
			Category = _task.Category;
			Description = _task.Description;
			Title = _task.Title;

			// Запуск приложения
			StartCommand = new RelayCommand(()=>
			                                	{
													if (File.Exists(ApplicationPath))
														Process.Start(ApplicationPath);
			                                	},
												()=> File.Exists(ApplicationPath));
		}

		/// <summary>
		/// Применяется для получения экземпляра Task - понадобится при сохранении информации из интерфейса в файл
		/// </summary>
		/// <returns></returns>
		public Task GetTask()
		{
			return new Task
			       	{
						// Это поле нельзя изменять через интерфейс. Для этого я и оставил переменную  _task
						ApplicationPath = _task.ApplicationPath,
						Category = Category,
						Description = Description,
						Title = Title
			       	};
		}

		
		/// <summary>
		/// Публичные поля
		/// </summary>
		public string ApplicationPath { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public string Category { get; set; }
		
		// Команды
		public ICommand StartCommand { get; set; }
		
		/// <summary>
		/// Получение иконки. Используется для показа иконки приложения.
		/// Как ни старался, так и не решил проблему с цветом. То есть у результирующей иконки палитра цветов отличается от оригинальной.
		/// </summary>
		private object _icon;
		public object Icon
		{
			get
			{
				if (_icon == null)
				{
					try
					{
						var ico = System.Drawing.Icon.ExtractAssociatedIcon(ApplicationPath);

						if (ico != null)
						{
							using (var strm = new MemoryStream())
							{
								ico.Save(strm);
								var ibd = new IconBitmapDecoder(strm, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
								var frame = ibd.Frames.FirstOrDefault();
								_icon = frame;
							}
						}
					}
					catch
					{
						_icon = null;
					}
				}
				return _icon;
			}
		}
	}
}