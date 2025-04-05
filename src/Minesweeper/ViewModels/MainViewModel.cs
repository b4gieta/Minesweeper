using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using Minesweeper.Models;

namespace Minesweeper.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Field> Fields { get; set; }
        public Board Board { get; set; }

        public event Action? GameWon;

        public MainWindow MainWindow { get; set; }

        public MainViewModel()
        {
            NewGame();
        }

        public void NewGame()
        {
            Board = new Board();
            Fields = new ObservableCollection<Field>(Board.Fields);
        }

        public void CheckVictory()
        {
            if (Board.CheckVictory()) GameWon?.Invoke();
        }

        public void RevealField(Button btn, int index)
        {
            Field field = Fields[index];

            if (!Board.BombsPlaced) Board.PlaceBombs(index);

            if (field.IsExposed) return;
            field.IsExposed = true;

            if (field.IsBomb)
            {
                MainWindow.SetButtonAsBomb(btn);
                MainWindow.DisableAllButtons();
            }
            else
            {
                MainWindow.SetButtonAsExposed(btn, field);
                if (field.BombsAround == 0)
                {
                    Board.RevealEmptyFields(index);
                    MainWindow.RefreshReveal();
                }
            }

            CheckVictory();
        }

        public void MarkField(Button btn, int index)
        {
            var field = Fields[index];
            if (!field.IsExposed)
            {
                field.IsMarked = !field.IsMarked;
                MainWindow.SetButtonAsMarked(btn, field);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
