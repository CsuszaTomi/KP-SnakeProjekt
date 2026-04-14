using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KP_SnakeProjekt.View
{
    internal class UIHandler
    {
        public static void ShowError(string msg, System.Windows.Controls.TextBlock txtError)
        {
            txtError.Text = msg;
            txtError.Visibility = Visibility.Visible;
        }
    }
}
