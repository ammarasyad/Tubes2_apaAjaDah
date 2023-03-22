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
    public class Node
    {
        private string _color = "Black";
        
        public Node(int x, int y, char value)
        {
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
                _color = value;
                ColorChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public static string DecreaseColorByFactor(string color, int factor)
        {
            var colorValue = ColorTranslator.FromHtml(color);
            var newColor = System.Drawing.Color.FromArgb(colorValue.A, colorValue.R - factor, colorValue.G - factor, colorValue.B - factor);
            return ColorTranslator.ToHtml(newColor);
        }
    }
}
