using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GUI
{
    /// <summary>
    /// Node class for the matrix.
    /// </summary>
    public class Node
    {
        private string _color = "Black";
        
        public Node(int x, int y, char value)
        {
            Visited = 0;
            X = x;
            Y = y;
            Color = (Value = value) switch
            {
                'K' => "LightBlue",
                'T' => "LightYellow",
                'R' => "Transparent",
                _ => "Black"
            };
            Text = value switch
            {
                'K' => "Start",
                'T' => "Treasure",
                _ => ""
            };
        }

        public int Visited { get; set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public char Value { get; private set; }
        public string Text { get; private set; }
        public event EventHandler ColorChanged;

        public string Color
        {
            get => _color;
            set
            {
                _color = DecreaseColorByVisit(value);
                ColorChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        /// <summary>
        /// Make node darker each time it gets visited
        /// </summary>
        /// <param name="oldColor"></param>
        /// <returns></returns>
        public string DecreaseColorByVisit(string oldColor)
        {
            var colorFromString = ColorTranslator.FromHtml(oldColor);
            var newColor = System.Drawing.Color.FromArgb(colorFromString.A, 
                Math.Max(colorFromString.R - Visited*30, 0),
                Math.Max(colorFromString.G - Visited*30, 0),
                Math.Max(colorFromString.B - Visited*30, 0));
            return ColorTranslator.ToHtml(newColor);
        }
    }
}
