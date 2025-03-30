namespace Minesweeper.Models
{
    public class Board
    {
        public List<Field> Fields { get; set; }

        public Board()
        {
            Fields = new List<Field>();
            for (int i = 0; i < 100; i++) Fields.Add(new Field());

            List<Field> bombFields = new List<Field>();
            Random rnd = new Random();
            while (bombFields.Count < 30)
            {
                int fieldIndex = rnd.Next(0, 100);
                if (!bombFields.Contains(Fields[fieldIndex])) Fields[fieldIndex].IsBomb = true;
            }
        }
    }
}
