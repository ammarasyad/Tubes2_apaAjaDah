using GUI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GUI
{
    public class Data : ViewModelBase
    {
        private string _fileName = "file.txt";
        public string FileName
        {
            get => _fileName;
            set => SetProperty(ref _fileName, value);
        }

        private static bool _bfs = true;
        private static bool _dfs = false;
        
        public static bool BFS
        {
            get => _bfs;
            set => _bfs = value;
        }

        public static bool DFS
        {
            get => _dfs;
            set => _dfs = value;
        }

        private static bool _tsp = false;

        public static bool TSP
        {
            get => _tsp;
            set => _tsp = value;
        }

        //public static bool BFS = true;
        //public static bool DFS = false;
        //public static bool TSP = false;
    }
}
