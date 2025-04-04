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
        private readonly string BombSign = "💣";
        private readonly string MarkSign = "⚑";

        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new MainViewModel();
            DataContext = ViewModel;
            GenerateFields();
            ViewModel.GameWon += OnGameWon;
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
                btn.PreviewMouseRightButtonDown += Field_RightClick;

                GameGrid.Children.Add(btn);
            }
        }

        private void Field_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int index)
            {
                Field field = ViewModel.Fields[index];

                if (!ViewModel.Board.BombsPlaced) ViewModel.Board.PlaceBombs(index);

                if (field.IsExposed) return;
                field.IsExposed = true;

                if (field.IsBomb)
                {
                    btn.Content = BombSign;
                    btn.Background = Brushes.Red;
                    DisableAllButtons();
                }
                else
                {
                    if (field.BombsAround > 0) btn.Content = field.BombsAround.ToString();
                    else btn.Content = "";
                    btn.Background = Brushes.LightGray;
                    if (field.BombsAround == 0)
                    {
                        ViewModel.Board.RevealEmptyFields(index);
                        RefreshReveal();
                    }
                }

                ViewModel.CheckVictory();
            }
        }

        private void Field_RightClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int index)
            {
                var field = ViewModel.Fields[index];

                if (!field.IsExposed)
                {
                    field.IsMarked = !field.IsMarked;
                    btn.Content = field.IsMarked ? MarkSign : "";
                }
            }
        }

        private void DisableAllButtons()
        {
            foreach (var child in GameGrid.Children)
            {
                if (child is Button btn) btn.IsEnabled = false;
            }
        }

        private void RefreshReveal()
        {
            for (int i = 0; i < GameGrid.Children.Count; i++)
            {
                if (GameGrid.Children[i] is Button btn)
                {
                    var field = ViewModel.Fields[i];

                    if (field.IsExposed)
                    {
                        if (field.BombsAround > 0) btn.Content = field.BombsAround.ToString();
                        else btn.Content = "";
                        btn.Background = Brushes.LightGray;
                    }
                }
            }
        }

        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.NewGame();

            foreach (var child in GameGrid.Children)
            {
                if (child is Button btn)
                {
                    btn.Content = "";
                    btn.Background = Brushes.LightGreen;
                    btn.IsEnabled = true;
                }
            }

            Background = Brushes.White;
        }

        private void OnGameWon()
        {
            DisableAllButtons();
            Background = Brushes.LightGreen;
        }
    }
}