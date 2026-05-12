using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace InventoryWpfApp
{
    public class TopicViewModel : INotifyPropertyChanged
    {
        private readonly DatabaseService _db;
        private readonly int _topicId;
        private FlowDocument _content;
        private bool _isLoading;
        private string _error;
        private string _title;

        public TopicViewModel(DatabaseService db, int topicId, string title)
        {
            _db = db;
            _topicId = topicId;
            _title = title;
        }

        public string TopicTitle
        {
            get => _title;
            set { _title = value; OnPropertyChanged(nameof(TopicTitle)); }
        }

        public FlowDocument Content
        {
            get => _content;
            set { _content = value; OnPropertyChanged(nameof(Content)); }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set { _isLoading = value; OnPropertyChanged(nameof(IsLoading)); }
        }

        public string ErrorMessage
        {
            get => _error;
            set { _error = value; OnPropertyChanged(nameof(ErrorMessage)); }
        }

        public async Task LoadContentAsync()
        {
            IsLoading = true;
            ErrorMessage = null;

            try
            {
                // Шаг 1: загружаем данные из БД в фоновом потоке
                List<ContentBlockModel> blocks = await Task.Run(() =>
                {
                    return _db.GetContentBlocks(_topicId);
                });

                if (blocks == null || blocks.Count == 0)
                {
                    ErrorMessage = "Нет данных по теме.";
                    return;
                }

                // Шаг 2: парсим в FlowDocument в UI-потоке
                Content = ContentParser.Parse(blocks);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Ошибка загрузки: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}