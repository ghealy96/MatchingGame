

namespace MatchingGame
{
    class Tile 
    {
        public int Id { get; set; }
        public int row { get; set; }
        public int col { get; set; }
        public string Value { get; set; }
        public bool IsFlipped { get; set; }
        public bool IsMatched { get; set; }       

    }
}
