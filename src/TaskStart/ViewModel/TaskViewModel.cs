using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using TaskStart.Tasks;

namespace TaskStart.ViewModel
{
    public class TaskViewModel : ViewModelBase
    {
        private readonly Task _task;

        public TaskViewModel(Task task)
        {
            _task = task;

            StartCommand = new DelegateCommand((ob) => Start());
        }

        private void Start()
        {
            var process = new Process();
            var startInfo = new ProcessStartInfo { FileName = _task.ApplicationPath };

            if (!string.IsNullOrEmpty(_task.ApplicationArguments))
            {
                startInfo.Arguments = _task.ApplicationArguments;
            }

            process.StartInfo = startInfo;
            process.Start();
        }

        public Task GetTask()
        {
            return _task;
        }

        public string ApplicationPath
        {
            get => _task.ApplicationPath;
            set => _task.ApplicationPath = value;
        }

        public string ApplicationArguments
        {
            get => _task.ApplicationArguments;
            set => _task.ApplicationArguments = value;
        }

        public string Title
        {
            get => _task.Title;
            set => _task.Title = value;
        }


        public string Description
        {
            get => _task.Description;
            set => _task.Description = value;
        }

        public string Category
        {
            get => _task.Category;
            set => _task.Category = value;
        }

        public ICommand StartCommand { get; set; }

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