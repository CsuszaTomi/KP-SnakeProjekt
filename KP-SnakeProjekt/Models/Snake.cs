using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KP_SnakeProjekt.Models
{
    class Snake
    {
        public Snake(int posX, int posY, int lastPosX, int lastPosY, int applesEaten = 0, int rotation = 0, bool following = false)
        {
            PosX = posX;
            PosY = posY;
            LastPosX = lastPosX;
            LastPosY = lastPosY;
            ApplesEaten = applesEaten;
            Rotation = rotation;
            Following = following;
        }
        public Snake() { }

        public int PosX { get; set; }
        public int PosY { get; set; }

        public int LastPosX { get; set; }
        public int LastPosY { get; set; }

        public int ApplesEaten { get; set; }

        public int Rotation { get; set; }

        bool Following { get; set; }

        public string GetDirection()
        {
            return Rotation switch
            {
                0 => "Down",
                270 => "Right",
                180 => "Up",
                90 => "Left",
                _ => "Unknown"
            };
        }

        enum Direction
        {
            Up,
            Right,
            Down,
            Left
        }
    }
}
