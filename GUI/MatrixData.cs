using GUI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI
{
    public class MatrixData : ViewModelBase
    {
        private string _route = "";
        private string _steps = "";
        private string _nodes = "";
        private string _executionTime = "";

        public string Route
        {
            get => _route;
            set => SetProperty(ref _route, value);
        }

        public string Steps
        {
            get => _steps;
            set => SetProperty(ref _steps, value);
        }

        public string Nodes
        {
            get => _nodes;
            set => SetProperty(ref _nodes, value);
        }

        public string ExecutionTime
        {
            get => _executionTime;
            set => SetProperty(ref _executionTime, value);
        }
    }
}
