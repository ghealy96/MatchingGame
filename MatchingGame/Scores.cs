using System;
using System.Collections.Generic;
using System.Text;

namespace MatchingGame
{
    class Scores
    {
        string scoreType;
        int time=1;
        string name="EMPTY";
        int missedClicks;
        int matches;
        int place;

        public string ScoreType { get => scoreType; set => scoreType = value; }
        public int Time { get => time; set => time = value; }
        public string Name { get => name; set => name = value; }
        public int MissedClicks { get => missedClicks; set => missedClicks = value; }

        public int Matches { get => matches; set => matches = value;  }

        public int Place { get => place; set => place = value;}
    }
}
