using System;
using System.Windows;
using System.Windows.Controls;

namespace InventoryWpfApp.Views
{
    public partial class EquationToolView : UserControl
    {
        public EquationToolView()
        {
            InitializeComponent();
        }

        private void SolveButton_Click(object sender, RoutedEventArgs e)
        {
            // Скрываем предыдущие результаты
            ResultPanel.Visibility = Visibility.Collapsed;
            ErrorPanel.Visibility = Visibility.Collapsed;

            // Проверка ввода
            if (!double.TryParse(TbA.Text, out double a))
            {
                ShowError("Коэффициент 'a' должен быть числом!");
                return;
            }
            if (!double.TryParse(TbB.Text, out double b))
            {
                ShowError("Коэффициент 'b' должен быть числом!");
                return;
            }
            if (!double.TryParse(TbC.Text, out double c))
            {
                ShowError("Коэффициент 'c' должен быть числом!");
                return;
            }

            // Проверка, что a ≠ 0 (это квадратное уравнение)
            if (a == 0)
            {
                ShowError("Коэффициент 'a' не может быть равен нулю! Это не квадратное уравнение.");
                return;
            }

            // Решение выбранным методом
            string solution;
            if (RbDiscriminant.IsChecked == true)
            {
                solution = SolveByDiscriminant(a, b, c);
            }
            else
            {
                solution = SolveByVieta(a, b, c);
            }

            // Показываем результат
            TbResult.Text = solution;
            ResultPanel.Visibility = Visibility.Visible;
        }

        private string SolveByDiscriminant(double a, double b, double c)
        {
            double discriminant = b * b - 4 * a * c;
            string result = "";

            result += $"Уравнение: {a}x² + {b}x + {c} = 0\n\n";
            result += $"Дискриминант D = b² - 4ac = {b}² - 4·{a}·{c} = {discriminant}\n\n";

            if (discriminant > 0)
            {
                double x1 = (-b + Math.Sqrt(discriminant)) / (2 * a);
                double x2 = (-b - Math.Sqrt(discriminant)) / (2 * a);
                result += $"D > 0, уравнение имеет два корня:\n";
                result += $"x₁ = ({-b} + √{discriminant}) / (2·{a}) = {Math.Round(x1, 4)}\n";
                result += $"x₂ = ({-b} - √{discriminant}) / (2·{a}) = {Math.Round(x2, 4)}";
            }
            else if (discriminant == 0)
            {
                double x = -b / (2 * a);
                result += $"D = 0, уравнение имеет один корень:\n";
                result += $"x = {-b} / (2·{a}) = {Math.Round(x, 4)}";
            }
            else
            {
                result += $"D < 0, уравнение не имеет действительных корней.";
            }

            return result;
        }

        private string SolveByVieta(double a, double b, double c)
        {
            // Приводим к виду x² + px + q = 0
            double p = b / a;
            double q = c / a;
            double discriminant = p * p - 4 * q;

            string result = "";
            result += $"Приведённое уравнение: x² + {p}x + {q} = 0\n\n";
            result += $"По теореме Виета:\n";
            result += $"x₁ + x₂ = -p = {-p}\n";
            result += $"x₁ · x₂ = q = {q}\n\n";

            if (discriminant > 0)
            {
                double x1 = (-p + Math.Sqrt(discriminant)) / 2;
                double x2 = (-p - Math.Sqrt(discriminant)) / 2;
                result += $"Корни уравнения:\n";
                result += $"x₁ = {Math.Round(x1, 4)}\n";
                result += $"x₂ = {Math.Round(x2, 4)}\n\n";
                result += $"Проверка:\n";
                result += $"x₁ + x₂ = {Math.Round(x1 + x2, 4)} = {-p} ✓\n";
                result += $"x₁ · x₂ = {Math.Round(x1 * x2, 4)} = {q} ✓";
            }
            else if (discriminant == 0)
            {
                double x = -p / 2;
                result += $"Один корень: x = {Math.Round(x, 4)}";
            }
            else
            {
                result += $"Действительных корней нет (D < 0).";
            }

            return result;
        }

        private void ShowError(string message)
        {
            TbError.Text = message;
            ErrorPanel.Visibility = Visibility.Visible;
        }
    }
}