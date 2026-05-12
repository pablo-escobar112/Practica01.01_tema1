using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace InventoryWpfApp
{
    public static class ContentParser
    {
        public static FlowDocument Parse(List<ContentBlockModel> blocks)
        {
            // Создаём FlowDocument в текущем (UI) потоке
            var doc = new FlowDocument
            {
                FontSize = 14,
                FontFamily = new FontFamily("Segoe UI")
            };

            foreach (var block in blocks)
            {
                var p = new Paragraph
                {
                    Margin = new Thickness(0, 3, 0, 3)
                };

                switch (block.Tag.ToLower())
                {
                    case "h":
                        p.FontSize = 18;
                        p.FontWeight = FontWeights.Bold;
                        p.Foreground = new SolidColorBrush(Color.FromRgb(44, 62, 80));
                        p.Margin = new Thickness(0, 15, 0, 5);
                        p.Inlines.Add(new Run(block.Text));
                        break;
                    case "b":
                        p.FontWeight = FontWeights.Bold;
                        p.Inlines.Add(new Run(block.Text));
                        break;
                    case "p":
                        p.Inlines.Add(new Run(block.Text));
                        break;
                    case "example":
                        p.Background = new SolidColorBrush(Color.FromRgb(245, 245, 245));
                        p.Padding = new Thickness(10, 5, 10, 5);
                        p.FontStyle = FontStyles.Italic;
                        p.Inlines.Add(new Run("• " + block.Text));
                        break;
                    case "formula":
                        p.Background = new SolidColorBrush(Color.FromRgb(232, 248, 245));
                        p.Padding = new Thickness(15, 8, 15, 8);
                        p.BorderBrush = new SolidColorBrush(Color.FromRgb(39, 174, 96));
                        p.BorderThickness = new Thickness(1);
                        p.FontSize = 15;
                        p.Inlines.Add(new Run(block.Text));
                        break;
                    default:
                        p.Inlines.Add(new Run(block.Text));
                        break;
                }
                doc.Blocks.Add(p);
            }
            return doc;
        }
    }
}