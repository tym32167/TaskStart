namespace TaskStart.ViewModel
{
    public class ViewModelLocator
    {
        private static MainViewModel _main;

        public ViewModelLocator()
        {
            _main = new MainViewModel();
        }

        public MainViewModel Main => _main;
    }
}