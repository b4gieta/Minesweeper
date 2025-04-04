﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using Minesweeper.Models;

namespace Minesweeper.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Field> Fields { get; set; }
        public Board Board { get; set; }

        public MainViewModel()
        {
            Board = new Board();
            Fields = new ObservableCollection<Field>(Board.Fields);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
