using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Threading;
using Minesweeper.Models;

namespace Minesweeper.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Field> Fields { get; set; }
        public Board Board { get; set; }

        public event Action? GameWon;

        public MainWindow MainWindow { get; set; }

        private int _bombsLeft;
        public int BombsLeft
        {
            get => _bombsLeft;
            set
            {
                _bombsLeft = value;
                OnPropertyChanged(nameof(BombsLeft));
            }
        }

        private int _timerCount;
        public int TimerCount
        {
            get => _timerCount;
            set
            {
                _timerCount = value;
                OnPropertyChanged(nameof(TimerCount));
            }
        }

        private DispatcherTimer _timer;

        public MainViewModel()
        {
            InitializeTimer();
            NewGame();
        }

        public void NewGame()
        {
            Board = new Board();
            Fields = new ObservableCollection<Field>(Board.Fields);
            BombsLeft = 10;
            TimerCount = 0;
            _timer.Start();
        }

        private void InitializeTimer()
        {
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += (sender, args) => TimerCount++;
            _timer.Start();
        }

        public void CheckVictory()
        {
            if (Board.CheckVictory())
            {
                GameWon?.Invoke();
                _timer.Stop();
            }
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

            RefreshBombsLeft();
            CheckVictory();
        }

        public void MarkField(Button btn, int index)
        {
            var field = Fields[index];
            if (!field.IsExposed)
            {
                field.IsMarked = !field.IsMarked;
                MainWindow.SetButtonAsMarked(btn, field);
                RefreshBombsLeft();
            }
        }

        private void RefreshBombsLeft()
        {
            BombsLeft = 10 - Fields.Where(f => f.IsMarked && !f.IsExposed).Count();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
