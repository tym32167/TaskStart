using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using System.Windows.Shell;
using TaskStart.Tasks;
using Application = System.Windows.Application;

namespace TaskStart.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        readonly JumpList _jumpList;

        public MainViewModel()
        {
            _jumpList = new JumpList();
            JumpList.SetJumpList(Application.Current, _jumpList);

            _tasks = new ObservableCollection<TaskViewModel>();

            Load();
            Apply();

            ApplyCommand = new DelegateCommand(ob => Apply());
        }

        public ICommand ApplyCommand { get; set; }

        private ObservableCollection<TaskViewModel> _tasks;
        public ObservableCollection<TaskViewModel> Tasks
        {
            get => _tasks;
            set
            {
                if (_tasks != value)
                {
                    _tasks = value;
                    OnPropertyChanged(nameof(Tasks));
                }
            }
        }


        public void Apply()
        {
            _jumpList.JumpItems.Clear();

            var jumpTasks = Tasks.Select(task => new JumpTask()
            {
                Title = task.Title ?? string.Empty,
                Description = task.Description ?? string.Empty,
                ApplicationPath = task.ApplicationPath ?? string.Empty,
                Arguments = task.ApplicationArguments ?? string.Empty,
                IconResourcePath = task.ApplicationPath ?? string.Empty,
                WorkingDirectory = Path.GetDirectoryName(task.ApplicationPath),
                CustomCategory = task.Category ?? string.Empty,
            }).ToList();


            jumpTasks.Reverse();
            jumpTasks.ForEach(_jumpList.JumpItems.Add);
            _jumpList.Apply();
            Save();
        }

        public void Save()
        {
            Settings.Instance.SetTasks(Tasks.Select(x => x.GetTask()));
        }

        public void Load()
        {
            try
            {
                var tasks = Settings.Instance.GetTasks();
                foreach (var task in tasks.OrderBy(x => x.Category).ThenBy(x => x.Title))
                {
                    _tasks.Add(new TaskViewModel(task));
                }
            }
            catch
            {
                _tasks = new ObservableCollection<TaskViewModel>();
            }
        }

        public void Add(string fileName)
        {
            var task = new Task { ApplicationPath = fileName, Category = string.Empty, Title = Path.GetFileNameWithoutExtension(fileName) };
            _tasks.Add(new TaskViewModel(task));
        }
    }
}