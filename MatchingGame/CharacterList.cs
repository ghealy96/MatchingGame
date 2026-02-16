using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MatchingGame
{
    class CharacterList
    {

        


        public string[] setCharacters(int size)
        {
            List<string> list = new List<string>()
            {
                "🦑","🦘","🐸","🦙","🦍","🐘","🦉","🐨","🐯","🦝","🦧",
                "🐎","🐮","🦔","🦇","🐼","🐧","🐋","🦭","🦈","🦞","🐞"
            };

            string[] selectedList = choseList(list, size);

            string[] returnList = Shuffle(selectedList,size);

            Console.WriteLine(returnList);  

            return returnList;            
      
        }

        public string[] Shuffle(string[] fullList,int size)
        {
            string[] returnList = new string[size];
            List<int> placement = new List<int>();
            for (int i = 0; i < size; i++) { placement.Add(i); }

          

            for (int c = 0; c < (size); c++)
            {
                int rand = Random.Shared.Next(0, placement.Count);
                returnList[c] = fullList[ placement[rand] % fullList.Length ];
                placement.RemoveAt(rand);

            }
            return returnList;
        }

        public string[] choseList(List<string> fullList, int size)
        {
            string[] returnList = new string[size/2];
            for (int i = 0; i < size / 2; i++)
            {
                int rand = Random.Shared.Next(0, fullList.Count);
                returnList[i] = fullList[rand];
                fullList.RemoveAt(rand);
            }
            return returnList;
        }


    }
}
