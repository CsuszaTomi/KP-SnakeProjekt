using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KP_SnakeProjekt.Models
{
    public class Score
    {
        public Score(string place, string name, int scorePoint)
        {
            Place = place;
            Name = name;
            ScorePoint = scorePoint;
        }

        public Score() { }

        public string Place { get; set; }
        public string Name { get; set; }
        public int ScorePoint { get; set; }
    }
}
