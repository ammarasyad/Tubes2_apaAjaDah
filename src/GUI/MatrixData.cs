using GUI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml, specifically for the matrix data.
    /// </summary>
    public class MatrixData : ViewModelBase
    {
        private int _row = 0;
        private int _col = 0;
        
        public int Rows
        {
            get => _row;
            set => SetProperty(ref _row, value);
        }
        
        public int Columns
        {
            get => _col;
            set => SetProperty(ref _row, value);
        }
    }
}
