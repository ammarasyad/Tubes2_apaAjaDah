using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GUI
{
    public class Node : Button
    {
        public static readonly DependencyProperty dependencyProperty = DependencyProperty.Register("NodeColor", typeof(bool), typeof(Node), new PropertyMetadata(false));

        public bool NodeColor
        {
            get => (bool)GetValue(dependencyProperty);
            set => SetValue(dependencyProperty, value);
        }
        
        public Node() : base()
        {
            
            if (Content == null)
            {
                NodeColor = false;
            }
            else
            {
                NodeColor = true;
            }
        }
    }
}
