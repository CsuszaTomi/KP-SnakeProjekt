using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KP_SnakeProjekt.Models
{
    class Snake
    {
        public Snake(int posX, int posY, int lastPosX, int lastPosY, int applesEaten = 0)
        {
            PosX = posX;
            PosY = posY;
            LastPosX = lastPosX;
            LastPosY = lastPosY;
            ApplesEaten = applesEaten;
        }
        public Snake() { }

        public int PosX { get; set; }
        public int PosY { get; set; }

        public int LastPosX { get; set; }
        public int LastPosY { get; set; }

        public int ApplesEaten { get; set; }


    }
}
