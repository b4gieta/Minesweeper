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
                    Width = 40,
                    Height = 40,
                    Tag = i,
                    Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#090c12"),
                    Style = (Style)FindResource("MinefieldButton"),
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

            Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#1a2336");
        }

        public void SetButtonAsDefault(Button btn)
        {
            btn.Content = "";
            btn.IsEnabled = true;
            btn.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#090c12");
            btn.Style = (Style)FindResource("MinefieldButton");
        }

        public void SetButtonAsMarked(Button btn, Field field)
        {
            btn.Content = field.IsMarked ? MarkSign : "";
        }

        public void SetButtonAsExposed(Button btn, Field field)
        {
            if (field.BombsAround > 0) btn.Content = field.BombsAround.ToString();
            else btn.Content = "";
            btn.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#3a3c40");
            btn.Style = (Style)FindResource("MinefieldButtonRevealed");
        }

        public void SetButtonAsBomb(Button btn)
        {
            btn.Content = BombSign;
            btn.Background = Brushes.Red;
            Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#4d1a1a");
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
            Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#538f5c");
        }
    }
}