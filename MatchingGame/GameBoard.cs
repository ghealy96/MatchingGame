
namespace MatchingGame
{
    class GameBoard
    {
        public int rowSize { get; private set; }
        public int columnSize { get; private set; }
        public Tile[,] grid { get; private set; }

        CharacterList chart = new CharacterList();
        string[] characters;
        public Tile lastSelectedTile { get; private set; }

        


        public GameBoard(int row,int column)
        {
            this.rowSize = row;
            this.columnSize = column;
            grid = new Tile[row,column];
            characters = chart.setCharacters(row * column);    
            BuildSquareBoard();
        }

        private void BuildSquareBoard()
        {
            

            for (int i = 0; i < rowSize; i++) { }

            for (int i = 0; i < columnSize; i++) {}


            for (int row = 0; row < rowSize; row++)
            {
                for (int column = 0; column < columnSize; column++)
                {
                    grid[row, column] = new Tile
                    {
                        Id = row * columnSize + column,
                        row = row,
                        col = column,
                        Value = characters[row * columnSize + column],
                        IsFlipped = false,
                        IsMatched = false

                    };                                
                }
            }        
        }

        public bool ProcessSelection(Tile clickedTile)
        {
            if (lastSelectedTile == null) { lastSelectedTile = clickedTile; return false; }
            if(lastSelectedTile.Id == clickedTile.Id) {  return false; }

            if (lastSelectedTile.Value == clickedTile.Value)
            {
                clickedTile.IsMatched = true;
                lastSelectedTile.IsMatched=true;
                lastSelectedTile = null;
                return true;
            }

            lastSelectedTile = null;
            return false;

        }

        
    }
}
