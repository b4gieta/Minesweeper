using System.Collections.ObjectModel;
using System.ComponentModel;
using Minesweeper.Models;

namespace Minesweeper.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Field> Fields { get; set; }
        public Board Board { get; set; }

        public event Action? GameWon;

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

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
