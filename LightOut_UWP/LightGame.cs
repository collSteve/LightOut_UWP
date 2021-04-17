using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace LightOut_UWP
{
    class LightGame : Game
    {
        private int[,] lightMatrix;
        private Light[,] lights;

        

        public LightGame(int[,] lightMatrix)
        {
            this.lightMatrix = lightMatrix;
            // set default row apcing and column spacing for light game
            gameBoardRowSpacing = 5f;
            gameBoardColumnSpacing = 5f;
            GameDescription = "Turn off the Lights";
        }

        public override void InitializeGame(Grid gameBoard, TextBlock gameStatueText, Button restartButton)
        {
            // hook up controls 
            SetUpGameControls(gameBoard, gameStatueText, restartButton);

            InitializeGameBoard(lightMatrix, GameBoard);
            lights = InitializeLights(lightMatrix, lights, GameBoard);
        }

        void InitializeGameBoard(int[,] matrix, Grid board)
        {
            // empty game board
            board.Children.Clear();
            board.RowDefinitions.Clear();
            board.ColumnDefinitions.Clear();

            // style
            ResizeGameBoardByWindowSize();
            board.RowSpacing = gameBoardRowSpacing;
            board.ColumnSpacing = gameBoardColumnSpacing;

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

        protected override void ResizeGameBoardByWindowSize()
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

        private Light[,] InitializeLights(int[,] matrix, Light[,] lightCollection, Grid boardGrid)
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
            s = (Light)sender;

            int[] lightPosition = s.position;
            // lightMatrix[lightPosition[0], lightPosition[1]] = s.lightOn ? 1 : 0;

            TurnOffNeighborLights(lights, lightPosition[0], lightPosition[1]);
            // wining evaluation 
            if (GameWon(lights))
            {
                // create event args
                GameEndEventArgs data = new GameEndEventArgs();
                data.gameResult = GameResult.Succeeded;

                // fire game ended event
                OnGameEnded(data);

                DisableLightClick(lights);
            }
        }

        void TurnOffNeighborLights(Light[,] lightCollection, int row, int col)
        {
            if (lightCollection.GetLength(0) != 0 && lightCollection.GetLength(1) != 0)
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
                if (col != lightCollection.GetLength(1) - 1 && lightCollection[row, col + 1].ClickEnabled)
                {
                    lightCollection[row, col + 1].ToggleLight();
                }
            }
        }

        /*
         * goalState = false to check if all lights off 
         * goalState = true to check is all lights on
         */
        bool GameWon(Light[,] lightCollection, bool goalState = false)
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
