using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace InventoryWpfApp.Views
{
    public partial class SquareTableView : UserControl
    {
        public SquareTableView()
        {
            InitializeComponent();
        }

        private void ShowTableButton_Click(object sender, RoutedEventArgs e)
        {
            // Скрываем ошибку
            ErrorPanel.Visibility = Visibility.Collapsed;

            // Проверка ввода
            if (!int.TryParse(TbFrom.Text, out int from))
            {
                ShowError("Поле 'От' должно быть целым числом!");
                return;
            }
            if (!int.TryParse(TbTo.Text, out int to))
            {
                ShowError("Поле 'До' должно быть целым числом!");
                return;
            }

            // Проверка диапазона
            if (from > to)
            {
                ShowError("Значение 'От' не может быть больше 'До'!");
                return;
            }

            if (to - from > 100)
            {
                ShowError("Максимальный диапазон — 100 чисел. Введите меньший диапазон.");
                return;
            }

            // Формируем таблицу
            var squares = new List<SquareRow>();
            for (int i = from; i <= to; i++)
            {
                squares.Add(new SquareRow
                {
                    Number = i,
                    Square = i * i
                });
            }

            DgSquares.ItemsSource = squares;
            DgSquares.Visibility = Visibility.Visible;
        }

        private void ShowError(string message)
        {
            TbError.Text = message;
            ErrorPanel.Visibility = Visibility.Visible;
            DgSquares.Visibility = Visibility.Collapsed;
        }
    }

    public class SquareRow
    {
        public int Number { get; set; }
        public int Square { get; set; }
    }
}