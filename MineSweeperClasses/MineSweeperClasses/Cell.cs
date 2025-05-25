
namespace MineSweeperClasses
{
    public class Cell
    {
        public bool HasBomb { get; set; }
        public int BombNeighbors { get; set; }
        public bool HasReward { get; set; }

        public bool IsVisited { get; set; }
        public bool IsFlagged { get; set; }
        public int AdjacentBombs { get; set; }

        public Cell()
        {
            HasBomb = false;
            BombNeighbors = 0;
            HasReward = false;
            IsVisited = false;
            IsFlagged = false;
        }
    }
}
