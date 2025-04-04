namespace Minesweeper.Models
{
    public class Field
    {
        public bool IsBomb { get; set; }
        public bool IsExposed { get; set; }
        public bool IsMarked { get; set; }
        public int BombsAround { get; set; }
    }
}
