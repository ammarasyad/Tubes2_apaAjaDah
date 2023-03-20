using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace App.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        
        private void RadioButtonChecked(object sender, RoutedEventArgs e)
        {
            RadioButton button = sender as RadioButton;
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

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Text files (*.txt)|*.txt";
            //dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            dialog.FilterIndex = 1;
            if (dialog.ShowDialog() == true)
            {
                DataContext = new Data { FileName = dialog.SafeFileName };
            }
        }
    }
}
