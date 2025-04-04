using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Minesweeper.ViewModels;
using Minesweeper.Models;

namespace Minesweeper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel ViewModel;

        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new MainViewModel();
            DataContext = ViewModel;
            GenerateFields();
        }

        private void GenerateFields()
        {
            for (int i = 0; i < 100; i++)
            {
                Button btn = new Button
                {
                    Content = "",
                    FontSize = 24,
                    Width = 40,
                    Height = 40,
                    Tag = i,
                };

                btn.Background = Brushes.LightGreen;
                btn.Click += Field_Click;

                GameGrid.Children.Add(btn);
            }
        }

        private void Field_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int index)
            {
                Field field = ViewModel.Fields[index];

                if (field.IsExposed)
                    return;

                field.IsExposed = true;

                if (field.IsBomb)
                {
                    btn.Content = "B";
                    btn.Background = Brushes.Red;

                    MessageBox.Show("Przegrałeś.");
                    DisableAllButtons();
                }
                else
                {
                    btn.Content = "";
                    btn.Background = Brushes.LightGray;
                }
            }
        }

        private void DisableAllButtons()
        {
            foreach (var child in GameGrid.Children)
            {
                if (child is Button btn)
                {
                    btn.IsEnabled = false;
                }
            }
        }
    }
}