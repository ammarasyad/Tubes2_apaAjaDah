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
        private string _color;
        private char _value;
        
        public Node(int x, int y, char value)
        {
            X = x;
            Y = y;
            switch (Value = value)
            {
                case 'K':
                    Color = "LightBlue";
                    break;
                case 'T':
                    Color = "LightYellow";
                    break;
                case 'R':
                    Color = "Transparent";
                    break;
                default:
                    Color = "Black";
                    break;
            }
        }
        
        public int X { get; private set; }
        public int Y { get; private set; }
        public char Value { get; private set; }
        public event EventHandler ColorChanged;

        public string Color
        {
            get => _color;
            set
            {
                _color = value;
                if (ColorChanged != null)
                {
                    ColorChanged(this, EventArgs.Empty);
                }
            }
        }
    }
}
