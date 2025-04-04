namespace Minesweeper.Models
{
    public class Board
    {
        public List<Field> Fields { get; set; }
        public bool BombsPlaced { get; set; }

        public Board()
        {
            Fields = new List<Field>();
            for (int i = 0; i < 100; i++) Fields.Add(new Field());
        }

        public void PlaceBombs(int clickIndex)
        {
            Random rnd = new Random();
            HashSet<int> bombIndexes = new HashSet<int>();

            while (bombIndexes.Count < 10)
            {
                int fieldIndex = rnd.Next(0, 100);
                if (fieldIndex == clickIndex) continue;
                bombIndexes.Add(fieldIndex);
            }

            foreach (int index in bombIndexes) Fields[index].IsBomb = true;
            for (int i = 0; i < Fields.Count; i++) CalculateBombsAround(i);

            BombsPlaced = true;
        }

        public void CalculateBombsAround(int index)
        {
            int row = index / 10;
            int col = index % 10;
            int bombCount = 0;

            for (int dr = -1; dr <= 1; dr++)
            {
                for (int dc = -1; dc <= 1; dc++)
                {
                    if (dr == 0 && dc == 0) continue;

                    int neighborRow = row + dr;
                    int neighborCol = col + dc;

                    if (neighborRow >= 0 && neighborRow < 10 && neighborCol >= 0 && neighborCol < 10)
                    {
                        int neighborIndex = neighborRow * 10 + neighborCol;
                        if (Fields[neighborIndex].IsBomb) bombCount++;
                    }
                }
            }
            Fields[index].BombsAround = bombCount;
        }

        public void RevealEmptyFields(int index)
        {
            int row = index / 10;
            int col = index % 10;

            for (int dr = -1; dr <= 1; dr++)
            {
                for (int dc = -1; dc <= 1; dc++)
                {
                    int newRow = row + dr;
                    int newCol = col + dc;

                    if (newRow >= 0 && newRow < 10 && newCol >= 0 && newCol < 10)
                    {
                        int newIndex = newRow * 10 + newCol;
                        var field = Fields[newIndex];
                        if (field.IsExposed) continue;
                        field.IsExposed = true;
                        if (field.BombsAround == 0) RevealEmptyFields(newIndex);
                    }
                }
            }
        }

        public bool CheckVictory()
        {
            foreach (Field field in Fields)
            {
                if (!field.IsBomb && !field.IsExposed) return false;
            }
            return true;
        }
    }
}
