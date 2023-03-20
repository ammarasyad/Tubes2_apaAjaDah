using App.WPF.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.WPF
{
    public class Data : ViewModelBase
    {
        private string _fileName = "file.txt";
        public string FileName
        {
            get => _fileName;
            set
            {
                if (_fileName != value)
                {
                    _fileName = value;
                    OnPropertyChanged(nameof(FileName));
                }
            }
        }
        
        private static bool _BFS = true;
        private static bool _DFS = false;
        public static bool BFS
        {
            get => _BFS;
            set
            {
                if (_BFS != value)
                {
                    _BFS = value;
                }
            }
        }

        public static bool DFS
        {
            get => _DFS;
            set
            {
                if (_DFS != value)
                {
                    _DFS = value;
                }
            }
        }
    }
}
