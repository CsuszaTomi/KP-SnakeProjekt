using KP_SnakeProjekt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KP_SnakeProjekt.View
{
    internal class SnakeView
    {
        /// <summary>
        /// A testresz forgatását számítja ki 
        /// </summary>  
        /// <param name="current">A jelenlegi testresz</param>
        /// <param name="lead">A vezető testresz</param>
        /// <returns>A forgatási szög</returns>
        public static double CalculateBodyRotation(Snake current, Snake lead)
        {
            if (lead.PosX > current.PosX) return 270; // Jobbra
            if (lead.PosX < current.PosX) return 90;  // Balra
            if (lead.PosY > current.PosY) return 0;   // Le
            if (lead.PosY < current.PosY) return 180; // Fel
            return 0;
        }

        /// <summary>
        /// A sarok forgatását számítja ki az irányvektorok alapján
        /// </summary>
        /// <param name="InX">A bejövő delta x</param>
        /// <param name="InY">A bejövő delta y</param>
        /// <param name="OutX">A kimenő delta x</param>
        /// <param name="OutY">A kimenő delta y</param>
        /// <returns>A forgatási szög</returns>
        public static double CalculateCornerRotation(int InX, int InY, int OutX, int OutY)
        {
            // Irányvektorok alapján meghatározzuk a sarok forgatását
            if ((InY == -1 && OutX == 1) || (InX == 1 && OutY == -1)) return 0;   // Bal-Felső sarok
            if ((InX == 1 && OutY == 1) || (InY == 1 && OutX == 1)) return 90;  // Bal-Alsó sarok
            if ((InY == 1 && OutX == -1) || (InX == -1 && OutY == 1)) return 180; // Jobb-Alsó sarok
            if ((InX == -1 && OutY == -1) || (InY == -1 && OutX == -1)) return 270; // Jobb-Felső sarok
            return 0;
        }
    }
}
