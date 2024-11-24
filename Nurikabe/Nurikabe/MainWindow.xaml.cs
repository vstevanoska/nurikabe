using Microsoft.Win32;
using System.Data.Common;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Nurikabe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<List<int>> state;      //current state of the board

        public MainWindow()
        {
            InitializeComponent();

            state = new List<List<int>>();
        }

        private void import_btn_Click(object sender, RoutedEventArgs e)
        {
            //select a nurikabe problem from the filesystem

            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

            bool? res = fileDialog.ShowDialog();

            if (res == true)
            {
                //load data from txt file and display state on grid

                using (StreamReader stream = new StreamReader(fileDialog.FileName))
                {
                    string str;
                    while ((str = stream.ReadLine()) != null)   //read line by line from file
                    {
                        var rowNums = str.Split(' ');           //split read line by whitespaces

                        List<int> list = new List<int>();

                        foreach (var number in rowNums)
                            list.Add(int.Parse(number));        //parse read number to int and save to list

                        state.Add(list);                        //add built list to current state of board
                    }
                }

                drawStartingState();
                //colorCell(0, 0, Brushes.Black);
            }
            else
            {
                MessageBox.Show("No file selected.");
            }
        }

        private void solve_btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void drawStartingState()
        {
            //add correct number of rows and columns to board
            //usually dimensions are nxn, mxn boards are also supported

            int rows = state.Count;
            int cols = state[0].Count;

            for (int i = 0; i < rows; ++i)
                board.RowDefinitions.Add(new RowDefinition());

            for (int i = 0; i < cols; ++i)
                board.ColumnDefinitions.Add(new ColumnDefinition());

            for (int i = 0; i < board.RowDefinitions.Count; ++i)
            {
                for (int j = 0; j < board.ColumnDefinitions.Count; ++j)
                {
                    Border border = new Border
                    {
                        BorderBrush = Brushes.Black,
                        BorderThickness = new System.Windows.Thickness(1),
                        Background = Brushes.White
                    };

                    if (state[i][j] != 0)
                    {
                        TextBlock textBlock = new TextBlock
                        {
                            Text = state[i][j].ToString(),
                            VerticalAlignment = VerticalAlignment.Center,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            FontSize = 40,
                            Background = Brushes.White
                        };

                        border.Child = textBlock;
                    }

                    Grid.SetRow(border, i);
                    Grid.SetColumn(border, j);
                    
                    board.Children.Add(border);
                }
            }
        }

        private void colorCell(int row, int column, Brush brush)
        {
            foreach (var cell in board.Children)
            {
                if (cell is Border border)
                {
                    border.Background = brush;

                    break;
                }    
            }
        }
    }
}