using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using InventoryWpfApp.Views;

namespace InventoryWpfApp
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly DatabaseService _db;
        private UserControl _currentView;
        private List<TopicModel> _topics;
        private bool _isLoading;
        private string _status;

        public MainViewModel()
        {
            string conn = "Host=localhost;Port=5432;Database=AlgebraDB;Username=postgres;Password=sa";
            _db = new DatabaseService(conn);
            SelectTopicCommand = new RelayCommand(SelectTopic);
            SelectToolCommand = new RelayCommand(SelectTool);
            LoadTopics();
        }

        public UserControl CurrentView { get => _currentView; set { _currentView = value; OnPropertyChanged(nameof(CurrentView)); } }
        public bool IsLoading { get => _isLoading; set { _isLoading = value; OnPropertyChanged(nameof(IsLoading)); } }
        public string StatusText { get => _status; set { _status = value; OnPropertyChanged(nameof(StatusText)); } }
        public ICommand SelectTopicCommand { get; }
        public ICommand SelectToolCommand { get; }

        private async void LoadTopics()
        {
            IsLoading = true; StatusText = "Загрузка тем...";
            try
            {
                _topics = await Task.Run(() => _db.GetTopics());
                if (_topics?.Count > 0) await ShowTopic(_topics[0]);
                else StatusText = "Темы не найдены.";
            }
            catch (System.Exception ex) { MessageBox.Show(ex.Message); }
            finally { IsLoading = false; }
        }

        private async void SelectTopic(object param)
        {
            if (!int.TryParse(param?.ToString(), out int id)) return;
            var topic = _topics?.Find(t => t.Id == id);
            if (topic != null) await ShowTopic(topic);
        }

        private async Task ShowTopic(TopicModel topic)
        {
            IsLoading = true;
            StatusText = $"Загрузка: {topic.Title}...";

            var vm = new TopicViewModel(_db, topic.Id, topic.Title);
            var view = new TopicView { DataContext = vm };

            // Переключаем View в UI-потоке ДО загрузки контента
            CurrentView = view;

            // Загружаем контент
            await vm.LoadContentAsync();

            IsLoading = false;
            StatusText = "";
        }

        private void SelectTool(object param)
        {
            switch (param?.ToString())
            {
                case "equation": CurrentView = new EquationToolView(); break;
                case "squaretable": CurrentView = new SquareTableView(); break;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}