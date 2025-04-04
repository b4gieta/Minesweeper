namespace Minesweeper.Models
{
    public class Board
    {
        public List<Field> Fields { get; set; }

        public Board()
        {
            Fields = new List<Field>();
            for (int i = 0; i < 100; i++) Fields.Add(new Field());

            Random rnd = new Random();
            HashSet<int> bombIndexes = new HashSet<int>();

            while (bombIndexes.Count < 30)
            {
                int fieldIndex = rnd.Next(0, 100);
                bombIndexes.Add(fieldIndex);
            }

            foreach (int index in bombIndexes)
            {
                Fields[index].IsBomb = true;
            }
        }
    }
}
