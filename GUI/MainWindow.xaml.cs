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
        private char[,]? _fileMap;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void RadioButtonChecked(object sender, RoutedEventArgs e)
        {
            RadioButton? button = sender as RadioButton;
            if (button != null)
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

        private void TSPChecked(object sender, RoutedEventArgs e)
        {
            CheckBox? checkBox = sender as CheckBox;
            if (checkBox != null)
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
                    BuildAndPopulateMatrix(_fileMap, _fileMap.GetLength(0), _fileMap.GetLength(1));
                    MessageBox.Show("Visualize Done");
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                }
            }
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            if (_fileMap == null)
            {
                MessageBox.Show("Import a file first!");
                return;
            }
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
            Thread thread = new(() => UpdateMatrix(ref solution));
            thread.Start();
        }
        
        private static void UpdateMatrix(ref Solution solution)
        {
            foreach (Tuple<int, int> coord in solution.Sequence)
            {
                int x = coord.Item1;
                int y = coord.Item2;
                foreach (Node node in _points)
                {
                    if (node.X == x && node.Y == y)
                    {
                        switch (node.Value)
                        {
                            case 'K':
                                node.Color = "LightSeaGreen";
                                break;
                            case 'T':
                                node.Color = "Crimson";
                                break;
                            default:
                                node.Color = "LightSkyBlue";
                                break;
                        }
                        Thread.Sleep(100);
                        break;
                    }
                }
            }
        }

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

        // TODO: Fix this
        private void BuildAndPopulateMatrix(char[,] map, int rows, int cols)
        {
            //List<char> mapList = map.Cast<char>().ToList();
            //List<List<string?>> mapList = new();
            //for (int i = 0; i < height; i++)
            //{
            //    mapList.Add(new List<string?>());

            //    for (int j = 0; j < width; j++)
            //    {
            //        switch (map[i, j])
            //        {
            //            case 'K':
            //                mapList[i].Add("start");
            //                break;
            //            case 'T':
            //                mapList[i].Add("treasure");
            //                break;
            //            case 'R':
            //                mapList[i].Add("");
            //                break;
            //            default:
            //                mapList[i].Add(null);
            //                break;
            //        }
            //    }
            //}
            //Grid.ItemsSource = mapList;
            Grid.DataContext = new MatrixData { Rows = cols, Columns = rows }; // Honestly idk why its inverted
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    _points.Add(new Node(i, j, map[i, j]));
                    //switch (map[i, j])
                    //{
                    //    case 'K':
                    //        _points.Add(new Node(i, j) { Color = "LightBlue" });
                    //        break;
                    //    case 'R':
                    //        _points.Add(new Node(i, j) { Color = "Transparent" });
                    //        break;
                    //    case 'T':
                    //        _points.Add(new Node(i, j) { Color = "LightYellow" });
                    //        break;
                    //    default:
                    //        _points.Add(new Node(i, j) { Color = "Black" });
                    //        break;
                    //}
                }
            }
            Grid.ItemsSource = _points;
        }
    }
}
