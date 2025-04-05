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
            ViewModel.MainWindow = this;
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
                    Background = Brushes.LightGreen,
                };

                btn.Click += Field_Click;
                btn.PreviewMouseRightButtonDown += Field_RightClick;

                GameGrid.Children.Add(btn);
            }
        }

        private void Field_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int index) ViewModel.RevealField(btn, index);
        }

        private void Field_RightClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int index) ViewModel.MarkField(btn, index);
        }

        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.NewGame();

            foreach (var child in GameGrid.Children)
            {
                if (child is Button btn) SetButtonAsDefault(btn);
            }

            Background = Brushes.White;
        }

        public void SetButtonAsDefault(Button btn)
        {
            btn.Content = "";
            btn.Background = Brushes.LightGreen;
            btn.IsEnabled = true;
        }

        public void SetButtonAsMarked(Button btn, Field field)
        {
            btn.Content = field.IsMarked ? MarkSign : "";
        }

        public void SetButtonAsExposed(Button btn, Field field)
        {
            if (field.BombsAround > 0) btn.Content = field.BombsAround.ToString();
            else btn.Content = "";
            btn.Background = Brushes.LightGray;
        }

        public void SetButtonAsBomb(Button btn)
        {
            btn.Content = BombSign;
            btn.Background = Brushes.Red;
        }

        public void DisableAllButtons()
        {
            foreach (var child in GameGrid.Children)
            {
                if (child is Button btn) btn.IsEnabled = false;
            }
        }

        public void RefreshReveal()
        {
            for (int i = 0; i < GameGrid.Children.Count; i++)
            {
                if (GameGrid.Children[i] is Button btn)
                {
                    var field = ViewModel.Fields[i];
                    if (field.IsExposed) SetButtonAsExposed(btn, field);
                }
            }
        }

        private void OnGameWon()
        {
            DisableAllButtons();
            Background = Brushes.LightGreen;
        }
    }
}