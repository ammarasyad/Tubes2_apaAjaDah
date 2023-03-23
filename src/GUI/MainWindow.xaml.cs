using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using DefaultNamespace;
using System.Threading;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static List<Node> _points = new();
        private string? _filePath;
        private static int _tickValue = 50;
        private char[,]? _fileMap;
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void RadioButtonChecked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton button)
            {
                //MessageBox.Show(button.Content.ToString());
                if (button.Content.ToString() == "BFS")
                {
                    Data.BFS = true;
                    Data.DFS = false;
                }
                else
                {
                    Data.BFS = false;
                    Data.DFS = true;
                }
                Console.WriteLine("BFS: " + Data.BFS);
                Console.WriteLine("DFS: " + Data.DFS);
            }
        }

        private void TickSlider_ValueChanged(object sender, RoutedEventArgs e)
        {
            if (sender is Slider slider)
            {
                _tickValue = (int)slider.Value;
            }
        }

        private void TSPChecked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox)
            {
                if (checkBox.IsChecked == true)
                {
                    Data.TSP = true;
                }
                else
                {
                    Data.TSP = false;
                }
            }
        }

        /// <summary>
        /// When clicking browse, an open file dialog will open and users can choose which (valid) file to open for the program.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Text files (*.txt)|*.txt";
            //dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            dialog.FilterIndex = 1;
            if (dialog.ShowDialog() == true)
            {
                DataContext = new Data { FileName = dialog.SafeFileName };
                _filePath = dialog.FileName;

                try
                {
                    _fileMap = Util.PopulateMapFromFile(_filePath);
                    ClearMatrix();
                    BuildAndPopulateMatrix(_fileMap, _fileMap.GetLength(0), _fileMap.GetLength(1));
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                }
            }
        }

        /// <summary>
        /// When the search button is clicked, the program begins to search for the appropriate path with the chosen method.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            if (_fileMap == null)
            {
                MessageBox.Show("Import a file first!");
                return;
            }
            ClearMatrix();
            BuildAndPopulateMatrix(_fileMap, _fileMap.GetLength(0), _fileMap.GetLength(1));
            Solution? solution = null;
            Tuple<int, int>? startPoint = null;
            int treasureCount = 0;
            GetStartingPointAndTreasureCount(_fileMap, ref startPoint, ref treasureCount);
            var treasureMap = new TreasureMap()
            {
                MapArr = _fileMap,
                StartPoint = startPoint,
                TreasureCount = treasureCount
            };
            if (Data.BFS)
            {
                var bfs = new BreadthSolver { TreasureMap = treasureMap };
                solution = bfs.Solve(Data.TSP);
                //MessageBox.Show(string.Join(Environment.NewLine, solution.Sequence));
            }
            else if (Data.DFS)
            {
                var dfs = new DepthSolver { TreasureMap = treasureMap };
                solution = dfs.Solve(Data.TSP);
            }
            ResultPanel.DataContext = new ResultData
            {
                Route = string.Join('-', solution.Path.ToArray()),
                Nodes = solution.NodesCheckedCount.ToString(),
                Steps = solution.Sequence.Count.ToString(),
                ExecutionTime = solution.ExecutionTime.ToString() + " ms"
            };
            Thread thread = new(() => DisplayMatrix(ref solution));
            thread.Start();
        }
        
        /// <summary>
        /// Method to display the matrix. Spawned with another thread to prevent a synchronization lock on the main thread.
        /// </summary>
        /// <param name="solution"></param>
        private static void DisplayMatrix(ref Solution solution)
        {
            foreach (Tuple<int, int> coord in solution.Sequence)
            {
                int x = coord.Item1;
                int y = coord.Item2;
                foreach (Node node in _points)
                {
                    if (node.X == x && node.Y == y)
                    {
                        node.Color = "Gold";
                        Thread.Sleep(_tickValue);
                        node.Color = node.Value switch
                        {
                            'K' => "LightSeaGreen",
                            'T' => "LimeGreen",
                            _ => "SkyBlue"
                        };
                        node.Visited++;
                    }
                }
            }
        }

        /// <summary>
        /// Clears the matrix.
        /// </summary>
        private void ClearMatrix()
        {
            Grid.DataContext = null;
            Grid.ItemsSource = null;
        }

        /// <summary>
        /// Determines the starting point and the treasure count. Used as a helper method for BFS and DFS.
        /// </summary>
        /// <param name="map">The imported file</param>
        /// <param name="startingPoint">Reference to the startingPoint tuple</param>
        /// <param name="treasureCount">Reference to the treasureCount integer</param>
        private void GetStartingPointAndTreasureCount(in char[,] map, ref Tuple<int, int>? startingPoint, ref int treasureCount)
        {
            treasureCount = 0;
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i, j] == 'K')
                    {
                        startingPoint = Tuple.Create(i, j);
                    }
                    else if (map[i, j] == 'T')
                    {
                        treasureCount++;
                    }
                }
            }
        }

        /// <summary>
        /// Builds (sets the appropriate rows and columns) and populates (creates each node) the matrix.
        /// </summary>
        /// <param name="map">The imported file</param>
        /// <param name="rows">Number of rows</param>
        /// <param name="cols">Number of columns</param>
        private void BuildAndPopulateMatrix(char[,] map, int rows, int cols)
        {
            Grid.DataContext = new MatrixData { Rows = cols, Columns = rows }; // Honestly idk why its inverted
            _points.Clear();
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    _points.Add(new Node(i, j, map[i, j]));
                }
            }
            Grid.ItemsSource = _points;
        }
    }
}
