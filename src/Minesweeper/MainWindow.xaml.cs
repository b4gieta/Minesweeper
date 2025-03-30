using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Minesweeper.ViewModels;

namespace Minesweeper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
            GenerateFields();
        }

        private void GenerateFields()
        {
            for (int i = 0; i < 100; i++)
            {
                Button btn = new Button
                {
                    Content = "?",
                    FontSize = 24,
                    Width = 40,
                    Height = 40,
                    Tag = i,
                };
                GameGrid.Children.Add(btn);
            }
        }
    }
}