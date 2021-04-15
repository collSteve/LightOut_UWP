using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Diagnostics;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace LightOut_UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        float gameBoardColumnSpacing = 5;
        float gameBoardRowSpacing = 5;

        int[,] lightMatrix = new int[,] { 
            { 1, 0, 1, 1, 1 }, 
            { 0, 0, 0, 1, 1 }, 
            { 1, 0, 1, 1, 1 } };

        Light[,] lights;

        public MainPage()
        {
            this.InitializeComponent();

            NewStart();

            Window.Current.SizeChanged += OnWindowSizeChanged;
            restartBtn.Click += OnRestart;
            
        }

        void OnRestart(object sender, RoutedEventArgs e)
        {
            NewStart();
        }

        void NewStart()
        {
            InitializeGameBoard(lightMatrix, GameBoard);
            lights = InitializeLights(lightMatrix, lights, GameBoard);
            GameStatueText.Text = "Turn off the Lights";
            restartBtn.Visibility = Visibility.Collapsed;
        }

        void OnWindowSizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            ResizeGameBoardByWindowSize();
            //Debug.WriteLine($"Smaller:{smallerDimension}, boardWidth: {gameBoardWidth}, boardHeight: {gameBoardHeight}");
        }

        void ResizeGameBoardByWindowSize()
        {
            // resize game board
            float maxHeight = (float)Window.Current.Bounds.Height * 1f / 2f; // margin 0.5
            float maxWidth = (float)Window.Current.Bounds.Width * 0.9f; // margin 0.9
            //float smallerDimension = windowHeight > windowWidth ? windowWidth : windowHeight;

            int numRows = lightMatrix.GetLength(0);
            int numCols = lightMatrix.GetLength(1);
            float rowSize = maxHeight / numRows;
            float colSize = maxWidth / numCols;

            if (rowSize > colSize)
            {
                GameBoard.Height = numRows * colSize;
                GameBoard.Width = numCols * colSize;
            }
            else
            {
                GameBoard.Height = numRows * rowSize;
                GameBoard.Width = numCols * rowSize;
            }
        }

        void InitializeGameBoard(int[,] matrix, Grid board)
        {
            // empty game board
            board.Children.Clear();
            board.RowDefinitions.Clear();
            board.ColumnDefinitions.Clear();

            // style
            ResizeGameBoardByWindowSize();

            // initialize number of rows
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                board.RowDefinitions.Add(new RowDefinition() 
                    { Height = new GridLength(1.0, GridUnitType.Star) });
            }
            // initialize number of columns
            for (int i = 0; i < matrix.GetLength(1); i++)
            {
                board.ColumnDefinitions.Add(new ColumnDefinition()
                { Width = new GridLength(1.0, GridUnitType.Star) });
            }
        }

        /*
         * Returns the light collect array
         */
        Light[,] InitializeLights(int[,] matrix, Light[,] lightCollection, Grid boardGrid)
        {
            int numRows = matrix.GetLength(0);
            int numCols = matrix.GetLength(1);

            // empty the light collection
            lightCollection = new Light[numRows, numCols];

            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numCols; j++)
                {
                    Light light = new Light(matrix[i, j] == 1);
                    boardGrid.Children.Add(light);

                    // set position of the light
                    Grid.SetRow(light, i);
                    Grid.SetColumn(light, j);

                    light.position = new int[2] { i, j };
                    // subscribe light's click event with game event handler
                    light.Click += OnLightClicked;

                    // add light to lights collection
                    lightCollection[i, j] = light;
                }
            }
            return lightCollection;
        }

        void OnLightClicked(Object sender, RoutedEventArgs e)
        { 
            if (!(sender is Light))
            {
                return;
            }
            Light s;
            s = (Light) sender;

            int[] lightPosition = s.position;
            // lightMatrix[lightPosition[0], lightPosition[1]] = s.lightOn ? 1 : 0;

            TurnOffNeighborLights(lights, lightPosition[0], lightPosition[1]);
            // wining evaluation 
            if (GameWon(lights)) 
            {
                GameStatueText.Text = "WooHoo! You Won!";
                restartBtn.Visibility = Visibility.Visible;
                DisableLightClick(lights);
            }
        }

        void TurnOffNeighborLights(Light[,] lightCollection, int row, int col)
        {
            if (lightCollection.GetLength(0) !=0 && lightCollection.GetLength(1) !=0)
            {
                if (row != 0 && lightCollection[row - 1, col].ClickEnabled)
                {
                    lightCollection[row - 1, col].ToggleLight();
                }
                if (col != 0 && lightCollection[row, col - 1].ClickEnabled)
                {
                    lightCollection[row, col - 1].ToggleLight();
                }
                if (row != lightCollection.GetLength(0) - 1 && lightCollection[row + 1, col].ClickEnabled)
                {
                    lightCollection[row + 1, col].ToggleLight();
                }
                if (col != lightCollection.GetLength(1) - 1 && lightCollection[row, col+1].ClickEnabled)
                {
                    lightCollection[row, col + 1].ToggleLight();
                }
            }
        }

        /*
         * goalState = false to check if all lights off 
         * goalState = true to check is all lights on
         */
        bool GameWon(Light[,] lightCollection, bool goalState= false)
        {
            foreach (Light light in lightCollection)
            {
                if (light.lightOn != goalState)
                {
                    return false;
                }
            }
            return true;
        }

        void DisableLightClick(Light[,] lightCollection)
        {
            foreach (Light light in lightCollection)
            {
                light.ClickEnabled = false;
            }
        }
    }

    

}
